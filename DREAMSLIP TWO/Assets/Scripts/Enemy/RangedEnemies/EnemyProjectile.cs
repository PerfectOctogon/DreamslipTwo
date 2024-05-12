using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Vector3 moveDirection;
    public float bulletSpeed = 10f;
    public float expireTimer = 10f;
    public float bulletExposionRange = 5f;
    private bool hit = false;
    float timer = 0;
    float damage = 5;
    float explosionDamage;
    public GameObject blasterShotEffect;
    public LayerMask enemyLayer;
    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= expireTimer) {
            DestroyBullet(transform.position);
        }
        transform.Translate(moveDirection * bulletSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hit) return;
        hit = true;
        explosionDamage = damage * 0.8f;
        if (other.CompareTag("Enemy") || other.CompareTag("Triggers")) { return; }
        foreach (RaycastHit hit in Physics.SphereCastAll(transform.position, bulletExposionRange, transform.forward)) {
            //print(hit.collider.name);
            if (hit.collider.gameObject.CompareTag("Player")) { hit.collider.gameObject.GetComponent<Player>().Damage(explosionDamage); print("Explosion!"); }
        }
        if (other.CompareTag("Player")) {
            other.gameObject.GetComponent<Player>().Damage(damage);
        }
        DestroyBullet(transform.position);
    }
    
    void DestroyBullet(Vector3 point) {
        Instantiate(blasterShotEffect, point, Quaternion.identity);
        Destroy(gameObject);
    }

    public void SetBulletDamage(float bulletDamage) {
        damage = bulletDamage;
    }
}
