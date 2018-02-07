// this script controls the player character. It acts as a motor, calling movement functions for 905.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

	public offsetController offsetController;
    private bool w, a, s, d, spacePressed, ctrlPressed;
    private bool[] rotationFlags;
    public robotMovement player;
	public Transform cameraTransform; 
	Vector3 direction; // direction camera is looking at
    private Quaternion finalRotation;
    public Transform lookAt;
    public bool isWalking, isRunning, isCrouching;
    private bool complete;

    private void Start()
    {
        rotationFlags = new bool[8];
    }

    void Update () {

        isWalking = false;
        isRunning = false;
        isCrouching = false;

        w = Input.GetKey("w") || Input.GetKey("up");
        a = Input.GetKey("a") || Input.GetKey("left");
        s = Input.GetKey("s") || Input.GetKey("down");
        d = Input.GetKey("d") || Input.GetKey("right");
        spacePressed = Input.GetKey("space");
        ctrlPressed = Input.GetKey(KeyCode.LeftControl);

        rotationFlags[0] = w && !(a || s || d);
        rotationFlags[1] = w && d;
        rotationFlags[2] = d && !(w || a || s);
        rotationFlags[3] = s && d;
        rotationFlags[4] = s && !(w || a || d);
        rotationFlags[5] = s && a;
        rotationFlags[6] = a && !(w || s || d);
        rotationFlags[7] = w && a;

        direction = lookAt.position - cameraTransform.position;

        if ((w || a || s || d) && !(spacePressed || ctrlPressed))
        {
            isWalking = true;
            player.forwardSpeed = 5;
        }
        else if ((w || a || s || d) && spacePressed && !ctrlPressed) {
            isRunning = true;
            player.forwardSpeed = 8;
			offsetController.RunDirectionSet();
		}

        for (int i = 0; i < rotationFlags.Length; i++)
		{
            if (rotationFlags[i])
            {
                player.goInDirection(rotateDirectionVector(direction, i));
                break;
			}
		}

        if (!spacePressed) {
			offsetController.WalkDirectionSet();
		}
	}

    private Vector3 rotateDirectionVector(Vector3 fwdWrtPlayer, int rotation) {

        finalRotation = new Quaternion(0, 0, fwdWrtPlayer.z, fwdWrtPlayer.x); // initial rotation as a quaternion

        switch (rotation)
        {
            case 0:
                return fwdWrtPlayer;
            case 1:
                finalRotation *= new Quaternion(0, 0, -1, 1); break;
            case 2:
                finalRotation *= new Quaternion(0, 0, -1, 0); break;
            case 3:
                finalRotation *= new Quaternion(0, 0, -1, -1); break;
            case 4:
                return -fwdWrtPlayer;
            case 5:
                finalRotation *= new Quaternion(0, 0, 1, -1); break;
            case 6:
                finalRotation *= new Quaternion(0, 0, 1, 0); break;
            case 7:
                finalRotation *= new Quaternion(0, 0, 1, 1); break;
            default:
                Debug.Log("Not a valid rotation. Provide an integer from 0-7."); break;
        }
        return Vector3.Normalize(new Vector3(finalRotation.w, 0, finalRotation.z));
    }
}