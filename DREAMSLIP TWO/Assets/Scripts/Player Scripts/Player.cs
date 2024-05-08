using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float maxHealth;
    public float health;

    public void TakeDamage(float damage) {
        health -= damage;
        if(health <= 0) { Die(); }
    }

    void Die() {
        print("PLAYER HAS DIED");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
