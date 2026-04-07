using AssetRipper.Assets.Collections;
using AssetRipper.Assets.IO;
using AssetRipper.IO.Files;
using AssetRipper.IO.Files.CompressedFiles;
using AssetRipper.IO.Files.ResourceFiles;
using AssetRipper.IO.Files.SerializedFiles;
using AssetRipper.IO.Files.SerializedFiles.Parser;
using System.Collections.Concurrent;

namespace AssetRipper.Assets.Bundles;

partial class GameBundle
{
	/// <summary>
	/// Create and initialize a <see cref="GameBundle"/> from a set of paths.
	/// </summary>
	/// <param name="paths">The set of paths to load.</param>
	/// <param name="assetFactory">The factory for reading assets.</param>
	/// <param name="dependencyProvider"></param>
	/// <param name="resourceProvider"></param>
	/// <param name="defaultVersion">The default version to use if a file does not have a version, ie the version has been stripped.</param>
	public static GameBundle FromPaths(IEnumerable<string> paths, AssetFactoryBase assetFactory, FileSystem fileSystem, IGameInitializer? initializer = null)
	{
		GameBundle gameBundle = new();
		initializer?.OnCreated(gameBundle, assetFactory);
		gameBundle.InitializeFromPaths(paths, assetFactory, fileSystem, initializer);
		initializer?.OnPathsLoaded(gameBundle, assetFactory);
		gameBundle.InitializeAllDependencyLists(initializer?.DependencyProvider);
		initializer?.OnDependenciesInitialized(gameBundle, assetFactory);
		return gameBundle;
	}

	private void InitializeFromPaths(IEnumerable<string> paths, AssetFactoryBase assetFactory, FileSystem fileSystem, IGameInitializer? initializer)
	{
		ResourceProvider = initializer?.ResourceProvider;
		List<FileBase> fileStack = LoadFilesAndDependencies(paths, fileSystem, initializer?.DependencyProvider);
		UnityVersion defaultVersion = initializer is null ? default : initializer.DefaultVersion;

		while (fileStack.Count > 0)
		{
			switch (RemoveLastItem(fileStack))
			{
				case SerializedFile serializedFile:
					SerializedAssetCollection.FromSerializedFile(this, serializedFile, assetFactory, defaultVersion);
					break;
				case FileContainer container:
					SerializedBundle serializedBundle = SerializedBundle.FromFileContainer(container, assetFactory, defaultVersion);
					AddBundle(serializedBundle);
					break;
				case ResourceFile resourceFile:
					AddResource(resourceFile);
					break;
				case FailedFile failedFile:
					AddFailed(failedFile);
					break;
			}
		}
	}

	private static FileBase RemoveLastItem(List<FileBase> list)
	{
		int index = list.Count - 1;
		FileBase file = list[index];
		list.RemoveAt(index);
		return file;
	}

	private static List<FileBase> LoadFilesAndDependencies(IEnumerable<string> paths, FileSystem fileSystem, IDependencyProvider? dependencyProvider)
	{
		List<string> pathList = paths.Distinct().ToList();
		ConcurrentBag<FileBase> files = new();
		ConcurrentDictionary<string, byte> serializedFileNames = new();//Includes missing dependencies
		int workerCount = GetWorkerCount("ASSETRIPPER_LOAD_WORKERS", fallback: 2);

		Parallel.ForEach(pathList, new ParallelOptions() { MaxDegreeOfParallelism = workerCount }, path =>
		{
			List<FileBase> localFiles = new();
			FileBase? file;
			try
			{
				file = SchemeReader.LoadFile(path, fileSystem);
				file.ReadContentsRecursively();
			}
			catch (Exception ex)
			{
				file = new FailedFile()
				{
					Name = fileSystem.Path.GetFileName(path),
					FilePath = path,
					StackTrace = ex.ToString(),
				};
			}
			while (file is CompressedFile compressedFile)
			{
				file = compressedFile.UncompressedFile;
			}
			if (file is ResourceFile or FailedFile)
			{
				localFiles.Add(file);
			}
			else if (file is SerializedFile serializedFile)
			{
				localFiles.Add(file);
				serializedFileNames.TryAdd(serializedFile.NameFixed, 0);
			}
			else if (file is FileContainer container)
			{
				localFiles.Add(file);
				foreach (SerializedFile serializedFileInContainer in container.FetchSerializedFiles())
				{
					serializedFileNames.TryAdd(serializedFileInContainer.NameFixed, 0);
				}
			}
			foreach (FileBase localFile in localFiles)
			{
				files.Add(localFile);
			}
		});

		List<FileBase> fileList = files.ToList();

		for (int i = 0; i < fileList.Count; i++)
		{
			FileBase file = fileList[i];
			if (file is SerializedFile serializedFile)
			{
				LoadDependencies(serializedFile, fileList, serializedFileNames, dependencyProvider);
			}
			else if (file is FileContainer container)
			{
				foreach (SerializedFile serializedFileInContainer in container.FetchSerializedFiles())
				{
					LoadDependencies(serializedFileInContainer, fileList, serializedFileNames, dependencyProvider);
				}
			}
		}

		return fileList;
	}

	private static void LoadDependencies(SerializedFile serializedFile, List<FileBase> files, ConcurrentDictionary<string, byte> serializedFileNames, IDependencyProvider? dependencyProvider)
	{
		foreach (FileIdentifier fileIdentifier in serializedFile.Dependencies)
		{
			string name = fileIdentifier.GetFilePath();
			if (serializedFileNames.TryAdd(name, 0) && dependencyProvider?.FindDependency(fileIdentifier) is { } dependency)
			{
				files.Add(dependency);
			}
		}
	}

	private static int GetWorkerCount(string envKey, int fallback)
	{
		if (int.TryParse(Environment.GetEnvironmentVariable(envKey), out int explicitValue) && explicitValue > 0)
		{
			return explicitValue;
		}
		if (int.TryParse(Environment.GetEnvironmentVariable("ASSETRIPPER_WORKERS"), out int sharedValue) && sharedValue > 0)
		{
			return sharedValue;
		}

		return Math.Max(1, Math.Min(fallback, Environment.ProcessorCount));
	}
}
