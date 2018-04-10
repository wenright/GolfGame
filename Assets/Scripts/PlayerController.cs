using System.Collections;
﻿using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	public GameObject directionArrow;

	private LookAtTarget lookAtTarget;
	private Rigidbody body;
	private float strength = 500.0f;

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
	// TODO Should we delay this by avg ping time to reduce jump in next SyncVar update?
	private void Swing (Vector3 dir) {
		// TODO add force locally to simulate what is happening on the server?
		// AddForce(dir);
		directionArrow.SetActive(false);
		CmdSwing(dir);
	}

	[Command]
	private void CmdSwing (Vector3 dir) {
		AddForce(dir);
	}

	private void AddForce (Vector3 dir) {
		body.AddForce(dir * strength);
	}
}
