using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneScript : MonoBehaviour {

    public Canvas gameOptionsUICanvas;
    public cameraController cam;

    private void Start()
    {
        Cursor.visible = false;
        gameOptionsUICanvas.enabled = false;
    }

    void Update () {
        if (Input.GetKeyDown("escape")) {
            if (gameOptionsUICanvas.enabled == false) {
                gameOptionsUICanvas.enabled = true;
                Cursor.visible = true;
                cam.Pause();
                Time.timeScale = 0;
            }
            else {
                gameOptionsUICanvas.enabled = false;
                Cursor.visible = false;
                cam.Restart();
                Time.timeScale = 1;
            }
        }
	}
}
