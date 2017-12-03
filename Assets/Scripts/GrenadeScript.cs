using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{

    public float m_fuse = 4.0f;
    public float m_damage = 50.0f;

	public GameObject m_explosionPrefab;    //explosion particle system

    public GameObject m_soundPrefab;       //explosion sound

    float m_range = 3.0f; //set as same value as collider radius

    List<GameObject> m_thingsInRange;
    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, m_fuse);
        m_thingsInRange = new List<GameObject>();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy" || other.tag == "Grenade")
        {
            m_thingsInRange.Remove(other.gameObject);
            //Debug.Log("Remove:" + other.name);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy" || other.tag == "Grenade")
        {
            m_thingsInRange.Add(other.gameObject);
            //Debug.Log("Add:" + other.name + ":" + name);
        }

    }

    void OnDestroy()
    {
		Destroy(Instantiate(m_explosionPrefab,transform.position,Quaternion.identity),3.0f);
        Destroy(Instantiate(m_soundPrefab,transform.position,Quaternion.identity),1.0f);
        foreach (GameObject go in m_thingsInRange)
        {
            if (go != null)
            {
                Vector3 dir = (go.transform.position - transform.position);
                float magnitude = dir.magnitude;
                //Debug.Log("mag:" + magnitude + ":" + go.name);
                if (go.tag != "Grenade")
                {
                    go.GetComponent<HealthScript>().TakeDamage(m_damage * ((m_range - (go.transform.position - transform.position).magnitude)) / m_range);
                    Debug.Log("Explode:" + go + ": damage:" + (m_damage * ((m_range - (go.transform.position - transform.position).magnitude)) / m_range));
                }
                else
                {
                    go.GetComponent<Rigidbody>().AddForce(dir.normalized * (1.0f/magnitude) * 2.0f,ForceMode.Impulse);
                }
            }

        }
    }
}
