using System.Collections;
﻿using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	public GameObject directionArrow;

	private LookAtTarget lookAtTarget;
	private Rigidbody body;
	private float strength = 10.0f;

	private void Start () {
		body = GetComponent<Rigidbody>();
	}

	public override void OnStartLocalPlayer () {
		base.OnStartLocalPlayer();

		lookAtTarget = Camera.main.GetComponent<LookAtTarget>();
		lookAtTarget.SetTarget(transform);
	}

	public override void OnStartServer () {
		base.OnStartServer();
	}

	void Update () {
		if (isLocalPlayer) {
			if (Input.GetMouseButtonDown(0)) {
				directionArrow.SetActive(true);
			}

			if (Input.GetMouseButton(0)) {
				directionArrow.transform.rotation =
					Quaternion.Euler(0, Quaternion.LookRotation(Quaternion.AngleAxis(180, Vector2.up) * GetDir()).eulerAngles.y, 0);
			}

			if (Input.GetMouseButtonUp(0)) {
				Swing(GetDir());
			}
		}
	}

	private Vector3 GetDir () {
		return (Quaternion.AngleAxis(90, Vector2.right)
			* (Camera.main.WorldToScreenPoint(transform.position) - Input.mousePosition)).normalized;
	}

	// Starts the physics simulation on client side while sending request to server
	private void Swing (Vector3 dir) {
		CmdSwing(dir);
		directionArrow.SetActive(false);
	}

	[Command]
	private void CmdSwing (Vector3 dir) {
		body.velocity = dir * strength;
		RpcSetVelocity(body.velocity);
	}

	[ClientRpc]
	private void RpcSetVelocity (Vector3 velocity) {
		print("setting new velocity");
		print(velocity);
		body.velocity = velocity;
	}
}
