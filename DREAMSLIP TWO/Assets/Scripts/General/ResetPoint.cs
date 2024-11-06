using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPoint : MonoBehaviour
{
    public bool pointReached = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && !pointReached)
        {
            StartCoroutine(HintBox.ShowHint("Checkpoint reached"));
            pointReached = true;
        }
    }
}
