using UnityEngine;
using System.Collections;



    public class EnemyAttack : MonoBehaviour
    {
        public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
        public int attackDamage = 10;               // The amount of health taken away per attack.


        Animator anim;                              // Reference to the animator component.

        HealthScript playerHealth;
        HealthScript enemyHealth;                    // Reference to this enemy's health.
        bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
        float timer;    
        GameObject thePlayer;                            // Timer for counting up to the next attack.


        void Awake ()
        {
            thePlayer = GameObject.FindGameObjectWithTag ("Player");
            playerHealth = thePlayer.GetComponent<HealthScript>();


            enemyHealth = GetComponent<HealthScript>();
            anim = GetComponent <Animator> ();
        }


        void OnTriggerEnter (Collider other)
        {
            // If the entering collider is the player...
//            Debug.Log(other);
            if(other.tag == "Player")
            {  
                 anim.SetBool ("isAttacking",true);
//                 Debug.Log(anim.GetBool("isPouncing"));

                // ... the player is in range.
                playerInRange = true;
            }
        }


        void OnTriggerExit (Collider other)
        {
            // If the exiting collider is the player...
            if(other.tag == "Player")
            {
                     anim.SetBool ("isAttacking",false);
                // ... the player is no longer in range.
                playerInRange = false;
            }
        }


        void Update ()
        {
            // Add the time since Update was last called to the timer.
            timer += Time.deltaTime;

            // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
            if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.m_currentHP > 0)
            {
                // ... attack.
                Attack ();
            }

            // // If the player has zero or less health...
            // if(playerHealth.m_currentHP <= 0)
            // {
            //     // ... tell the animator the player is dead.
            //     anim.SetTrigger ("PlayerDead");
            // }
        }


        void Attack ()
        {
            // Reset the timer.
            timer = 0f;

            // // If the player has health to lose...
            // if(playerHealth.m_currentHP > 0)
            // {
            //     // ... damage the player.
            //     playerHealth.TakeDamage (attackDamage);
            // }
        }
    }
