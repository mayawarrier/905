using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeaponController : MonoBehaviour {

	weapon current, primary, secondary, melee, blank;
	void Start()
	{
		//load weapons from save file
	}
	void Update()
	{
		if (Input.GetButton ("1")) {
			current = primary;
		}
		if (Input.GetButton ("2")) {
			current = secondary;
		}
		if (Input.GetButton ("3")) {
			current = melee;
		}
		if (Input.GetButton ("Fire")) {
			current.Fire ();
		}
		if (Input.GetButton ("G")) {
			//gun tossing animation
			current = blank;
		}
		if (Input.GetButton ("R")) {
			current.Reload ();
		}
	}
	void Pickup(weapon pickup)
	{
		if (pickup.tag == "primary")
			primary = pickup;
		else if (pickup.tag == "secondary")
			secondary = pickup;
		else if (pickup.tag == "melee")
			melee = pickup;
	}

}
