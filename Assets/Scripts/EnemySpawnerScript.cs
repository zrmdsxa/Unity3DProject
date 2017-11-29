using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour {

	public GameObject[] m_enemies;
	public Transform[] m_spawns;

	public int m_maxEnemies = 10;

	public float m_spawnDelay = 10.0f;

	int m_currentNumEnemies = 0;
	float m_currentSpawnDelay = 10.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(Random.Range(0,m_spawns.Length));

		if (m_currentSpawnDelay > 0.0f){
			m_currentSpawnDelay -= Time.deltaTime;
		}
		else if( m_currentSpawnDelay <= 0.0f){
			if (m_currentNumEnemies < m_maxEnemies){
				SpawnEnemy();
			}
			
		}
		
	}

	void SpawnEnemy(){
		m_currentSpawnDelay = m_spawnDelay;
		GameObject enemy = Instantiate(m_enemies[Random.Range(0,m_enemies.Length)],m_spawns[Random.Range(0,m_spawns.Length)].position,Quaternion.identity);
		m_currentNumEnemies++;
	}

	public void EnemyDied(){
		m_currentNumEnemies--;
	}
}
