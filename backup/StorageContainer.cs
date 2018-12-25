using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Storage container class that stores the data to be saved into a save file.
/// </summary>
[Serializable]
public class StorageContainer {

	/* Currently stores the following data:
	 * 	> Active Scene
	 * 	> Player Position
	 * 	> Player Rotation
	 * 	> Player Scale
	 * 	> Objective List
	 * 
	 * More will be added later.
	 */

	// All the data to store:
	private float[] PlayerPos;
	private float[] PlayerRot;
	private float[] PlayerScale;
	private string SceneName;
	private ObjectiveList objList;

	/// <summary>
	/// Initializes a new instance of the <see cref="StorageContainer"/> class.
	/// </summary>
	public StorageContainer() {
		// Player Info
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		Transform playerTrans = player.transform;
		Vector3 p_pos = playerTrans.position;
		Quaternion p_rot = playerTrans.rotation;
		Vector3 p_scale = playerTrans.localScale;

		// Level Info
		string sceneName = SceneManager.GetActiveScene().name;

		PlayerPos = new float[] { p_pos.x, p_pos.y, p_pos.z };
		PlayerRot = new float[] { p_rot.x, p_rot.y, p_rot.z, p_rot.w };
		PlayerScale = new float[] { p_scale.x, p_scale.y, p_scale.z };

		SceneName = sceneName;

		objList = ObjectiveManager.GetList ();
	}

    /*
     * Async does not work in this version of C#. Commented out the code below to test in-editor and build.
     * /

    /*
    /// <summary>
    /// Load data from this instance of StorageContainer.
    /// <returns><c>true</c> if load was successful, <c>false</c> otherwise.
    /// </summary>
    public bool Load() {
		try {
			LoadDataAsync();
			return true;
		} catch (Exception e) {
			Debug.LogError (e.Message);
			return false;
		}
	}

    
	/// <summary>
	/// Loads the data async.
	/// </summary>
	private async void LoadDataAsync() {
		AsyncOperation ao = SceneManager.LoadSceneAsync (SceneName, LoadSceneMode.Single);

		while (!ao.isDone)
			await Task.Delay (1);
		await Task.Delay (10);

		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		Transform playerTrans = player.transform;

		Vector3 pos = new Vector3 ( PlayerPos[0], PlayerPos[1], PlayerPos[2] );
		Quaternion rot = new Quaternion ( PlayerRot[0], PlayerRot[1], PlayerRot[2], PlayerRot[3] );
		Vector3 scale = new Vector3 ( PlayerScale[0], PlayerScale[1], PlayerScale[2] );

		playerTrans.position = pos;
		playerTrans.rotation = rot;
		playerTrans.localScale = scale;

		ObjectiveManager.ReplaceObjectivesList (objList);
	}

    */

}