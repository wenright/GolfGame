using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	private LookAtTarget lookAtTarget;
	private Rigidbody body;
	private Vector3 mouseStartPos;
	private float strength = 100.0f;

	public override void OnStartLocalPlayer ()
	{
		base.OnStartLocalPlayer();

		lookAtTarget = Camera.main.GetComponent<LookAtTarget> ();

		lookAtTarget.SetTarget(transform);
	}

	public override void OnStartServer ()
	{
		base.OnStartServer();

		body = GetComponent<Rigidbody>();
	}

	void Update ()
	{
		if (isLocalPlayer)
		{
			if (Input.GetMouseButtonDown(0))
			{
				mouseStartPos = Input.mousePosition;
			}

			if (Input.GetMouseButton(0))
			{
				// TODO hit direction grqphics
			}

			if (Input.GetMouseButtonUp(0))
			{
				Vector3 dir = (Quaternion.AngleAxis(90, Vector2.right) * (mouseStartPos - Input.mousePosition)).normalized;
				CmdSwing(dir);
			}
		}
	}

	[Command]
	private void CmdSwing (Vector3 dir)
	{
		body.AddForce(dir * strength);
	}
}
