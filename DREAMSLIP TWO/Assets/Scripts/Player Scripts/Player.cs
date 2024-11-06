using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, Damageable
{
    public float maxHealth;
    public float health;

    public void Damage(float damage) {
        health -= damage;
        if(health <= 0) { Die(); }
    }

    public void Heal(float healAmount)
    {
        health = health + healAmount >= maxHealth ? (health = maxHealth) : health += healAmount;
    }

    public void Die() {
        print("PLAYER HAS DIED");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
