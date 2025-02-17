using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WraithboundAI : EnemyAI
{
    //public Transform player;                  // Player's Transform
    public float detectionRange = 100f;        // Range to detect the player
    public float chargeSpeed = 50f;            // Speed at which the charger moves
    public float chargeDuration = 5f;         // Duration for charging
    public float cooldownDuration = 5f;
    public LayerMask obstacleLayer;           // Layer mask for obstacles
    public float stoppingDistance = 0.5f;
    public GameObject chargingHitbox;
    [SerializeField]
    private DamageOnPlayerTouch damageOnPlayerTouch;
    
    private CharacterController characterController; // CharacterController component
    private bool isCharging = false;           // Is the charger currently charging
    private float chargeStartTime;             // Time when the charging started
    private bool playerDetected = false;
    private bool isCoolingDown = false;
    Vector3 targetPosition;
    private Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        agent.acceleration = 50;
        //DetectPlayer();
    }

    private void Update()
    {
        if (isDead) return;
        DetectPlayer();
        if (playerDetected && !isCharging)
        {
            StartCoroutine(ChargeTowardsPlayer());
        }

        if (isCharging)
        {
            CheckForWall();
        }

        if (Vector3.Distance(transform.position, targetPosition) <= 2 && isCharging)
        {
            print("Reached target");
            StopAllCoroutines();
            StartCoroutine(StopCharging());
        }
    }

    private IEnumerator ChargeTowardsPlayer()
    {
        animator.SetTrigger("Charge");
        agent.speed = chargeSpeed;
        isCharging = true;
        chargingHitbox.SetActive(true);
        damageOnPlayerTouch.SetDamage(CalculateHitDamage());
        yield return new WaitForSeconds(chargeDuration);
        StopAllCoroutines();
        StartCoroutine(StopCharging());
    }

    private void CheckForWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2, obstacleLayer))
        {
            print("Wall detected");
            StopAllCoroutines();
            StartCoroutine(StopCharging());
        }
    }
    

    private IEnumerator StopCharging()
    {
        animator.SetTrigger("Stop");
        chargingHitbox.SetActive(false);
        agent.SetDestination(transform.position);
        isCharging = false;
        playerDetected = false;
        isCoolingDown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isCoolingDown = false;
        transform.LookAt(player.transform);
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) <= detectionRange && !isCoolingDown)
        {
            print("Detected player");
            targetPosition = player.position;
            // Setting charging location
            agent.SetDestination(targetPosition);
            playerDetected = true;
        }
    }

    private void ChompPlayer()
    {
        
    }
}
