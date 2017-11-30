using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	float m_damage = 0;

	// Use this for initialization
	void Start () {
		Destroy(gameObject,3.0f);
	}
	
	// Update is called once per frame
	public void SetDamage(float damage){
		m_damage = damage;
	}

	public void OnCollisionEnter(Collision other){
		//Debug.Log(other.gameObject);
		if (other.gameObject.tag == "Enemy"){
			other.gameObject.GetComponent<HealthScript>().TakeDamage(m_damage);
			DestroyBullet(Vector3.zero);
		}
		else{
			DestroyBullet(Vector3.zero);
		}
	}
	
	void DestroyBullet(Vector3 pos){
		Destroy(gameObject);
	}
}
