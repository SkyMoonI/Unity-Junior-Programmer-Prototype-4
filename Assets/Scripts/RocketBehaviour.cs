using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
	public Transform target;
	float speed = 15.0f;
	bool homing;

	float rocketStrength = 15.0f;
	float aliveTimer = 5.0f;

	// Update is called once per frame
	void Update()
	{
		// if homing is true, move towards target
		if (homing && target != null)
		{
			Vector3 moveDirection = (target.transform.position - transform.position).normalized;
			transform.position += moveDirection * speed * Time.deltaTime;
			transform.LookAt(target);
		}
	}

	public void Fire(Transform newTarget)
	{
		target = newTarget;
		homing = true;
		Destroy(gameObject, aliveTimer);
	}

	void OnCollisionEnter(Collision other)
	{
		// if rocket is colliding with target, push the target back
		if (target != null && other.gameObject.tag == target.tag)
		{
			Rigidbody targetRB = other.gameObject.GetComponent<Rigidbody>();
			Vector3 away = -other.contacts[0].normal;
			targetRB.AddForce(away * rocketStrength, ForceMode.Impulse);
			Destroy(gameObject);
		}
	}
}
