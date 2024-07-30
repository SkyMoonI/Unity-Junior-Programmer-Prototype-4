using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] float speed = 5f;
	Rigidbody playerRB;
	GameObject focalPoint;

	// [Header("Knock back Powerup")]
	// [SerializeField] bool hasKnockBackPowerup = false;
	// [SerializeField] float KnockBackPowerupStrength = 15f;
	// [SerializeField] GameObject KnockBackPowerupIndicator;
	// float KnockBackPowerupIndicatorOffset = -0.5f;

	// [Header("Projectile Powerup")]
	// [SerializeField] GameObject ProjectilePrefab;
	// [SerializeField] GameObject ProjectilePowerupIndicator;

	[Header("New Powerup Setup")]
	[SerializeField] PowerUpType currentPowerUp = PowerUpType.None;
	public GameObject rocketPrefab;
	GameObject tmpRocket;
	Coroutine powerupCountdown;
	bool hasPowerup;
	[SerializeField] GameObject powerupIndicator;
	float powerupIndicatorOffset = -0.5f;
	float pushBackPowerupStrength = 35f;

	[Header("Smash Powerup")]
	[SerializeField] float hangTime = 0.5f;
	[SerializeField] float smashSpeed = 8f;
	[SerializeField] float explosionForce = 10f;
	[SerializeField] float explosionRadius = 10f;
	bool smashing;
	float floorY;
	// Start is called before the first frame update
	void Start()
	{
		playerRB = GetComponent<Rigidbody>();
		focalPoint = GameObject.Find("Focal Point");
	}

	// Update is called once per frame
	void Update()
	{
		float verticalInput = Input.GetAxis("Vertical");
		playerRB.AddForce(focalPoint.transform.forward * verticalInput * speed);
		powerupIndicator.transform.position = transform.position + new Vector3(0, powerupIndicatorOffset, 0);
		// KnockBackPowerupIndicator.transform.position = transform.position + new Vector3(0, KnockBackPowerupIndicatorOffset, 0);

		if (currentPowerUp == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F))
		{
			LaunchRockets();
		}
		if (currentPowerUp == PowerUpType.Smash && Input.GetKeyDown(KeyCode.Space) && !smashing)
		{
			smashing = true;
			StartCoroutine(Smash());
		}
	}
	// IEnumerator StartKnockBackPowerupCountdown(int seconds)
	// {
	// 	yield return new WaitForSeconds(seconds);


	// 	hasKnockBackPowerup = false;
	// 	KnockBackPowerupIndicator.SetActive(false);
	// }


	void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.CompareTag("Powerup"))
		{
			hasPowerup = true;
			currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType;
			powerupIndicator.gameObject.SetActive(true);
			Destroy(other.gameObject);

			if (powerupCountdown != null)
			{
				StopCoroutine(powerupCountdown);
			}
			powerupCountdown = StartCoroutine(PowerupCountdownRoutine());
		}
		// if (other.gameObject.CompareTag("KnockBackPowerup"))
		// {
		// 	hasKnockBackPowerup = true;
		// 	Destroy(other.gameObject);
		// 	StartCoroutine(StartKnockBackPowerupCountdown(7));
		// 	KnockBackPowerupIndicator.SetActive(true);
		// }
		// if (other.gameObject.CompareTag("ProjectilePowerup"))
		// {
		// 	Destroy(other.gameObject);
		// 	SpawnProjectiles(other);
		// }
	}
	IEnumerator PowerupCountdownRoutine()
	{
		yield return new WaitForSeconds(7);
		hasPowerup = false;
		powerupIndicator.gameObject.SetActive(false);
		currentPowerUp = PowerUpType.None;
	}
	// void SpawnProjectiles(Collider other)
	// {
	// 	int enemiesLength = GameObject.FindGameObjectsWithTag("Enemy").Length;
	// 	for (int i = 0; i < enemiesLength; i++)
	// 	{
	// 		Instantiate(ProjectilePrefab, focalPoint.transform.position, focalPoint.transform.rotation);
	// 	}
	// }

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Enemy") && currentPowerUp == PowerUpType.Pushback) //  && hasKnockBackPowerup
		{
			Rigidbody enemyRB = collision.gameObject.GetComponent<Rigidbody>();
			Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position).normalized;
			enemyRB.AddForce(awayFromPlayer * pushBackPowerupStrength, ForceMode.Impulse);
			Debug.Log("Player collided with: " + collision.gameObject.name + " withpowerup set to " + currentPowerUp.ToString());
			// KnockBack(collision);
		}
	}
	void LaunchRockets()
	{
		foreach (Enemy enemy in FindObjectsOfType<Enemy>())
		{
			tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up, Quaternion.identity);
			tmpRocket.GetComponent<RocketBehaviour>().Fire(enemy.transform);
		}
	}
	// private void KnockBack(Collision collision)
	// {
	// 	Rigidbody enemyRB = collision.gameObject.GetComponent<Rigidbody>();
	// 	Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

	// 	enemyRB.AddForce(awayFromPlayer * KnockBackPowerupStrength, ForceMode.Impulse);
	// }

	IEnumerator Smash()
	{
		var enemies = FindObjectsOfType<Enemy>();

		// Store the y position before taking off
		floorY = transform.position.y;

		// Calculate the amount of time we want to go up
		float jumpTime = Time.time + hangTime;

		while (Time.time < jumpTime)
		{
			// move player up while still keeping their x velocity
			playerRB.velocity = new Vector2(playerRB.velocity.x, smashSpeed);
			yield return null;
		}

		// now move player down
		while (transform.position.y > floorY)
		{
			playerRB.velocity = new Vector2(playerRB.velocity.x, -smashSpeed * 2);
			yield return null;
		}

		//Cycle through all enemies.
		for (int i = 0; i < enemies.Length; i++)
		{
			//Apply an explosion force that originates from our position.
			if (enemies[i] != null)
				enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce,
				transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
		}
		//We are no longer smashing, so set the boolean to false
		smashing = false;

	}
}
