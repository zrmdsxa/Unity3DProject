using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	float m_damage = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetDamage(float damage){
		m_damage = damage;
	}

	public void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "Enemy"){
			other.gameObject.GetComponent<HealthScript>().TakeDamage(m_damage);
		}
	}
}
