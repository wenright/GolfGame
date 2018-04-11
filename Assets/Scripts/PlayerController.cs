using System.Collections;
﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour {

	public GameObject directionArrow;
	public GameObject confettiPrefab;

	private Text numSwingsText;
	private LineRenderer lineRenderer;
	private LookAtTarget lookAtTarget;
	private Rigidbody body;
	private float maxDrawLength = 150.0f;
	private float strength = 0.03f;

	[SyncVar(hook="UpdateNumSwings")]
	private int numSwings = 0;

	private void Start () {
		body = GetComponent<Rigidbody>();
		lineRenderer = transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<LineRenderer>();
	}

	public override void OnStartLocalPlayer () {
		base.OnStartLocalPlayer();

		lookAtTarget = Camera.main.GetComponent<LookAtTarget>();
		lookAtTarget.SetTarget(transform);

		numSwingsText = GameObject.FindGameObjectWithTag("NumSwingsText").GetComponent<Text>();
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

				directionArrow.transform.GetChild(0).transform.localPosition = new Vector3(0, 0, -1 + -GetVector().magnitude);
				lineRenderer.SetPosition(0, Vector3.zero);
				lineRenderer.SetPosition(1, new Vector3(0, GetVector().magnitude, 0));
			}

			if (Input.GetMouseButtonUp(0)) {
				Swing(GetVector());
			}
		}
	}

	private Vector3 GetDir () {
		return GetVector().normalized;
	}

	private Vector3 GetVector () {
		Vector3 vec = (Quaternion.AngleAxis(90, Vector2.right)
			* (Camera.main.WorldToScreenPoint(transform.position) - Input.mousePosition));

		vec = Vector3.ClampMagnitude(vec, maxDrawLength) * strength;

		return vec;
	}

	// Starts the physics simulation on client side while sending request to server
	private void Swing (Vector3 dir) {
		CmdSwing(dir);
		directionArrow.SetActive(false);
	}

	[Command]
	private void CmdSwing (Vector3 dir) {
		// TODO need vector clamping on server side. Otherwise player could just send a crazy high velocity
		// Could just move client side clamping to server, but then need to fix UI with clamping
		// Clamp a local variable on client so it is not double clamped?
		body.velocity = dir;
		RpcSetVelocity(body.velocity);
		numSwings++;
	}

	[ClientRpc]
	private void RpcSetVelocity (Vector3 velocity) {
		body.velocity = velocity;
	}

	private void OnTriggerEnter (Collider other) {
		if (!isServer) {
			return;
		}

		if (other.tag == "Hole") {
			GameObject confettiInstance = Instantiate(confettiPrefab, other.transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
			NetworkServer.Spawn(confettiInstance);
			print("You did it in " + numSwings + " swings!");
		}
	}

	private void UpdateNumSwings (int numSwings) {
		numSwingsText.text = "" + numSwings;
	}

}
