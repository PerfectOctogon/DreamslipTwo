using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    public float maxHealth = 100;
    public float health;
    bool died = false;

    private void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if(health <= 0) { Die(); }
    }

    void Die() {
        if (died) return;
        GetComponent<NavMeshAgent>().speed = 0;
        died = true;
        GetComponent<Animator>().SetTrigger("Die");
        StartCoroutine(WaitDestroy(3f));
    }

    IEnumerator WaitDestroy(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
