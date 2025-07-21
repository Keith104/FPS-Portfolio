using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour
{
    enum damagetype { moving, stationary, DOT, explosion, stun, smoke }

    [SerializeField] damagetype type;
    [SerializeField] Rigidbody rb;

    public int damageAmount;
    [SerializeField] float damageRate;
    [SerializeField] int speed;
    public int destroyTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (type == damagetype.moving || type == damagetype.explosion)
        {
            Destroy(gameObject, destroyTime);

            if (type == damagetype.moving)
            {
                rb.linearVelocity = transform.forward * speed;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamage dmg = other.GetComponent<IDamage>();

        if(dmg != null && type != damagetype.DOT)
        {
            dmg.TakeDamage(damageAmount);
        }

        if(type == damagetype.moving || type == damagetype.explosion)
        {
            Destroy(gameObject);
        }
    }
}
