using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUpdater : MonoBehaviour
{
    public EnemyAI enemy;
    Image healthBar;

    private void Awake()
    {
        healthBar = GetComponent<Image>();
    }
    
    public void UpdateHealthBar()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, enemy.health / enemy.maxHealth, 0.5f);
    }
}
