using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	GameObject[] enemies;
	Rigidbody projetileRB;
	float speed = 10.0f;
	float pushStrength = 30.0f;

	// Start is called before the first frame update
	void Start()
	{
		transform.position = FindObjectOfType<PlayerController>().transform.position + Vector3.forward;
	}

	// Update is called once per frame
	void Update()
	{
		GameObject target = GameObject.FindWithTag("Enemy");

		transform.LookAt(target.transform.position);
		transform.position += transform.forward * speed * Time.deltaTime;

	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			Rigidbody enemyRB = other.gameObject.GetComponent<Rigidbody>();
			Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;
			enemyRB.AddForce(awayFromPlayer * pushStrength, ForceMode.Impulse);
			Destroy(gameObject);
		}
	}
}
