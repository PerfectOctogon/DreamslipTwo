using System.Collections;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogue;
    public float textSpeed = 0.01f;
    private AudioSource textSound;
    public float timeBetweenDialogue = 3;

    private void Start()
    {
        textSound = gameObject.GetComponent<AudioSource>();
    }
    
    private IEnumerator sayText(string textToSay)
    {
        dialogue.text = "";
        foreach (char c in textToSay)
        {
            dialogue.text += c;
            textSound.Play();
            yield return new WaitForSeconds(textSpeed);
        }
    }
    
    public void say(string textToSay)
    {
        StopAllCoroutines();
        StartCoroutine(sayText(textToSay));
    }
    
    public IEnumerator sayWaitDisable(string textToSay)
    {
        StopAllCoroutines();
        //DO NOT EXTRACT
        dialogue.text = "";
        foreach (char c in textToSay)
        {
            dialogue.text += c;
            textSound.Play();
            yield return new WaitForSeconds(textSpeed);
        }
        yield return new WaitForSeconds(timeBetweenDialogue);
        dialogue.text = "";
    }
    
    public IEnumerator sayWaitDisable(string[] textsToSay)
    {
        StopAllCoroutines();
        foreach(string textToSay in textsToSay)
        {
            //DO NOT EXTRACT
            dialogue.text = "";
            foreach (char c in textToSay)
            {
                dialogue.text += c;
                textSound.Play();
                yield return new WaitForSeconds(textSpeed);
            }
            yield return new WaitForSeconds(timeBetweenDialogue);
        }
        dialogue.text = "";
    }
    
}
