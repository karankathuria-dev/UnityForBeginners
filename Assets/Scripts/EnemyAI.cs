using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //Deine the possible states of our enemy
    public enum AIState { Patrol , Chase , Attack};
    [SerializeField] private AIState currentState = AIState.Patrol;
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float chaseRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 2f;
    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private float lastAttackTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //start the enemy on its patrol route
        if (currentState == AIState.Patrol && patrolPoints.Length>0)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /* if (player!=null)
         {
             agent.SetDestination(player.position);
         }*/

        //The main state machine logic
        switch (currentState)
        {
            case AIState.Patrol:
                Patrol();
                break;
            case AIState.Chase:
                Chase();
                break;
            case AIState.Attack:
                Attack();
                break;
        }
    }
    private void Patrol()
    {
        // Transition to chase if the player is in range
        if (Vector3.Distance(transform.position , player.position)< chaseRange)
        {
            currentState = AIState.Chase;
            return;
        }
        // If the enemy has reached its patrol point...
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // move to the next patrol point
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }
    private void Chase()
    {
        // Transition back to Patrol if the player is out of range
        if (Vector3.Distance(transform.position,player.position)>chaseRange)
        {
            currentState = AIState.Patrol;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            return;
        }

        //Transition to Attack if the player is in range
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            currentState = AIState.Attack;
            agent.isStopped = true;
            return;
        }

        // continously chase the player
        agent.SetDestination(player.position);
    }
    private void Attack()
    {
        // Transition back to chase if the player out o range
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            currentState = AIState.Chase;
            agent.isStopped = false;
            return;
        }
        // check the attack cooldown to prevent constant damage
        if (Time.time > lastAttackTime + attackCooldown)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
            lastAttackTime = Time.time;

        }
    }
}
