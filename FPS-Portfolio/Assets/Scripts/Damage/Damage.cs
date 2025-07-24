using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour
{
    public enum damagetype { moving, stationary, DOT, explosion, stun, smoke, homing}
    [SerializeField] damagetype type;
    [SerializeField] Rigidbody rb;
    [SerializeField] float checkRadius;
    [SerializeField] string targetTag;
    [SerializeField] LayerMask enemyLayer;

    public float damageAmount;
    [SerializeField] float damageRate;
    [SerializeField] int regSpeed;
    [SerializeField] int homeSpeed;
    public int destroyTime;

    PickupSystem pickupSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        pickupSystem = GameManager.instance.Player.GetComponent<PickupSystem>();

        if (type == damagetype.moving || 
            type == damagetype.explosion || 
            type == damagetype.stun || 
            type == damagetype.smoke || 
            type == damagetype.homing || 
            type == damagetype.DOT
            )
        {
            Destroy(gameObject, destroyTime);
            if (type == damagetype.moving || type == damagetype.DOT)
            {
                rb.linearVelocity = transform.forward * regSpeed;
            }
        }
    }

    private void Update()
    {
        if (type == damagetype.homing)
        {
            GameObject closestObject = Closest(targetTag, transform.position, checkRadius);
            if (closestObject == null)
            {
                rb.linearVelocity = transform.forward * regSpeed;
            }
            else
            {
                Debug.Log(closestObject);
                Vector3 dir = (closestObject.transform.position - transform.position).normalized;
                rb.linearVelocity = dir * homeSpeed * Time.deltaTime;

            }
        }
    }

    GameObject Closest(string tag, Vector3 center, float radius)
    {
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, radius,enemyLayer, QueryTriggerInteraction.Collide);
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach(Collider collider in hitCollider)
        {
            if(collider.CompareTag(tag))
            {
                Debug.Log("In Compare Tag");
                float distance = Vector3.Distance(center, collider.transform.position);
                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = collider.gameObject;
                    Debug.Log(closestObject);
                }
            }
        }

        return closestObject;
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
        Debug.Log("Check");
        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null && type != damagetype.DOT)
        {
            dmg.TakeDamage(damageAmount, type);
        }

        if (type == damagetype.moving || 
            type == damagetype.explosion || 
            type == damagetype.stun || 
            type == damagetype.homing)
        {
            Destroy(gameObject);
        }
        else if(type == damagetype.DOT && dmg != null)
        {
            DOTDamage damage = other.gameObject.AddComponent<DOTDamage>();
            damage.damageAmount = pickupSystem.damageAmount;
            damage.damageRate = pickupSystem.damageRate;
            damage.dotTimer = pickupSystem.dotTimer;
            Destroy(gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.75f, 0.0f, 0.0f, 0.75f);
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
