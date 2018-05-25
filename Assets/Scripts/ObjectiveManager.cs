using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objective manager class manages adding, removing and updating of objectives.
/// Also manages the saving/loading of ObjectiveDatabase.
/// </summary>
public static class ObjectiveManager {

	const string defaultSavePath = "[]objectiveManager"; // Default save path if none specified.
	static ObjectiveDatabase objDB = new ObjectiveDatabase(); // Objective database that holds objective list.

	/// <summary>
	/// Gets the objective at the specified index of objective list.
	/// </summary>
	/// <returns>The objective if present in the list, null otherwise.</returns>
	/// <param name="index">Index.</param>
	public static objective GetObjective(int index) {
		if (index < 0 || index >= objDB.objList.Count) {
			Debug.LogError ("Invalid Objective Index");
			return null;
		}
		return objDB.objList [index];
	}

	/// <summary>
	/// Gets the objective with the specified id.
	/// </summary>
	/// <returns>The objective if present in the list, null otherwise.</returns>
	/// <param name="id">Objective id.</param>
	public static objective GetObjective(string id) {
		List<string> ids = objDB.objList.Select (obj => obj.id).ToList ();
		int index = ids.IndexOf (id);
		if (index < 0) {
			Debug.LogError ("Objective with id ' " + id + " ' does not exist in list");
			return null;
		}
		return GetObjective (index);
	}

	/// <summary>
	/// Gets the current objective list.
	/// </summary>
	/// <returns>The list.</returns>
	public static List<objective> GetList() {
		return objDB.objList;
	}

	/// <summary>
	/// Determines if the specified objective is present in the list.
	/// </summary>
	/// <returns><c>true</c> if is objective present the list, <c>false</c> otherwise.</returns>
	/// <param name="obj">Objective.</param>
	public static bool IsObjectivePresent(objective obj) {
		return objDB.objList.Contains (obj);
	}

	/// <summary>
	/// Determines if the objective with the specified id is present in the list.
	/// </summary>
	/// <returns><c>true</c> if is objective is present, <c>false</c> otherwise.</returns>
	/// <param name="id">Objective id.</param>
	public static bool IsObjectivePresent(string id) {
		return objDB.objList.Select(obj => obj.id).Contains (id);
	}

	public static void MakeChildrenConnections() {
		if (objDB.objList.Count <= 0)
			return;
		foreach (objective obj in objDB.objList) {
			AppendToParent (obj);
		}
	}

	public static void AppendToParent(objective obj) {
		if (obj.parentId == null || obj.parentId == "")
			return;
		objective parent = GetObjective (obj.parentId);
		if (parent != null) {
			if (parent.childIds == null)
				parent.childIds = new List<string> ();
			if (!parent.childIds.Contains(obj.id))
				parent.childIds.Add (obj.id);
		}
	}

	/// <summary>
	/// Adds the objective to the list.
	/// </summary>
	/// <returns><c>true</c>, if objective was added, <c>false</c> otherwise.</returns>
	/// <param name="obj">Objective to add.</param>
	/// <param name="avoidRepeats">If true, will not add objective if its already added.</param>
	public static bool AddObjective(objective obj, bool avoidRepeats = true) {
		if (avoidRepeats && IsObjectivePresent (obj)) {
			Debug.Log ("Objective already present");
			return false;
		}
		try {
			objDB.objList.Add(obj);
			AppendToParent(obj);
			return true;
		} catch (Exception e) {
			Debug.LogError (e.Message);
			return false;
		}
	}

	/// <summary>
	/// Removes the objective with specified id from the list.
	/// </summary>
	/// <returns><c>true</c>, if objective was removed, <c>false</c> otherwise.</returns>
	/// <param name="id">Objective id.</param>
	/// <param name="removeChildren">If true, also remove all child objectives.</param>
	public static bool RemoveObjective(string id, bool removeChildren = true) {
		if (IsObjectivePresent (id)) {
			objective toRemove = GetObjective (id);
			return RemoveObjective (toRemove, removeChildren);
		} else {
			Debug.Log ("Objective with ID: " + id + " Not Found");
			return false;
		}
	}

	/// <summary>
	/// Removes the objective from the list.
	/// By default also removes any child objectives.
	/// </summary>
	/// <returns><c>true</c>, if objective was removed, <c>false</c> otherwise.</returns>
	/// <param name="obj">Objective.</param>
	/// <param name="removeChildren">If true, also remove all child objectives.</param> 
	public static bool RemoveObjective(objective obj, bool removeChildren = true) {
		if (IsObjectivePresent (obj)) {
			int index = objDB.objList.IndexOf (obj);
			if (removeChildren) {
				List<string> childrenIds = obj.childIds;
				foreach (string id in childrenIds) {
					objective child = GetObjective (id);
					if (child != null)
						RemoveObjective (child);
				}
			}
			/* Not sure whether to add this part. Leave commented out for now.
			 * if (obj.parentId != null) {
				objective parentObj = GetObjective (obj.parentId);
				if (parentObj != null) {
					parentObj.childIds.Remove (obj.id);
				}
			}
			*/
			objDB.objList.RemoveAt (index);
			return true;
		} else {
			Debug.Log ("Objective " + obj.title + " Not Found");
			return false;
		}
	}

	/* Requires more information about how objectives will be handled to complete this function */
	public static bool UpdateObjective<T>(int id, T attr, T newValue) {
		return true;
	}

	/// <summary>
	/// Saves the current state of the objectives in an external xml file.
	/// </summary>
	/// <returns><c>true</c>, if objectives state was saved, <c>false</c> otherwise.</returns>
	/// <param name="filePath">File path to save to.</param>
	public static bool SaveObjectivesState(string filePath = defaultSavePath) {
		bool success = FileHandler.SaveFile (filePath, objDB);
		if (!success) {
			Debug.LogError ("Could Not Save");
			return false;
		}
		return true;
	}

	/// <summary>
	/// Loads the state of the objectives from an external xml file.
	/// Will replace current objectives state with new one.
	/// </summary>
	/// <returns><c>true</c>, if objectives state was loaded, <c>false</c> otherwise.</returns>
	/// <param name="filePath">File path to load from.</param>
	public static bool LoadObjectivesState(string filePath = defaultSavePath) {
		ObjectiveDatabase db = FileHandler.ReadFile<ObjectiveDatabase> (filePath);
		if (db == null) {
			Debug.LogError ("Could Not Load File");
			return false;
		}
		objDB = db;
		MakeChildrenConnections ();
		return true;
	}

	/// <summary>
	/// Adds the objectives from file.
	/// To be used for adding new objectives from a file while preserving the current objectives state.
	/// </summary>
	/// <returns><c>true</c>, if objectives from file was added, <c>false</c> otherwise.</returns>
	/// <param name="filePath">File path to load from.</param>
	public static bool AddObjectivesFromFile(string filePath) {
		ObjectiveDatabase db = FileHandler.ReadFile<ObjectiveDatabase> (filePath);
		if (db == null) {
			Debug.LogError ("Could Not Load File");
			return false;
		}
		return AppendObjectivesDatabase (db);
	}

	/// <summary>
	/// Appends new objectives database list to current list.
	/// </summary>
	/// <returns><c>true</c>, if objectives database was appended, <c>false</c> otherwise.</returns>
	/// <param name="db">Objective database.</param>
	/// <param name="avoidRepeats">If true, will not add objectives already in the list.</param>
	public static bool AppendObjectivesDatabase(ObjectiveDatabase db, bool avoidRepeats = true) {
		try {
			for (int i = 0; i < db.objList.Count; i++) {
				if (!avoidRepeats || !objDB.objList.Contains(db.objList [i]))
					objDB.objList.Add (db.objList [i]);
			}
			MakeChildrenConnections();
			return true;
		} catch (Exception e) {
			Debug.LogError (e.Message);
			return false;
		}
	}
}

/// <summary>
/// Objectives database that holds a list of objectives.
/// </summary>
[System.Serializable]
public class ObjectiveDatabase {
	public List<objective> objList;

	public ObjectiveDatabase() {
		objList = new List<objective>();
	}

	public ObjectiveDatabase(List<objective> list = null) {
		if (list == null) {
			objList = new List<objective> ();
		} else {
			objList = list;
		}
	}
}