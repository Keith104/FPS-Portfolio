using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damageAmount;
    [SerializeField] float damageRate;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        StartCoroutine(WaitToDie());
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
        if (other.CompareTag("Player"))
        {
            IDamage dmg = other.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.TakeDamage(damageAmount);
            }

            Debug.Log("HIT Player");
        }

        Destroy(gameObject);
    }

    IEnumerator WaitToDie()
    {
        yield return new WaitForSeconds(destroyTime);
        GameManager.instance.Player.GetComponent<IDamage>().TakeDamage(damageAmount);

        Destroy(gameObject);
    }
}
