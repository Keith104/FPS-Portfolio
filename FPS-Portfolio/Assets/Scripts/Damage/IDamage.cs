using UnityEngine;

public interface IDamage
{
    abstract public void TakeDamage(float amount, Damage.damagetype damagetype);
}
