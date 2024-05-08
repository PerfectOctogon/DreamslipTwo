using UnityEngine;
public interface Damageable
{
    public void Damage(float damage);
    public void Heal(float healthIncrease);
    public void Die();
}
