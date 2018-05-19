using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDUIManager : MonoBehaviour {

	public Canvas HUDUICanvas;
	public Text codebox,subtitles;
	public Image healthWhite, healthRed, selectedObjectBox, selectedObject;
	public float subtime;

	private void Start()
	{

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
		if(subs.text != "")
			subtime+=Time.deltaTime;
		if (subtime>=10){
			subs.text="";
			subtime=0;
		}
	}
	void codeBoxAppend(string content)
	{
		codebox.text = codebox.text + "\n" + Time.time + "\t" + content;
	}
	void codeBoxUpdate(string content)
	{
		codebox.text = Time.time + "\t" + content;
	}
	void Addsub(string subtitle)
	{
		subs.text=subtitle;
		subtime=0;
	}
}