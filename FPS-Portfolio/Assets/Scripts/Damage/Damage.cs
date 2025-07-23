using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour
{
    public enum damagetype { moving, stationary, DOT, explosion, stun, smoke }
    [SerializeField] damagetype type;
    [SerializeField] Rigidbody rb;

    public int damageAmount;
    [SerializeField] float damageRate;
    public int speed;
    public int destroyTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (type == damagetype.moving || type == damagetype.explosion || type == damagetype.stun)
        {
            Destroy(gameObject, destroyTime);
            if (type == damagetype.moving)
            {
                rb.linearVelocity = transform.forward * speed;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TryDamage(other);
    }

    private void OnTriggerExit(Collider other)
    {
        TryDamage(other);
    }

    void TryDamage(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();

            if (dmg != null && type != damagetype.DOT)
            {
                dmg.TakeDamage(damageAmount, type);
            }

        if (type == damagetype.moving || type == damagetype.explosion || type == damagetype.stun)
        {
            Destroy(gameObject);
        }
    }
}
