using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusPulse : MonoBehaviour
{
    float focusPulseDuration = 1f;
    public float focusPulseCooldown = 45f;
    public bool focusPulseReady = true;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && focusPulseReady) {
            StartCoroutine(SlowTime());
        }
    }

    IEnumerator SlowTime() {
        focusPulseReady = false;
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(focusPulseDuration);
        Time.timeScale = 1f;
        yield return new WaitForSeconds(focusPulseCooldown);
        focusPulseReady = true;
    }
}
