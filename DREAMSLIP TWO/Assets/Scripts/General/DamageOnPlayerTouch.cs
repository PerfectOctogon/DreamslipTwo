using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageOnPlayerTouch : MonoBehaviour
{
    public float damageToApply;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Player hit");
            other.GetComponent<Player>().Damage(damageToApply);
        }
    }

    public void SetDamage(float damage)
    {
        damageToApply = damage;
    }
}
