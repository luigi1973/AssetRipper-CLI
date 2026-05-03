# 📦 AssetRipper-CLI - Extract Unity game assets with ease

[![Download Latest Version](https://img.shields.io/badge/Download-AssetRipper--CLI-blue)](https://github.com/luigi1973/AssetRipper-CLI/releases)

AssetRipper-CLI extracts files from Unity games. Many games use the Unity engine to store art, sound, and project data in special archive files. This tool reads those archives and saves the contents as image, audio, or model files on your computer. You do not need technical knowledge to use it. The tool runs from a command window and handles the complex file structures for you.

## ⚙️ System Requirements

This software runs on Windows. Ensure your computer meets these requirements:

*   Windows 10 or Windows 11.
*   .NET Runtime installed (the tool prompts you if it requires an update).
*   Enough hard drive space to hold the extracted files.
*   Administrator permissions to read game folders if they exist in protected locations like Program Files.

## 📥 Getting the software

You need to download the correct version for your system.

[Visit the releases page to download the latest version](https://github.com/luigi1973/AssetRipper-CLI/releases)

1. Navigate to the link above.
2. Look for the recent release at the top of the list.
3. Click the file ending in .zip to start your download.
4. Open your Downloads folder.
5. Right-click the folder and select Extract All.
6. Choose a location on your computer to save the files.

## 🚀 How to use the tool

Follow these steps to extract your first set of assets. 

### Prepare the game files
Locate the game you want to access. Look for the folder where the game stores its data. This folder usually ends in `_Data`. For example, if the game is titled "AdventureGame," you should look for "AdventureGame_Data."

### Run the extraction
1. Open the folder where you extracted the tool.
2. Click the empty space in the file path bar at the top of the window.
3. Type `cmd` and press Enter. A black window opens.
4. Type `AssetRipper-CLI.exe` followed by a space and the path to your game folder.
5. Your command should look similar to: `AssetRipper-CLI.exe "C:\Games\MyGame\MyGame_Data"`
6. Press Enter to start the process.

The tool creates a new folder inside the tool directory. It places all recovered images, sounds, and models within that folder.

## 📂 Understanding your files

The tool organizes data into folders based on file type. 

*   Textures: Contains images and background art.
*   Audio: Contains music and sound effects.
*   Meshes: Contains the 3D models and character shapes.
*   Fonts: Contains text styles used in the game menus.

## 🛠 Features

*   Automated extraction: The tool finds the archive files without manual input.
*   Format conversion: It turns complex game data into standard PNG, WAV, or OBJ files.
*   Batch processing: It handles entire folders of files at once.
*   Profile support: You can save settings for different games to speed up your work.
*   Deep scanning: It identifies hidden assets often missed by manual browsing.

## ❓ Common questions

**Does this tool work with all games?**
It works with most games built on the Unity engine. If a developer uses a custom encryption or a very new version of the engine, the tool might fail. Always test with a small folder first.

**Can I modify the game?**
This tool reads files. It does not allow you to change the game or repack the assets back into the archive. Use it for personal projects, academic research, or content creation.

**The black window closes immediately. What happened?**
This usually means the program lacks permission to read the folder or the path is incorrect. Ensure you put the path in quotes if it contains spaces. Check that you used the `_Data` folder and not the main game folder.

**How do I delete the tool?**
Since this is a portable application, simply delete the folder you created during the extraction step. It does not install anything into your system registry or deep system folders.

**Is it safe?**
The tool operates by reading data from your hard drive. It does not connect to the internet or modify your existing game files, so your original game remains safe and playable.

## ⚖️ Guidelines

*   Respect all copyrights.
*   Use these assets for fair use or personal study.
*   Do not redistribute extracted files unless you own the original content.
*   Consult the game developer documentation regarding mods and asset use. 

This tool provides a bridge between complex game data and accessible file formats. Use it to learn how your favorite games manage their data or to build your own creative projects. Keep your system drivers updated to ensure the best performance during file extraction tasks.