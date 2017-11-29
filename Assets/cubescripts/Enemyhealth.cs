using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemyhealth : MonoBehaviour {
	public float EnemyMax;
	float currentHealth;
public GameObject enemydeathBar;
public  Slider enemyHealthSlider;

	// Use this for initialization
	void Start () {
		currentHealth = EnemyMax;
		enemyHealthSlider.maxValue = currentHealth;
		enemyHealthSlider.value = currentHealth;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void addDamage(float damage){
enemyHealthSlider.gameObject.SetActive(true);
currentHealth -= damage;
enemyHealthSlider.value = currentHealth;

if(currentHealth <= 0) makeDead();




	}
	void makeDead(){
		Destroy(gameObject);
		GameObject particleDie = Instantiate(enemydeathBar, transform.position, transform.rotation);
		Destroy(particleDie, 1f);



	}
}
