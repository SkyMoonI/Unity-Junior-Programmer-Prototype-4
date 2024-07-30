using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	[SerializeField] GameObject[] enemyPrefab;
	[SerializeField] GameObject[] powerupPrefabs;

	float spawnRange = 9.0f;

	int enemyCount;
	int waveNumber = 1;


	// Start is called before the first frame update
	void Start()
	{
		SpawnEnemyWave(waveNumber);
		int randomIndex = Random.Range(0, powerupPrefabs.Length);
		Instantiate(powerupPrefabs[randomIndex], GenerateRandomSpawnPosition(), powerupPrefabs[randomIndex].transform.rotation);
	}

	// Update is called once per frame
	void Update()
	{
		enemyCount = FindObjectsOfType<Enemy>().Length;
		if (enemyCount == 0)
		{
			waveNumber++;
			SpawnEnemyWave(waveNumber);
			int randomIndex = Random.Range(0, powerupPrefabs.Length);
			Instantiate(powerupPrefabs[randomIndex], GenerateRandomSpawnPosition(), powerupPrefabs[randomIndex].transform.rotation);
		}
	}
	void SpawnEnemyWave(int enemiesToSpawn)
	{
		for (int i = 0; i < enemiesToSpawn; i++)
		{
			int randomIndex = Random.Range(0, enemyPrefab.Length);
			Instantiate(enemyPrefab[randomIndex], GenerateRandomSpawnPosition(), enemyPrefab[randomIndex].transform.rotation);
		}
	}
	private Vector3 GenerateRandomSpawnPosition()
	{
		float spawnRangeX = Random.Range(-spawnRange, spawnRange);
		float spawnRangeZ = Random.Range(-spawnRange, spawnRange);

		Vector3 spawnPos = new Vector3(spawnRangeX, 0.0f, spawnRangeZ);
		return spawnPos;
	}


}
