using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour {

	private Transform target;
	private Vector3 offset = new Vector3(0, 4, -5);
	private float lerpSpeed = 1.0f;

	void Update () {
		if (target == null) {
			return;
		}

		transform.LookAt(target);
		transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * lerpSpeed);
	}

	public void SetTarget (Transform target) {
		this.target = target;
	}
}
