using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour, Damageable
{
    public float maxHealth = 100;
    public float health;
    bool died = false;

    private void Awake()
    {
        health = maxHealth;
    }

    public void Damage(float damage) {
        health -= damage;
        if(health <= 0 && !died) { Die(); }
    }

    public void Heal(float healAmount)
    {
        //First time using ternary operator :3
        health = health + healAmount >= maxHealth ? (health = maxHealth) : health += healAmount;
    }

    public void Die()
    {
        died = true;
        GetComponent<NavMeshAgent>().speed = 0;
        GetComponent<Animator>().SetTrigger("Die");
        StartCoroutine(WaitDestroy(3f));
    }

    IEnumerator WaitDestroy(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
