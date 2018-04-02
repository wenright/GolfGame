using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour {

	private Transform target;

	void Update ()
	{
		if (target == null)
		{
			return;
		}

		transform.LookAt(target);
	}

	public void SetTarget (Transform target)
	{
		this.target = target;
	}
}
