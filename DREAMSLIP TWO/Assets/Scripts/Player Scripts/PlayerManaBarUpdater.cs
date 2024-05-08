using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManaBarUpdater : MonoBehaviour
{
    public InsomniaBeamEmitter insomniaBeamEmitter;
    Image manaBar;

    private void Awake()
    {
        manaBar = GetComponent<Image>();
    }
    private void Update()
    {
        manaBar.fillAmount = Mathf.Lerp(manaBar.fillAmount, insomniaBeamEmitter.mana / insomniaBeamEmitter.maxMana, 0.5f);
    }
}
