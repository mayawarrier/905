using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement: MonoBehaviour {

    public GameObject player;
    public GameObject moveableObject;
    private Rigidbody pRigidBody;
    private Animator anim;


    private bool isWalking;
    private bool isPushing;
    private float h;
    private float v;
    public float speed;
    public float rotSpeed;

    //HASH

    int walkHash = Animator.StringToHash("isWalking");
    


    private void Start()
    {
        pRigidBody = player.GetComponent<Rigidbody>();
        isWalking = false;
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {

        isPushing = moveableObject.GetComponent<ShelfTrigger>().isPushing;
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if (v != 0)
        {
            isWalking = true;
            anim.SetBool(walkHash, isWalking);

        }
        else
        {
            isWalking = false;
            anim.SetBool(walkHash, isWalking);
        }

        if (player.transform.position.x > moveableObject.transform.position.x && isPushing == true)
        {
            player.transform.position = player.transform.position + (transform.forward * speed * v);
            player.transform.rotation = Quaternion.AngleAxis(-90, Vector3.up);
        }

        else if (player.transform.position.x < moveableObject.transform.position.x && isPushing == true)
        {
            player.transform.position = player.transform.position + (transform.forward * speed * v);
            player.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
        }

        else
        {
            player.transform.position = player.transform.position + (transform.forward * speed * v);
            player.transform.Rotate(Vector3.up * rotSpeed * h);
        }
    }


}
