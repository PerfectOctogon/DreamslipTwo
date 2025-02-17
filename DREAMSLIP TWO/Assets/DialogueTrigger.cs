using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogueSystem;
    public string dialogueText;
    public bool isManySentences;
    public string[] dialogueTexts;

    private bool isActive = false;
    private void OnTriggerEnter(Collider other)
    {
        if (isActive) return;
        if (other.transform.tag.Equals("Player"))
        {
            if (isManySentences)
            {
                StartCoroutine(dialogueSystem.sayWaitDisable(dialogueTexts));
                isActive = true;
            }
            else
            {
                StartCoroutine(dialogueSystem.sayWaitDisable(dialogueText));
                isActive = true;
            }
        }
    }
}
