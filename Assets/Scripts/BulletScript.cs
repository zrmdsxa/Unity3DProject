using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    float m_damage = 0;
    bool m_active = false;

    // Use this for initialization
    void Start()
    {
        m_active = true;
        Destroy(gameObject, 3.0f);
    }

    // Update is called once per frame
    public void SetDamage(float damage)
    {
        m_damage = damage;
    }

    public void OnCollisionEnter(Collision other)
    {
        //Debug.Log(other.gameObject);
        if (m_active)
        {
            if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<HealthScript>().TakeDamage(m_damage);
                DestroyBullet(Vector3.zero);
            }
            else
            {
				m_active = false;
				GetComponent<Rigidbody>().useGravity = true;
                //DestroyBullet(Vector3.zero);
            }
        }

    }

    void DestroyBullet(Vector3 pos)
    {
        Destroy(gameObject);
    }
}
