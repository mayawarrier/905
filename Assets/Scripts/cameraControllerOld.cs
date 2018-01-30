using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControllerOld: MonoBehaviour {

    public Vector3 offsetFromPlayer;
    public movement movement;
    private Vector3 deltaTheta;
    public Vector3 playerPos;

	void Start () {
        deltaTheta = Vector3.zero;
        offsetFromPlayer = new Vector3(0, 1.5f, -2);
        transform.rotation = Quaternion.Euler(new Vector3(20, 0, 0));
        transform.position = movement.initPos + offsetFromPlayer;
        transform.RotateAround(movement.initPos, Vector3.up, 1);
    }
	
	void Update () {
        playerPos = movement.getPlayerPosition();
        transform.position = ClampMagnitude(transform.position - playerPos, 2.5f, 2.5f) + playerPos;
        transform.RotateAround(playerPos, Vector3.up, deltaTheta.x);
        transform.RotateAround(playerPos, movement.getSpeedVector(2), -deltaTheta.y);
        transform.RotateAround(playerPos, movement.fwdWrtPlayer, -transform.rotation.eulerAngles.z);
    }

    private void FixedUpdate()
    {
        deltaTheta.x = 1.5f * Input.GetAxis("Mouse X");
        deltaTheta.y = Input.GetAxis("Mouse Y");
    }

    private float sqrMagnitude;
    private Vector3 ClampMagnitude(Vector3 vector, float min, float max) {
        sqrMagnitude = Vector3.SqrMagnitude(vector);
        if (sqrMagnitude > max * max) {
            return vector.normalized * max;
        }
        else if (sqrMagnitude < min * min) {
            return vector.normalized * min;
        }
        else return vector;
    }

}
