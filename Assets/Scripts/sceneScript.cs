using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sceneScript : MonoBehaviour {

    public Canvas gameOptionsUICanvas;
    public cameraController cam;
    public Slider sensitivitySlider;
    public Toggle mouseAxis;
    public Toggle fpsShowToggle;
    public Text fpsDisplayer;

    private void Start()
    {
        Cursor.visible = false;
        gameOptionsUICanvas.enabled = false;
    }

    void Update () {
        if (Input.GetKeyDown("escape")) {
            if (gameOptionsUICanvas.enabled == false) {
                gameOptionsUICanvas.enabled = true;
                sensitivitySlider.interactable = true;
                mouseAxis.interactable = true;
                fpsShowToggle.interactable = true;
                Cursor.visible = true;
                cam.Pause();
                Time.timeScale = 0;
            }
            else {
                gameOptionsUICanvas.enabled = false;
                sensitivitySlider.interactable = false; // makes sure these can't be changed after escaping the UI
                mouseAxis.interactable = false;
                fpsShowToggle.interactable = false;
                Cursor.visible = false;
                cam.Restart();
                Time.timeScale = 1;
            }
        }
        if (fpsShowToggle.isOn == true && Time.timeScale != 0)
        {
            fpsDisplayer.text = "FPS\n" + (Mathf.Round(1 / Time.smoothDeltaTime)).ToString();
        }
	}

    public void ToggleFPSDisplayer() {
        fpsDisplayer.enabled = fpsShowToggle.isOn;
    }
}
