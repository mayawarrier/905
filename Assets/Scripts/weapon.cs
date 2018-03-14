//Attach this to the weapon object
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour {

	public int clipCapacity, loadedBullets, extraBullets, bulletsShotAtOnce;
	public GameObject bullet;
	public Transform bulletSpawn;
	public float fireRate, reloadTime;
	public string tag;

	private AudioSource audioSource, nullAudio;

	void Start () 
	{
		audioSource = GetComponent<AudioSource> ();
		nullAudio = GetComponent<AudioSource> ();
	}

	public void Fire ()
	{
		if (loadedBullets > 0) {
			for (int i = 0; i < bulletsShotAtOnce; i++) {
				Instantiate (bullet, bulletSpawn.position, bulletSpawn.rotation);
			}
			loadedBullets -= bulletsShotAtOnce;
			audioSource.Play ();
		} else if (tag == "melee") {
			 //add code to whack the enemy with melee weapon
		} else {
			nullAudio.Play ();
		}
	}
	public void Reload ()
	{
		if (loadedBullets != clipCapacity) {
			yield return new WaitForSeconds (reloadTime);
			if (extraBullets >= clipCapacity - loadedBullets) {
				extraBullets -= clipCapacity - loadedBullets;
				loadedBullets = clipCapacity;
			} else {
				loadedBullets += extraBullets;
				extraBullets = 0;
			}
		}
	}
}
