using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	public GameObject directionArrow;

	private LookAtTarget lookAtTarget;
	private Rigidbody body;
	private float strength = 500.0f;

	public override void OnStartLocalPlayer () {
		base.OnStartLocalPlayer();

		lookAtTarget = Camera.main.GetComponent<LookAtTarget>();
		lookAtTarget.SetTarget(transform);
	}

	public override void OnStartServer () {
		base.OnStartServer();

		body = GetComponent<Rigidbody>();
	}

	void Update () {
		if (isLocalPlayer) {
			if (Input.GetMouseButtonDown(0)) {
				directionArrow.SetActive(true);
			}

			if (Input.GetMouseButton(0)) {
				// TODO hit direction grqphics
				directionArrow.transform.rotation =
					Quaternion.Euler(0, Quaternion.LookRotation(Quaternion.AngleAxis(180, Vector2.up) * GetDir()).eulerAngles.y, 0);
			}

			if (Input.GetMouseButtonUp(0)) {
				CmdSwing(GetDir());
				directionArrow.SetActive(false);
			}
		}
	}

	private Vector3 GetDir () {
		return (Quaternion.AngleAxis(90, Vector2.right)
			* (Camera.main.WorldToScreenPoint(transform.position) - Input.mousePosition)).normalized;
	}

	[Command]
	private void CmdSwing (Vector3 dir) {
		body.AddForce(dir * strength);
	}
}
