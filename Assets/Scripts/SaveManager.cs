using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Save manager class handles the saving and loading of the game.
/// Can have multiple save files starting from index 1.
/// Save file 0 is reserved for autosave.
/// </summary>
public static class SaveManager {

	static int MaxSaves = 10;
	static string saveDirectory = "[]saves/"; // Directory for save games.
	static string filepath_format = saveDirectory + "save{0}.sav"; // {0} replaced by save file id.

	/// <summary>
	/// Creates save directory if does not exist.
	/// </summary>
	static SaveManager() {
		if (!FileHandler.DirectoryExists (saveDirectory))
			FileHandler.CreateDirectory (saveDirectory);
	}

	/// <summary>
	/// Save game to the specified saveFileId.
	/// </summary>
	/// <param name="saveFileId">Index of save file. 0 for autosave</param>
	/// <returns><c>true</c> if save was successful, <c>false</c> otherwise.
	public static bool Save(int saveFileId) {
		if (saveFileId < 0) {
			Debug.LogError ("Invalid save file id.");
			return false;
		}
		string filepath;
		if (saveFileId == 0)
			filepath = saveDirectory + "Autosave.sav";
		else
			filepath = String.Format (filepath_format, saveFileId);
		StorageContainer sc = new StorageContainer ();
		return FileHandler.SaveFile (filepath, sc);
	}

	/// <summary>
	/// Load game from the specified saveFileId.
	/// </summary>
	/// <param name="saveFileId">Index of save file. 0 for autosave</param>
	/// <returns><c>true</c> if load was successful, <c>false</c> otherwise.
	public static bool Load(int saveFileId) {
		if (saveFileId < 0) {
			Debug.LogError ("Invalid save file id.");
			return false;
		}
		string filepath;
		if (saveFileId == 0)
			filepath = saveDirectory + "Autosave.sav";
		else
			filepath = String.Format (filepath_format, saveFileId);
		if (!FileHandler.FileExists (filepath)) {
			Debug.LogError ("Save file does not exist.");
			return false;
		}
		StorageContainer sc = FileHandler.ReadFile<StorageContainer> (filepath);
		return sc.Load ();
	}

}