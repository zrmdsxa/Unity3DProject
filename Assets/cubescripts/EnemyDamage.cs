using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour {
	public float damage;
	public float damageRate;
	public float pushBackForce;
	float nextDamage;


	HealthScript enemyHealth;  


	// Use this for initialization
	void Start () {
		nextDamage = 0f;
		enemyHealth = GetComponent<HealthScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider other){
		if (other.tag == "Player" && nextDamage<Time.time && enemyHealth.m_currentHP > 0){
			HealthScript thePlayerHealth = other.gameObject.GetComponent<HealthScript>();
			thePlayerHealth.TakeDamage(damage);
			nextDamage = Time.time + damageRate;
		}

	}
	// void pushBack(Transform pushedObject){
	// 	Vector2 pushDirection = new Vector2(0,(pushedObject.position.y - transform.position.y)).normalized;
	// 	pushDirection *= pushBackForce;
	// 	Rigidbody2D pushRB = pushedObject.gameObject.GetComponent<Rigidbody2D>();
	// 	pushRB.velocity = Vector2.zero;
	// 	pushRB.AddForce(pushDirection, ForceMode2D.Impulse);
	//}
}
