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

    private bool canAttack = true;

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
        if (playerInAttackRange && canAttack) {
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
        animator.SetBool("isWalking", false);
        agent.speed = 0;
        transform.LookAt(player);
        if (true/*Physics.Raycast(projectileSpawnLocation.position, Vector3.forward, attackRange, whatIsPlayer)*/)
        {
            StartCoroutine(Attack());
        }
    }

    private void SearchWalkPoint() {
        //animator.SetBool("isWalking", false);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) {
            walkpointSet = true;
        }
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(1.6f);
        GameObject projectileObject = Instantiate(projectile, projectileSpawnLocation.position, Quaternion.identity);
        projectileObject.GetComponent<EnemyProjectile>().SetBulletDamage(CalculateHitDamage());
        projectileObject.GetComponent<EnemyProjectile>().SetDirection((player.position - projectileSpawnLocation.position).normalized);
        yield return new WaitForSeconds(timeBetweenAttacks);
        agent.speed = 3.5f;
        canAttack = true;
        animator.SetBool("isWalking", true);
    }
}
