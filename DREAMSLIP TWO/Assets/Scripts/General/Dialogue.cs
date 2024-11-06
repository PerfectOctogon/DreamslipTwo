using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogue;
    public float textSpeed = 0.01f;
    private AudioSource textSound;

    private void Start()
    {
        textSound = gameObject.GetComponent<AudioSource>();
    }

    public void updateDialogueAsset(TextMeshProUGUI dialogue)
    {
        this.dialogue = dialogue;
    }

    public void say(string textToSay)
    {
        StopAllCoroutines();
        StartCoroutine(sayText(textToSay));
    }

    public IEnumerator say(string[] textsToSay)
    {
        StopAllCoroutines();
        foreach(string textToSay in textsToSay)
        {
            float waitTime = textToSay.Length * textSpeed + 1f;
            StartCoroutine(sayText(textToSay));
            yield return new WaitForSeconds(waitTime);
        }
    }

    public IEnumerator sayWaitDisable(string textToSay)
    {
        StopAllCoroutines();
        StartCoroutine(sayText(textToSay));
        yield return new WaitForSeconds(3);
        StartCoroutine(sayText(""));
    }
    
    public IEnumerator sayWaitDisable(string[] textsToSay)
    {
        StopAllCoroutines();
        foreach(string textToSay in textsToSay)
        {
            StartCoroutine(sayText(textToSay));
            yield return new WaitForSeconds(3);
        }
        StartCoroutine(sayText(""));
    }

    public IEnumerator sayText(string textToSay)
    {
        dialogue.text = "";
        foreach (char c in textToSay)
        {
            dialogue.text += c;
            textSound.Play();
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
