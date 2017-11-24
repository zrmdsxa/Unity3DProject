using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
	public int m_MaxHP = 100;
	public float m_deathTime = 3f;
	public float m_hitRect = 0.1f;
	private int m_currenthealth;
	private Animator m_animController;
	private float m_hitDelay;
	private Transform m_argoTarget;
	void start(){
		m_animController = GetComponent<Animator>();
		m_currenthealth = m_MaxHP;
	} 
	void Update(){
		if(m_hitDelay <= 0){
			m_animController.SetBool("tookDamage",false);
		}else{
			m_hitDelay -= Time.deltaTime;
		}
		if(m_currenthealth <= 0){
			Die();
		}
	}
	public void ApplyDamage(int amount){
		m_currenthealth -= amount;
		if(m_currenthealth <= 0){
			m_hitDelay = m_deathTime;
			m_animController.SetBool("tookDamage",true);
		}else{
			m_hitDelay = m_hitRect;
			m_animController.SetBool("tokkDamage",true);
		}
	}
	private void Die(){
		if(m_hitDelay <= 0){
			Destroy(gameObject);
		}
	}public Transform GetArgoTarget(){
		return m_argoTarget? m_argoTarget: null;
	}
	public void SetArgoTarget(Transform target){
		m_argoTarget = target;
	}


}
