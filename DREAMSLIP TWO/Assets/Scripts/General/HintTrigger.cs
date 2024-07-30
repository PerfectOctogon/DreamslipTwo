using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    public string textToTrigger;
    public bool destroyAfter = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            StartCoroutine(HintBox.ShowHint(textToTrigger));
            if (destroyAfter) Destroy(gameObject);
        }
    }
}
