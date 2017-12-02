using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnerScript : MonoBehaviour
{
	public static EnemySpawnerScript instance;
    public GameObject[] m_enemies;
    public Transform[] m_spawns;

    public int m_numEnemiesToSpawn = 20;
    public int m_maxEnemies = 10;   //max active enemies allowed

    public float m_spawnDelay = 10.0f;

	public Text m_enemiesText;

    int m_currentNumEnemies = 0;

    int m_enemiesToSpawn;  //keep track of how many we need to spawn

    int m_enemiesLeft;  //total enemies left to kill
	
    float m_currentSpawnDelay = 10.0f;

    // Use this for initialization
    void Start()
    {
		if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        m_enemiesLeft = m_numEnemiesToSpawn;
        m_enemiesToSpawn = m_numEnemiesToSpawn;
		UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_enemiesToSpawn > 0)
        {

            if (m_currentSpawnDelay > 0.0f)
            {
                m_currentSpawnDelay -= Time.deltaTime;
            }
            else if (m_currentSpawnDelay <= 0.0f)
            {
                if (m_currentNumEnemies < m_maxEnemies)
                {
                    SpawnEnemy();
                }

            }
        }

    }

    void SpawnEnemy()
    {
        m_enemiesToSpawn--;
        m_currentSpawnDelay = m_spawnDelay;
        GameObject enemy = Instantiate(m_enemies[Random.Range(0, m_enemies.Length)], m_spawns[Random.Range(0, m_spawns.Length)].position, Quaternion.identity);
        m_currentNumEnemies++;
		
    }

    public void EnemyDied()
    {
        m_currentNumEnemies--;
        m_enemiesLeft--;
        
		Debug.Log("current:"+m_currentNumEnemies+" left:"+m_enemiesLeft);
		if(m_enemiesLeft <= 0 && m_currentNumEnemies <= 0){
			GameManager.instance.GameWin();
		}
		UpdateText();
    }

	void UpdateText(){
		m_enemiesText.text = "Enemies\n"+m_enemiesLeft;
	}
}
