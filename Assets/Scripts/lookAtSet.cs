using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtSet : MonoBehaviour {

    public playerController player;
    public Transform mainCamera;

	void Update () {
        if (player.isWalking)
        {
            // projects the main camera vector onto the x axis
            transform.localPosition = new Vector3(mainCamera.transform.localPosition.x, 0, 0);
        }
        else if (player.isRunning) {
            transform.localPosition = Vector3.zero;
        }
    }
}