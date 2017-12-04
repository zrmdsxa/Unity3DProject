﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    

    //0 = 5.56mm
    //1 = 9mm
    public int m_ammoType = 0;
    public float m_damage = 1.0f;
    public int m_maxRounds = 30;
    public float m_RoF = 10.0f;
    public bool m_auto = false;
    public float m_reloadTime = 3.0f;

    public float m_deviation = 0.0f;

	public Transform m_bulletSpawn;

	public GameObject m_bullet;

	public float m_bulletSpeed = 100.0f;
    public GameObject m_muzzleflashPrefab;

    public GameObject m_soundPrefab;
    //public GameObject m_reloadPrefab;

    int m_remainingShots = 0;
    float m_coolDown = 0.0f;

    AudioSource m_audioReload;

    // Use this for initialization
    void Start()
    {
        
        m_remainingShots = m_maxRounds;
        m_audioReload = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
		m_coolDown -= Time.deltaTime;
    }

    public bool Fire()
    {
        bool fired = false;
        if (m_remainingShots > 0)
        {
            if (m_coolDown <= 0.0f)
            {
                //Debug.Log("GunScript:Fire");
                m_coolDown = 1.0f / m_RoF;
				m_remainingShots--;
                
                
				GameObject b = Instantiate(m_bullet,m_bulletSpawn.position,m_bulletSpawn.rotation);
                Vector3 dir = (b.transform.forward * m_bulletSpeed) + (b.transform.right * Random.Range(-m_deviation,m_deviation)) + (b.transform.up * Random.Range(-m_deviation,m_deviation));
                
				b.GetComponent<Rigidbody>().velocity = dir;
                b.transform.rotation = Quaternion.LookRotation(dir);

                b.GetComponent<BulletScript>().SetDamage(m_damage);

                Destroy(Instantiate(m_soundPrefab,transform.position,Quaternion.identity),1.0f);
                Destroy(Instantiate(m_muzzleflashPrefab,m_bulletSpawn.position,Quaternion.identity),0.3f);
                fired = true;
                //Debug.Log(b);
            }

        }
        return fired;
    }

    public int GetRemainingBullets(){
        return m_remainingShots;
    }

    public void ReloadBullets(int bullets){
        m_remainingShots = bullets;
    }

    public float StartReload(){
        //Destroy(Instantiate(m_reloadPrefab,transform.position,Quaternion.identity),m_reloadTime);
        m_audioReload.Play();
        return m_reloadTime;
    }

    public void CancelReload(){
        m_audioReload.Stop();
    }

    public int GetAmmoType(){
        return m_ammoType;
    }
}
