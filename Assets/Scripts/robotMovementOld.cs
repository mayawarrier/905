using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class movement : MonoBehaviour
{
    public Animator anim;
    public Vector3 speed;
    private bool w, a, s, d;
    private bool up, down, left, right;
    private bool up_right, down_right, down_left, up_left;
    private bool isGrounded;
    public Vector3 fwdWrtPlayer;
    private Quaternion finalRotation;
    public Terrain terrain;
    public Vector3 playerOffset;
    public float groundOffset, terrainHeight;
    public Vector3 initPos, actualPos;
    private float playerObjX, playerObjY, playerObjZ;
    private float camX, camY, camZ;
    public float directionFacingIn;
    private float[] rotations;
    private bool[] rotationFlags;
    public Camera cam;
    private int i;

    void Start()
    {
        speed = new Vector3(1.5f, 1.5f, 1.5f);
        initPos = new Vector3(2.399143f, 0.7010555f, 1.573025f);
        numberOfBodyParts = 15;
        playerOffset = new Vector3(-17.6f, 9.9f, 11.5f);
        rotationFlags = new bool[8];
        rotations = new float[8];
        groundOffset = 9.213802f;
        for (i = 0; i < 8; i++) {
            rotations[i] = 45 * i;
        }
        transform.RotateAround(initPos, Vector3.up, 1);
        Cursor.visible = false;
    }

    void Update()
    {
        actualPos = getPlayerPosition();
        terrainHeight = terrain.SampleHeight(actualPos);
        directionFacingIn = transform.rotation.eulerAngles.y;

        w = Input.GetKey("w") || Input.GetKey("up");
        a = Input.GetKey("a") || Input.GetKey("left");
        s = Input.GetKey("s") || Input.GetKey("down");
        d = Input.GetKey("d") || Input.GetKey("right");

		up = rotationFlags[0] = w && !(a || s || d);
        up_right = rotationFlags[1] = w && d;
        right = rotationFlags[2] = d && !(w || a || s);
        down_right = rotationFlags[3] = s && d;
        down = rotationFlags[4] = s && !(w || a || d);
        down_left = rotationFlags[5] = s && a;
        left = rotationFlags[6] = a && !(w || s || d);
        up_left = rotationFlags[7] = w && a;

        if (up || down || left || right || up_right || down_right || down_left || up_left) {
            anim.SetBool("move", true);
        }
        else {
            anim.SetBool("move", false);
        }

        for (i = 0; i < rotationFlags.Length; i++)
        {
            if (rotationFlags[i] && directionFacingIn != rotations[i]) {
				transform.RotateAround(actualPos, Vector3.up, rotations[i] - directionFacingIn + cam.transform.rotation.eulerAngles.y);
                break;
            }
        }

        playerObjX = transform.position.x;
        playerObjY = transform.position.y;
        playerObjZ = transform.position.z;

        //Debug.Log(Vector3.Magnitude(getPlayerPosition() - cam.transform.position));
        //Debug.Log(actualPos);
    }

    private void FixedUpdate()
    { 
        if (anim.GetBool("move")) {
            transform.position += getSpeedVector(i) * Time.deltaTime;
            cam.transform.position += getSpeedVector(i) * Time.deltaTime;
        }
    }
    private void LateUpdate()
    {
        camX = cam.transform.position.x;
        camY = cam.transform.position.y;
        camZ = cam.transform.position.z;
        setTransform("y", terrainHeight - groundOffset);

        //cam.transform.Translate(new Vector3(0, terrain.SampleHeight(getPlayerPosition()) - terrainHeight, 0);

       // cam.transform.position = new Vector3(camX, actualPos.y + 1.5f, camZ); // this is another problem
    }
    
    private int numberOfBodyParts;
    private Vector3 sum, average;
    public Vector3 getPlayerPosition()
    {
        sum = Vector3.zero;
        foreach (Transform child in transform) {
            if (child.childCount == 0) {
                sum += child.position;
            }
            else {
                foreach (Transform grandChild in child.transform) {
                    sum += grandChild.position;
                }
            }
        }
        average = sum / numberOfBodyParts;
        return average;
    }

    public Vector3 getSpeedVector(int rotation)
    {
        fwdWrtPlayer = Vector3.Normalize(new Vector3(actualPos.x - camX, 0, actualPos.z - camZ));
        finalRotation = new Quaternion(0, 0, fwdWrtPlayer.z, fwdWrtPlayer.x);

        switch (rotation) {
            case 0:
                return speed.z * fwdWrtPlayer;
            case 1:
                finalRotation *= new Quaternion(0, 0, -1, 1); break;
            case 2:
                finalRotation *= new Quaternion(0, 0, -1, 0); break;
            case 3:
                finalRotation *= new Quaternion(0, 0, -1, -1); break;
            case 4:
                return -speed.z * fwdWrtPlayer;
            case 5:
                finalRotation *= new Quaternion(0, 0, 1, -1); break;
            case 6:
                finalRotation *= new Quaternion(0, 0, 1, 0); break;
            case 7:
                finalRotation *= new Quaternion(0, 0, 1, 1); break;
        }
        return speed.x * Vector3.Normalize(new Vector3(finalRotation.w, 0, finalRotation.z));
    }

    private Vector3 tempVector;

    public void setTransform(string axis, float position)
    {
        switch (axis) {
            case "x":
                tempVector = new Vector3(position, playerObjY , playerObjZ);
                transform.position = tempVector; break;
            case "y":
                tempVector = new Vector3(playerObjX, position, playerObjZ);
                transform.position = tempVector; break;
            case "z":
                tempVector = new Vector3(playerObjX, playerObjY, position);
                transform.position = tempVector; break;
        }
    }
}