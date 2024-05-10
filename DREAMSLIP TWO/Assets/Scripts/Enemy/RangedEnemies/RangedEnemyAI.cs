using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAI : EnemyAI
{
    protected Animator animator;

    //Patroling
    public Vector3 walkPoint;
    bool walkpointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange, alertedSightRange;
    public bool playerInSightRange, playerInAttackRange, playerInAttackPath;

    protected GameObject projectile;
    [SerializeField] private Transform projectileSpawnLocation;
    
    private void Update()
    {
        if(isDead) return;
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        

        if (!playerInSightRange && !playerInAttackRange) {
            Patroling();
        }
        if (playerInSightRange && !playerInAttackRange) {
            sightRange = alertedSightRange;
            ChasePlayer();
        }
        if (playerInAttackRange) {
            AttackPlayer();
        }
    }

    private void Patroling() {
        if (!walkpointSet) SearchWalkPoint();

        if (walkpointSet) {
            animator.SetBool("isWalking", true);
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) {
            walkpointSet = false;
        }
    }

    private void ChasePlayer() {
        animator.SetBool("isWalking", true);
        agent.SetDestination(player.position);
    }

    protected void AttackPlayer()
    {
        Debug.Log("Player attackable!");
        if (Physics.Raycast(projectileSpawnLocation.position, Vector3.forward, attackRange, whatIsPlayer))
        {
            Debug.Log("Attacked player!");
            Instantiate(projectile, projectileSpawnLocation);
        }
    }

    private void SearchWalkPoint() {
        animator.SetBool("isWalking", false);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) {
            walkpointSet = true;
        }
    }
}
