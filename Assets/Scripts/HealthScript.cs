using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{

    public float m_maxHP = 100f;
    public bool m_isPlayer = false;
    Animator anima;

    public Image m_healthBar;

    public float m_currentHP;
    // Use this for initialization
    void Start()
    {
        anima = GetComponent<Animator>();
        m_currentHP = m_maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isPlayer)
        {
             if (Input.GetKeyDown(KeyCode.K))
             {
                 TakeDamage(999f);
             }
        }
    }

    void UpdateHealthBar()
    {
        if (m_isPlayer)
        {
            m_healthBar.fillAmount = m_currentHP / m_maxHP;
        }
    }

    public void TakeDamage(float damage)
    {
        if (m_currentHP > 0)
        {
            m_currentHP -= damage;
            if (m_currentHP <= 0)
            {
                if (m_isPlayer)
                {
                    PlayerDie();
                }
                else
                {
                    EnemyDie();
                }
            }
            else if (m_isPlayer)
            {
                if (m_currentHP >= 75.0f)
                {
                    m_healthBar.color = Color.green;
                }
                else if (m_currentHP <= 25.0f)
                {
                    m_healthBar.color = Color.red;
                }
                else
                {
                    m_healthBar.color = Color.yellow;
                }
            }
        }
    }

    void PlayerDie()
    {

    }
    void EnemyDie()
    {Debug.Log("enemydie");
        anima.SetBool("isDead", true);
        
        Destroy(gameObject, 3f);
    }
}
