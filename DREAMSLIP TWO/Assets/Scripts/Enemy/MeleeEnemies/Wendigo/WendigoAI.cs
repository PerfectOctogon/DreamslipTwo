using UnityEngine;
using UnityEngine.AI;

public class WendigoAI : MeleeEnemyAI
{
    
    private void Awake()
    {
        
        health = maxHealth;
        if (player == null)
        {
            player = GameObject.Find("Player").transform;
            playerScript = player.GetComponent<Player>();
        }
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
}
