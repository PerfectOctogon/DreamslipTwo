using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUpdater : MonoBehaviour
{
    public Player player;
    Image healthBar;

    private void Awake()
    {
        healthBar = GetComponent<Image>();
    }
    private void Update()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, player.health / player.maxHealth, 0.5f);
        //healthBar.fillAmount = player.health / player.maxHealth;
    }
}
