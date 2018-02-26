using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robotMovement: MonoBehaviour
{
    public float forwardSpeed, turnSpeed;
    private Quaternion fwdWrtRobot;
    private Quaternion finalRotation;

    private void Start()
    {
        forwardSpeed = 5;
        turnSpeed = 6;
    }

    public void goInDirection(Vector3 direction, int rotation)
    {
        direction = rotateDirectionVector(direction, rotation);
        fwdWrtRobot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, fwdWrtRobot, turnSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
    }

    private Vector3 rotateDirectionVector(Vector3 fwdWrtRobot, int rotation)
    {

        finalRotation = new Quaternion(0, 0, fwdWrtRobot.z, fwdWrtRobot.x); // initial rotation as a quaternion

        switch (rotation)
        {
            case 0:
                return new Vector3(fwdWrtRobot.x, 0, fwdWrtRobot.z);
            case 1:
                finalRotation *= new Quaternion(0, 0, -1, 1); break;
            case 2:
                finalRotation *= new Quaternion(0, 0, -1, 0); break;
            case 3:
                finalRotation *= new Quaternion(0, 0, -1, -1); break;
            case 4:
                return new Vector3(-fwdWrtRobot.x, 0, -fwdWrtRobot.z);
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