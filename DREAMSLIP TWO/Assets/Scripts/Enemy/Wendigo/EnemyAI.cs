using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, Damageable
{
    public NavMeshAgent agent;
    public Transform player;
    public Player playerScript;
    public LayerMask whatIsGround, whatIsPlayer;
    public EnemyHealthBarUpdater healthBarUpdater;

    public float minDamage = 10;
    public float maxDamage = 15f;
    public float maxHealth;
    public float health;
    public float critChance = 0.1f;
    public float critMultiplier = 2;
    public bool isDead = false;

    public void Damage(float damage)
    {
        print("Hit");
        health -= damage;
        healthBarUpdater.UpdateHealthBar();
        if (health <= 0) Die();
    }

    public void Heal(float healthIncrease)
    {
        if (health + healthIncrease > maxHealth)
        {
            health = maxHealth;
            healthBarUpdater.UpdateHealthBar();
            return;
        }
        health += healthIncrease;
        healthBarUpdater.UpdateHealthBar();
    }

    public void Die()
    {
        if (isDead) return;
        GetComponent<NavMeshAgent>().speed = 0;
        isDead = true;
        GetComponent<Animator>().SetTrigger("Die");
        StartCoroutine(WaitDestroy(3f));
    }
    
    protected IEnumerator WaitDestroy(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
