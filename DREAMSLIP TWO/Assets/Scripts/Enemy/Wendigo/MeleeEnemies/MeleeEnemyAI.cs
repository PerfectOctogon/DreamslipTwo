using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyAI : EnemyAI
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
    public bool playerInSightRange, playerInAttackRange;

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

    private void AttackPlayer() {
        agent.SetDestination(transform.position);
        //transform.LookAt(player);
        if (!alreadyAttacked) {
            alreadyAttacked = true;
            animator.SetTrigger("Punch");
            StartCoroutine(TryDamagePlayer());
        }
    }

    IEnumerator TryDamagePlayer() {
        yield return new WaitForSeconds(0.5f);
        if (Physics.CheckSphere(transform.position, attackRange, whatIsPlayer)) {
            float damage = Random.Range(minDamage, maxDamage);
            if(Random.Range(0, 1f) <= critChance) { damage *= critMultiplier; }
            playerScript.TakeDamage(damage);
        }
        yield return new WaitForSeconds(timeBetweenAttacks);
        alreadyAttacked = false;
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
