using AssetRipper.Assets;
using AssetRipper.Assets.Bundles;
using AssetRipper.Export.Configuration;
using AssetRipper.Export.PrimaryContent.Audio;
using AssetRipper.Export.PrimaryContent.DeletedAssets;
using AssetRipper.Export.PrimaryContent.Models;
using AssetRipper.Export.PrimaryContent.Scripts;
using AssetRipper.Export.PrimaryContent.Textures;
using AssetRipper.Import.Configuration;
using AssetRipper.Import.Logging;
using AssetRipper.Processing;
using AssetRipper.Processing.Prefabs;
using AssetRipper.Processing.Textures;
using AssetRipper.SourceGenerated.Classes.ClassID_1;
using AssetRipper.SourceGenerated.Classes.ClassID_1032;
using AssetRipper.SourceGenerated.Classes.ClassID_1101;
using AssetRipper.SourceGenerated.Classes.ClassID_1102;
using AssetRipper.SourceGenerated.Classes.ClassID_1107;
using AssetRipper.SourceGenerated.Classes.ClassID_1109;
using AssetRipper.SourceGenerated.Classes.ClassID_111;
using AssetRipper.SourceGenerated.Classes.ClassID_1111;
using AssetRipper.SourceGenerated.Classes.ClassID_1120;
using AssetRipper.SourceGenerated.Classes.ClassID_115;
using AssetRipper.SourceGenerated.Classes.ClassID_128;
using AssetRipper.SourceGenerated.Classes.ClassID_150;
using AssetRipper.SourceGenerated.Classes.ClassID_152;
using AssetRipper.SourceGenerated.Classes.ClassID_156;
using AssetRipper.SourceGenerated.Classes.ClassID_189;
using AssetRipper.SourceGenerated.Classes.ClassID_2;
using AssetRipper.SourceGenerated.Classes.ClassID_206;
using AssetRipper.SourceGenerated.Classes.ClassID_21;
using AssetRipper.SourceGenerated.Classes.ClassID_221;
using AssetRipper.SourceGenerated.Classes.ClassID_238;
using AssetRipper.SourceGenerated.Classes.ClassID_3;
using AssetRipper.SourceGenerated.Classes.ClassID_329;
using AssetRipper.SourceGenerated.Classes.ClassID_43;
using AssetRipper.SourceGenerated.Classes.ClassID_49;
using AssetRipper.SourceGenerated.Classes.ClassID_72;
using AssetRipper.SourceGenerated.Classes.ClassID_74;
using AssetRipper.SourceGenerated.Classes.ClassID_83;
using AssetRipper.SourceGenerated.Classes.ClassID_90;
using AssetRipper.SourceGenerated.Classes.ClassID_91;
using AssetRipper.SourceGenerated.Classes.ClassID_93;
using AssetRipper.SourceGenerated.Classes.ClassID_95;
using System.Collections.Concurrent;
using System.Threading;

namespace AssetRipper.Export.PrimaryContent;

public sealed class PrimaryContentExporter
{
	private readonly ObjectHandlerStack<IContentExtractor> exporters = new();
	private readonly GameData gameData;

	private PrimaryContentExporter(GameData gameData)
	{
		this.gameData = gameData;
	}

	public void RegisterHandler<T>(IContentExtractor handler, bool allowInheritance = true) where T : IUnityObjectBase
	{
		exporters.OverrideHandler(typeof(T), handler, allowInheritance);
	}

	public void RegisterHandler(Type type, IContentExtractor handler, bool allowInheritance = true)
	{
		exporters.OverrideHandler(type, handler, allowInheritance);
	}

	public static PrimaryContentExporter CreateDefault(GameData gameData, FullConfiguration settings)
	{
		PrimaryContentExporter exporter = new(gameData);
		exporter.RegisterDefaultHandlers(settings);
		return exporter;
	}

	private void RegisterDefaultHandlers(FullConfiguration settings)
	{
		RegisterHandler<IUnityObjectBase>(new JsonContentExtractor());

		RegisterEmptyHandler<IAnimation>();
		RegisterEmptyHandler<IAnimationClip>();
		RegisterEmptyHandler<IAnimator>();
		RegisterEmptyHandler<IAnimatorController>();
		RegisterEmptyHandler<IAnimatorOverrideController>();
		RegisterEmptyHandler<IAnimatorState>();
		RegisterEmptyHandler<IAnimatorStateMachine>();
		RegisterEmptyHandler<IAnimatorStateTransition>();
		RegisterEmptyHandler<IAnimatorTransition>();
		RegisterEmptyHandler<IAnimatorTransitionBase>();
		RegisterEmptyHandler<IAvatar>();
		RegisterEmptyHandler<IBlendTree>();
		RegisterEmptyHandler<IComponent>();
		RegisterEmptyHandler<IComputeShader>();
		RegisterEmptyHandler<ILightingDataAsset>();
		RegisterEmptyHandler<IMaterial>();
		RegisterEmptyHandler<IPreloadData>();
		RegisterEmptyHandler<IRuntimeAnimatorController>();
		RegisterEmptyHandler<ISceneAsset>();
		RegisterEmptyHandler<SpriteInformationObject>();

		GlbModelExporter modelExporter = new();
		RegisterHandler<GameObjectHierarchyObject>(modelExporter);
		RegisterHandler<IGameObject>(modelExporter);
		RegisterHandler<IComponent>(modelExporter);
		RegisterHandler<ILevelGameManager>(modelExporter);

		RegisterHandler<IMesh>(new GlbMeshExporter());

		RegisterHandler<INavMeshData>(new GlbNavMeshExporter());
		RegisterHandler<ITerrainData>(new GlbTerrainExporter());

		RegisterHandler<ITextAsset>(BinaryAssetContentExtractor.Instance);
		RegisterHandler<IFont>(BinaryAssetContentExtractor.Instance);
		RegisterHandler<IMovieTexture>(BinaryAssetContentExtractor.Instance);
		RegisterHandler<IVideoClip>(BinaryAssetContentExtractor.Instance);

		RegisterHandler<IAudioClip>(new AudioContentExtractor());

		RegisterHandler<IImageTexture>(new TextureExporter(settings.ExportSettings.ImageExportFormat));

		RegisterHandler<IMonoScript>(new ScriptContentExtractor(gameData.AssemblyManager, settings.ExportSettings.ScriptLanguageVersion.ToCSharpLanguageVersion(gameData.ProjectVersion)));

		// Deleted assets
		// This must be the last handler
		RegisterHandler<IUnityObjectBase>(DeletedAssetsExporter.Instance);
	}

	public PrimaryExportStats Export(GameBundle fileCollection, FullConfiguration settings, FileSystem fileSystem, Func<ExportCollectionBase, ExportCollectionSelectionDecision>? selectionPredicate = null)
	{
		List<ExportCollectionBase> allCollections = CreateCollections(fileCollection)
			.Where(c => c.Exportable)
			.ToList();
		List<ExportCollectionBase> collections = [];
		List<PrimarySkippedCollection> skippedCollections = [];

		if (selectionPredicate is null)
		{
			collections.AddRange(allCollections);
		}
		else
		{
			foreach (ExportCollectionBase collection in allCollections)
			{
				ExportCollectionSelectionDecision decision = selectionPredicate(collection);
				if (decision.Include)
				{
					collections.Add(collection);
				}
				else
				{
					skippedCollections.Add(new PrimarySkippedCollection(
						Name: collection.Name,
						ClassName: collection.ExportableAssets.FirstOrDefault()?.ClassName ?? "Unknown",
						Directory: collection.ExportableAssets.FirstOrDefault()?.GetBestDirectory() ?? string.Empty,
						Reason: decision.Reason ?? "excluded-by-selection"));
				}
			}
		}
		ConcurrentDictionary<string, object> exportLocks = new(StringComparer.OrdinalIgnoreCase);
		ConcurrentBag<PrimaryFailedCollection> failedCollections = [];
		int totalCount = collections.Count;
		int completedCount = 0;
		int failedCount = 0;
		int workerCount = GetWorkerCount("ASSETRIPPER_EXPORT_WORKERS", fallback: 4);

		if (selectionPredicate is not null)
		{
			Logger.Info(LogCategory.Export, $"Selected {totalCount} of {allCollections.Count} primary collections for export.");
		}

		Logger.Info(LogCategory.Export, $"Exporting {totalCount} primary collections with up to {workerCount} worker(s).");

		if (totalCount == 0)
		{
				return new PrimaryExportStats(
					TotalCollections: allCollections.Count,
					SelectedCollections: 0,
					SkippedBySelection: allCollections.Count,
					FailedCollections: 0,
					SkippedCollections: skippedCollections,
					FailedCollectionDetails: []);
		}

		Parallel.ForEach(collections, new ParallelOptions() { MaxDegreeOfParallelism = workerCount }, collection =>
		{
			string exportKey = collection.GetExportKey(settings.ExportRootPath, fileSystem);
			object exportLock = exportLocks.GetOrAdd(exportKey, static _ => new object());
			bool exportedSuccessfully;
			lock (exportLock)
			{
				exportedSuccessfully = collection.Export(settings.ExportRootPath, fileSystem);
			}
			if (!exportedSuccessfully)
			{
				Interlocked.Increment(ref failedCount);
				failedCollections.Add(new PrimaryFailedCollection(
					Name: collection.Name,
					ClassName: collection.ExportableAssets.FirstOrDefault()?.ClassName ?? "Unknown",
					Directory: collection.ExportableAssets.FirstOrDefault()?.GetBestDirectory() ?? string.Empty,
					Reason: "exporter-returned-false"));
				Logger.Warning(LogCategory.ExportProgress, $"Failed to export '{collection.Name}'");
			}

			int currentCount = Interlocked.Increment(ref completedCount);
			if (ShouldLogProgress(currentCount, totalCount))
			{
				Logger.Info(LogCategory.ExportProgress, $"({currentCount}/{totalCount}) Exported '{collection.Name}'");
			}
		});

		if (failedCount > 0)
		{
			Logger.Warning(LogCategory.Export, $"{failedCount} primary collection(s) failed to export.");
		}

			return new PrimaryExportStats(
				TotalCollections: allCollections.Count,
				SelectedCollections: totalCount,
				SkippedBySelection: Math.Max(0, allCollections.Count - totalCount),
				FailedCollections: failedCount,
				SkippedCollections: skippedCollections,
				FailedCollectionDetails: failedCollections.ToArray());
	}

	private List<ExportCollectionBase> CreateCollections(GameBundle fileCollection)
	{
		List<ExportCollectionBase> collections = new();
		HashSet<IUnityObjectBase> queued = new();

		foreach (IUnityObjectBase asset in fileCollection.FetchAssets())
		{
			if (!queued.Add(asset))
			{
				// Skip duplicates
				continue;
			}

			ExportCollectionBase collection = CreateCollection(asset);
			if (collection is EmptyExportCollection)
			{
				// Skip empty collections. The asset has already been added to the hash set.
				continue;
			}

			foreach (IUnityObjectBase element in collection.Assets)
			{
				queued.Add(element);
			}
			collections.Add(collection);
		}

		return collections;
	}

	private ExportCollectionBase CreateCollection(IUnityObjectBase asset)
	{
		foreach (IContentExtractor exporter in exporters.GetHandlerStack(asset.GetType()))
		{
			if (exporter.TryCreateCollection(asset, out ExportCollectionBase? collection))
			{
				return collection;
			}
		}
		throw new Exception($"There is no exporter that can handle '{asset}'");
	}

	private void RegisterEmptyHandler<T>() where T : IUnityObjectBase
	{
		RegisterHandler<T>(EmptyContentExtractor.Instance);
	}

	private static bool ShouldLogProgress(int currentCount, int totalCount)
	{
		if (currentCount <= 10 || currentCount == totalCount)
		{
			return true;
		}

		int interval = totalCount switch
		{
			>= 10000 => 500,
			>= 5000 => 250,
			>= 1000 => 100,
			_ => 25,
		};
		return currentCount % interval == 0;
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

public readonly record struct PrimaryExportStats(
	int TotalCollections,
	int SelectedCollections,
	int SkippedBySelection,
	int FailedCollections,
	IReadOnlyList<PrimarySkippedCollection> SkippedCollections,
	IReadOnlyList<PrimaryFailedCollection> FailedCollectionDetails);

public readonly record struct ExportCollectionSelectionDecision(bool Include, string? Reason)
{
	public static ExportCollectionSelectionDecision Included() => new(true, null);
	public static ExportCollectionSelectionDecision Excluded(string reason) => new(false, reason);
}

public readonly record struct PrimarySkippedCollection(
	string Name,
	string ClassName,
	string Directory,
	string Reason);

public readonly record struct PrimaryFailedCollection(
	string Name,
	string ClassName,
	string Directory,
	string Reason);
