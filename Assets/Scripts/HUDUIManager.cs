using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDUIManager : MonoBehaviour {

	public Canvas HUDUICanvas;
	public Text codebox,subtitles;
	public Image selectedObjectBox, selectedObject;
	private float subtime,health,armour,maxHealth,MaxArmour,distance;
	public Slider healthSlider, armourSlider;
	public GameObject arrow;
	private Vector3 rotation,objectiveLocation;

	private void Start()
	{
		healthSlider.interactable = false;
		armourSlider.interactable = false;
		health = 0;
		armour = 0;
		healthSlider.value = 0;
		armourSlider.value = 0;
		subtitles.text = "";
	}
	void Update()
	{
		if (Input.GetKeyDown ("i")) {
			//Launch Inventory
		}
		for(int i=0;i<10;i++)
		if (Input.GetKeyDown ("" + i)) {
				//Select Item i
		}
		if(subtitles.text != "")
			subtime+=Time.deltaTime;
		if (subtime>=10){
			subtitles.text="";
			subtime=0;
		}
		rotateArrow ();
	}
	private void rotateArrow()
	{
		rotation = new Vector3 (objectiveLocation.x - arrow.transform.position.x, objectiveLocation.y - arrow.transform.position.y, objectiveLocation.z - arrow.transform.position.z);
		distance = (float) Mathf.Sqrt (Mathf.Pow (rotation.x, 2) + Mathf.Pow (rotation.y, 2) + Mathf.Pow (rotation.z, 2));
		arrow.transform.rotation = Quaternion.Euler (rotation.x/distance*Mathf.Rad2Deg, rotation.y/distance*Mathf.Rad2Deg, rotation.z/distance*Mathf.Rad2Deg);
	}
	void Addsub(string subtitle)
	{
		subtitles.text=subtitle;
		subtime=0;
	}
	public void displayOnTerminal(string toBeDisplayed, string hexCodeColor, int fontSize, char option)
	{
		if(option=='a')
			codebox.text += "\n" + Time.time + "\t" + toBeDisplayed;
		else if(option=='r')
			codebox.text = Time.time + "\t" + toBeDisplayed;
	}
	public void showOnCompass(Vector3 locationOfObjective)
	{
		objectiveLocation = locationOfObjective;
		rotateArrow ();
	}
	public void changeHealthBy(float change)
	{
		if (maxHealth >= health + change)
			health += change;
		else if (maxHealth <= health)
			health = maxHealth;
		healthSlider.value = health;
	}
	public void changeArmourBy(float change)
	{
		if (MaxArmour >= armour + change && armour > -change) {
			armour += change;
			armourSlider.value = armour;
		} else if (MaxArmour <= armour && armour > -change) {
			armour = MaxArmour;
			armourSlider.value = armour;
		} else {
			armour = 0;
			armourSlider.value = armour;
			armourSlider.transform.localScale = new Vector2 (0, 0);
		}
	}
	public void changeMaxHealthBy(float change)
	{
		maxHealth += change;
		healthSlider.maxValue = maxHealth;
	}
	public void changeMaxArmourBy(float change)
	{
		MaxArmour += change;
		armourSlider.maxValue = MaxArmour;
		armourSlider.transform.localScale = new Vector2 (1, 1);
	}

}