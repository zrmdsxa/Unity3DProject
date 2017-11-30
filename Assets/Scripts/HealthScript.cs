using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class HealthScript : MonoBehaviour
{

    public float m_maxHP = 100f;
    public bool m_isPlayer = false;
    Animator anima;

    public Image m_healthBar;
    public float damage;
    public float m_currentHP;                           // Reference to the UI's health bar.
    public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    public AudioClip deathClip;                                 // The audio clip to play when the player dies.
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.
    PlayerMovement playerMovement;                              // Reference to the player's movement.
    GunScript playerShooting;                              // Reference to the PlayerShooting script.
    bool isDead;                                                // Whether the player is dead.
    bool damaged;                                               // True when the player gets damaged.

    // Use this for initialization
    void Start()
    {
        //   playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponentInChildren<GunScript>();

        anima = GetComponent<Animator>();
        m_currentHP = m_maxHP;
        Debug.Log(m_currentHP);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isPlayer)
        {
            if (damaged)
            {
                // ... set the colour of the damageImage to the flash colour.
                damageImage.color = flashColour;
            }
            // Otherwise...
            else
            {
                // ... transition the colour back to clear.
                damageImage.color = Color.Lerp(damageImage.color, new Color(1, 1, 1, 0), flashSpeed * Time.deltaTime);
            }


        }

        // If the player has just been damaged...

        // Reset the damaged flag.
        damaged = false;

        // if (!m_isPlayer )
        // {
        //      if (Input.GetKeyDown(KeyCode.K))
        //      {
        //          TakeDamage(999f);
        //      }
        // }
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
            if (m_isPlayer)
            {
                damaged = true;
                UpdateHealthBar();
            }
            //            Debug.Log(m_currentHP);

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
        Debug.Log("player die");
        anima.SetBool("isDead", true);

        GetComponent<PlayerCameraScript>().enabled = false;
        GetComponent<PlayerMovementScript>().enabled = false;
        GetComponent<PlayerWeaponScript>().enabled = false;
        GameManager.instance.GameOver();
    }
    void EnemyDie()
    {
        Debug.Log("enemydie");
        anima.SetBool("isDead", true);
        GetComponent<NavMeshAgent>().enabled = false;
        Destroy(gameObject, 3f);
    }

}
