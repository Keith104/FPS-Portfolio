using UnityEngine;

public class MeleeLogic : MonoBehaviour
{
    [SerializeField] Equipment equipment;
    [SerializeField] float AttackRechargeTimer;
    [SerializeField] LayerMask attackIgnoreLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    void Attack()
    {
        if (Input.GetMouseButton(0) && AttackRechargeTimer == 0)
        {
            HitCheck();
        }
        else if(AttackRechargeTimer >= 0)
        {
            AttackRecharge();
        }
    }

    void HitCheck()
    {
        Vector3 explosionCenter = transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(explosionCenter, equipment.range, attackIgnoreLayer);

        foreach (Collider collider in hitColliders)
        {
            IDamage dmg = collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.TakeDamage(equipment.damageAmount);
            }
        }
    }

    void AttackRecharge()
    {
        AttackRechargeTimer += Time.deltaTime;
        if (AttackRechargeTimer >= equipment.fireRate)
        {
            AttackRechargeTimer = 0;
        }
    }
}
