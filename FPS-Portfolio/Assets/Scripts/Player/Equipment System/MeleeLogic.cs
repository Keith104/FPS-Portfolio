using UnityEngine;

public class MeleeLogic : MonoBehaviour
{
    [SerializeField] SwappingSystem swappingSystem;
    [SerializeField] Equipment equipment;
    [SerializeField] GameObject weapon;
    [SerializeField] LayerMask attackIgnoreLayer;
    [SerializeField] BoxCollider attackCollider;
    private float AttackRechargeTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        updateMeleeUI();
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
        UIManager.instance.SetGun(equipment.weaponImage, "", 0, 0);
    }
    public void ChangeMelee()
    {
        swappingSystem.DestroyCurrentGun();
        Instantiate
            (
            weapon,
            swappingSystem.gunModel.transform.position,
            swappingSystem.gunModel.transform.rotation,
            swappingSystem.gunModel.transform
            );
    }
}
