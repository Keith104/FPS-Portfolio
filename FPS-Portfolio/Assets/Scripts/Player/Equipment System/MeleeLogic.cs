using UnityEngine;

public class MeleeLogic : MonoBehaviour
{
    [SerializeField] Equipment equipment;
    [SerializeField] LayerMask attackIgnoreLayer;
    [SerializeField] BoxCollider attackCollider;
    private float AttackRechargeTimer;
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
            AttackRecharge();
        }
        else if(AttackRechargeTimer > 0)
        {
            AttackRecharge();
        }
    }

    void HitCheck()
    {
        Collider[] hitColliders = Physics.OverlapBox(attackCollider.center, attackCollider.size / 2, Quaternion.identity, attackIgnoreLayer);

        foreach (Collider collider in hitColliders)
        {
            IDamage dmg = collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.TakeDamage(equipment.damageAmount, Damage.damagetype.stationary);
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
    public void updateMeleeUI()
    {
        UIManager.instance.SetGun(equipment.weaponName, 0, 0);
    }
}
