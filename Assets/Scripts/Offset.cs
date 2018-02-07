using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offset : MonoBehaviour {

	public float minDistance = 1.0f;
	public float maxDistance;
	public float smooth = 20.0f;
	Vector3 dir;
	float distance;
	Vector3 endpoint;
	public GameObject player;
	private Renderer playerRenderer;


	// Use this for initialization
	void Start () {
		dir = transform.localPosition.normalized;
		distance = transform.localPosition.magnitude;
		playerRenderer = player.GetComponent<Renderer> ();
	}

	// Update is called once per frame
	void Update () {
		endpoint = transform.parent.TransformPoint (dir * maxDistance);
		RaycastHit hit;

		if (Physics.Linecast (transform.parent.position, endpoint, out hit)) {
			distance = Mathf.Clamp (hit.distance, minDistance, maxDistance - 3.0f);
			distance = distance - 0.5f;
		} else {
			distance = maxDistance;
		}
		if (distance < 1.7) {
			player.SetActive (false);
		} else {
			player.SetActive(true);
		}
		transform.localPosition = Vector3.Lerp (transform.localPosition, dir * distance, smooth * Time.deltaTime)
	}

	public void WalkDirectionSet() {
		dir = new Vector3 (3.30f, 1.50f, -6.80f).normalized;
		maxDistance = 7.4f;
	}

	public void RunDirectionSet() {
		dir = new Vector3 (1.0f, 2.54f, -10f).normalized;
		maxDistance = 10;
	}
	}
