using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextOn : MonoBehaviour {

    public GameObject gameObject;
    
    private Text text;

	// Use this for initialization
	void Awake () {
        text = GetComponent<Text>();
        text.enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () { 
        if (gameObject.GetComponent<ShelfTrigger>().textOn)
        {
            text.enabled = true;
            if (Input.GetButton("Jump"))
            {
                text.color = Color.red;
            }
            else
            {
                text.color = Color.black;
            }
            
        }
        else
        {
            text.enabled = false;
        }
	}
}
