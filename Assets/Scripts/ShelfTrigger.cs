using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShelfTrigger : MonoBehaviour {

    public bool textOn = false;
    public bool isPushing = false;
    public GameObject player;

    private Animator anim;
    private Rigidbody shelfRig;

    //HASHES

    int pushTrans = Animator.StringToHash("pushingStart");
    int pushHash = Animator.StringToHash("isPushing");

	// Use this for initialization
	void Awake () {
        anim = player.GetComponent<Animator>();
        shelfRig = GetComponentInParent<Rigidbody>();
        shelfRig.isKinematic = false;
        textOn = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Input.GetButtonDown("Jump")) 
        {
            anim.SetTrigger(pushTrans);
        }
        else if (Input.GetButton("Jump"))
        {
            shelfRig.isKinematic = false;
            isPushing = true;
            anim.SetBool(pushHash, isPushing);
        }
        else
        {
            shelfRig.isKinematic = true;
            isPushing = false;
            anim.SetBool(pushHash, isPushing);
        }
        textOn = true;
        //Debug.Log("We in");
        
    }

    private void OnTriggerStay(Collider other)
    {

        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger(pushTrans);
        }
        else if (Input.GetButton("Jump"))
        {
            shelfRig.isKinematic = false;
            isPushing = true;
            anim.SetBool(pushHash, isPushing);
        }
        else
        {
            shelfRig.isKinematic = true;
            isPushing = false;
            anim.SetBool(pushHash, isPushing);
        }
        
        //Debug.Log("We still in");
    }

    private void OnTriggerExit(Collider other)
    {
        shelfRig.isKinematic = false;
        textOn = false;
        isPushing = false;
        anim.SetBool(pushHash, isPushing);
        //Debug.Log("We Out");
    }
}
