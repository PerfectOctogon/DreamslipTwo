using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIUpdater : MonoBehaviour
{
    public Player player;
    public InsomniaBeamEmitter insomniaBeamEmitter;
    public FocusPulse dreamPulse;
    public Image healthBar;
    public Image manaBar;
    public Image dreamPulseBar;

    private void Update()
    {
        UpdateHealthBar();
        UpdateManaBar();
        UpdateDreamPulseBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, player.health / player.maxHealth, 0.5f);
    }

    private void UpdateManaBar()
    {
        manaBar.fillAmount = Mathf.Lerp(manaBar.fillAmount, insomniaBeamEmitter.mana / insomniaBeamEmitter.maxMana, 0.5f);
    }

    private void UpdateDreamPulseBar()
    {
        dreamPulseBar.enabled = dreamPulse.focusPulseReady;
    }
}
