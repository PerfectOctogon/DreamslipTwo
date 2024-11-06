using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WraithboundAI : EnemyAI
{
    //public Transform player;                  // Player's Transform
    public float detectionRange = 10f;        // Range to detect the player
    public float chargeSpeed = 5f;            // Speed at which the charger moves
    public float obstacleDetectionDistance = 1f; // Distance to detect obstacles
    public float chargeDuration = 5f;         // Duration for charging
    public LayerMask obstacleLayer;           // Layer mask for obstacles

    private NavMeshAgent agent;                // NavMeshAgent component
    private CharacterController characterController; // CharacterController component
    private bool isCharging = false;           // Is the charger currently charging
    private float chargeStartTime;             // Time when the charging started

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (isCharging)
        {
            // Check if the charging duration has elapsed
            if (Time.time - chargeStartTime > chargeDuration)
            {
                StopCharging();
                return;
            }

            // Check for obstacles in front of the charger
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, obstacleDetectionDistance, obstacleLayer))
            {
                StopCharging();
                return;
            }

            // Keep charging towards the player
            agent.SetDestination(player.position);
            agent.speed = chargeSpeed;
        }
        else
        {
            // Detect if the player is within range to start charging
            if (Vector3.Distance(transform.position, player.position) <= detectionRange)
            {
                StartCharging();
            }
        }
    }

    private void StartCharging()
    {
        isCharging = true;
        chargeStartTime = Time.time;
        agent.isStopped = false; // Ensure the agent is not stopped
        characterController.enabled = false; // Disable CharacterController if necessary
    }

    private void StopCharging()
    {
        isCharging = false;
        agent.isStopped = true; // Stop the agent
        agent.ResetPath();      // Reset the agent path
        characterController.enabled = true; // Re-enable CharacterController if necessary
    }
}
