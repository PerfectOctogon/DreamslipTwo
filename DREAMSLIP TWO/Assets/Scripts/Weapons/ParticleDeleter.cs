using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDeleter : MonoBehaviour
{
    public float timer = 1f;
    void Awake() {
        StartCoroutine(DeleteParticle());
    }
    IEnumerator DeleteParticle() {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
