using System.Collections;
using TMPro;
using UnityEngine;

public class HintBox : MonoBehaviour
{
    private static TextMeshProUGUI hintText;
    private static GameObject container;

    private void Awake()
    {
        container = transform.GetChild(0).gameObject;
        hintText = container.transform.Find("HintText").GetComponent<TextMeshProUGUI>();
    }
    

    public static IEnumerator ShowHint(string hint)
    {
        container.SetActive(true);
        hintText.text = hint;
        yield return new WaitForSeconds(6f);
        container.SetActive(false);
    }
    
}
