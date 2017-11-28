using UnityEngine;
using System.Collections;

namespace CompleteProject
{

    public class EnemyMovement : MonoBehaviour
    {
        Transform player;               // Reference to the player's position.
      HealthScript playerHealth;      // Reference to the player's health.
        HealthScript enemyHealth;        // Reference to this enemy's health.
        UnityEngine.AI.NavMeshAgent nav;               // Reference to the nav mesh agent.


        void Awake ()
        {
            // Set up the references.
            player = GameObject.FindGameObjectWithTag ("Player").transform;
           playerHealth = player.GetComponent <HealthScript> ();
            enemyHealth = GetComponent <HealthScript> ();
            nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
        }


        void Update ()
        {
          //  If the enemy and the player have health left...
            if(enemyHealth.m_currentHP > 0 && playerHealth.m_currentHP > 0)
          {
                // ... set the destination of the nav mesh agent to the player.
                nav.SetDestination (player.position);
            }
            // Otherwise...
           else
           {
                // ... disable the nav mesh agent.
              nav.enabled = false;
           }
        }
    }
}