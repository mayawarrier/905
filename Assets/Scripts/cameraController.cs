// this script should be assinged to an empty object whose child is the main camera
// the camera must be manually set offset from this empty object and looking towards this empty object

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraController: MonoBehaviour {
    
    private float totalHorizontalMovement, totalVerticalMovement;
    private Vector3 initialPosition;
    private float glideSpeed;
    public Slider mouseSensitivity;
    public Toggle mouseAxis;
    private int axisDirection;
    private bool cameraPaused;
    private Vector3 previousFinalPosition;
	public Transform follow; // gameObject to follow

    public void Start()
    {
        cameraPaused = false;
        mouseSensitivity.value = 10;
        mouseAxis.isOn = true;
        glideSpeed = 5;
        transform.GetChild(0).localPosition = new Vector3(3.29f, 3.96f, -6.78f);
    }

    public void Pause()
    {
        cameraPaused = true;
    }

    public void Restart() {
        cameraPaused = false;
    }

    void Update () {
        //Debug.Log("glideComplete: " + glideComplete.ToString());
        if (!cameraPaused) {
            if (mouseAxis.isOn)
            {
                axisDirection = -1;
            }
            else {
                axisDirection = 1;
            }
            totalHorizontalMovement += Input.GetAxis("Mouse X") * mouseSensitivity.value;
            totalVerticalMovement += axisDirection * Input.GetAxis("Mouse Y") * mouseSensitivity.value;
            // clamp vertical rotation so that camera does not go upside down
            totalVerticalMovement = Mathf.Clamp(totalVerticalMovement, -50, 50);
            transform.position = follow.position; // make sure the camera always follows the gameObject
        }
	}

	void FixedUpdate() {
        // update the angle with the new angle based on the mouse rotation
        transform.eulerAngles = new Vector3(totalVerticalMovement, totalHorizontalMovement, 0);
		// we don't want horizontal rotation to overflow if we keep rotating horizontally
		// instead update its value as the object y-rotation value as the y-rotation value is always between -180 and 180
		// the y-rotation value will go descending from -180 if we exceed 180
		totalHorizontalMovement = transform.eulerAngles.y;
	}

    public void GlideToPosition(Vector3 finalPosition) {
        initialPosition = transform.GetChild(0).localPosition;

        if (initialPosition != finalPosition)
        {
            transform.GetChild(0).localPosition = Vector3.Slerp(initialPosition, finalPosition, glideSpeed * Time.deltaTime);
        }
    }

    private Vector3 AccurateSlerp(Vector3 initialPosition, Vector3 finalPosition, float increment) {


        return Vector3.zero;
    }
}