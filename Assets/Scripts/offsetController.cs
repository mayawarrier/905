using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class offsetController : MonoBehaviour {

	public float minDistance;
	public float maxDistance;
	public float smoothInitialValue;
	private float smooth;
	private Vector3 offsetDir;
	private float distance;
	private Vector3 endpoint;
	public GameObject player;

	void Start () {
		offsetDir = transform.localPosition.normalized;
		distance = transform.localPosition.magnitude;
		smoothInitialValue = 20.0f;
		minDistance = 1.0f;
	}

	void Update () {
		endpoint = transform.parent.TransformPoint (offsetDir * maxDistance);
		RaycastHit hit;

		if (Physics.Linecast (transform.parent.position, endpoint, out hit)) {
			distance = Mathf.Clamp (hit.distance * 0.9f, minDistance, maxDistance);
		} else {
			distance = maxDistance;
		}
		if (distance < 1.7) {
			player.SetActive (false);
		} else {
			player.SetActive(true);
		}

		smooth = smoothInitialValue * Time.deltaTime + Mathf.Abs(Input.GetAxis("Mouse X"));
		transform.localPosition = Vector3.Lerp (transform.localPosition, offsetDir * distance, smooth);
	}

	public void WalkDirectionSet() {
		offsetDir = new Vector3 (3.30f, 1.50f, -6.80f).normalized;
		maxDistance = 7.4f;
	}

	public void RunDirectionSet() {
		offsetDir = new Vector3 (0, 2.60f, -10f).normalized;
		maxDistance = 10;
	}
	}
