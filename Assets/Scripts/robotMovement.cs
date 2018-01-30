using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotMovement: MonoBehaviour
{
    public float forwardSpeed, turnSpeed;
    private Quaternion fwdWrtRobot;

    private void Start()
    {
        forwardSpeed = 5;
        turnSpeed = 6;
    }

    public void goInDirection(Vector3 dir)
    {
        //project onto the xz plane
        dir.y = 0;
        dir.Normalize();
        //smoothly rotate towards direction of travel
        fwdWrtRobot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, fwdWrtRobot, turnSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
    }
}