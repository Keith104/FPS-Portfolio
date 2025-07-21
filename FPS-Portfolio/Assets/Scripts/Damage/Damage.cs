using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    public int damageAmount;
    [SerializeField] float damageRate;
    [SerializeField] int speed;
    public int destroyTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = transform.forward * speed;
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

            if (dmg != null)
            {
                dmg.TakeDamage(damageAmount);
            }

        Destroy(gameObject);
    }
}
