using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
[NetworkSettings(channel = 0, sendInterval =  0.5f)]
public class SyncRigidbody : NetworkBehaviour {

	[SyncVar(hook="UpdatePosition")]
	private Vector3 position;

	[SyncVar(hook="UpdateVelocity")]
	private Vector3 velocity;

	private Rigidbody body;

	private void Start () {
		body = GetComponent<Rigidbody>();
	}

	private void FixedUpdate () {
		if (isLocalPlayer) {
			return;
		}

		position = transform.position;
		velocity = body.velocity;
	}

	private void UpdatePosition (Vector3 newPosition) {
		transform.position = newPosition;
	}

	private void UpdateVelocity (Vector3 newVelocity) {
		body.velocity = newVelocity;
	}

}
