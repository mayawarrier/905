// controls all standard animations of a robot

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationController : MonoBehaviour {

	public Animator anim;
	public playerController robot;

	void Update () {
        //if (robot.updateMovementStatus)
		anim.SetBool("isWalking", robot.isWalking);
        anim.SetBool("isRunning", robot.isRunning);
	}
}
