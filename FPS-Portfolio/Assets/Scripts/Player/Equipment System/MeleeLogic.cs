using UnityEngine;

public class MeleeLogic : MonoBehaviour
{
    [SerializeField] SwappingSystem swappingSystem;
    [SerializeField] Damage damage;
    public Equipment equipment;
    [SerializeField] LayerMask attackIgnoreLayer;
    [SerializeField] BoxCollider attackCollider;
    private float attackRechargeTimer;
    private float attackLengthTimer;
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
        if (Input.GetMouseButton(0) && attackRechargeTimer == 0)
        {
            attackCollider.enabled = true;
            AttackRecharge();
        }
        else if(attackRechargeTimer > 0)
        {
            AttackRecharge();
        }
    }

    void AttackRecharge()
    {
        attackRechargeTimer += Time.deltaTime;
        attackLengthTimer += Time.deltaTime;

        if (attackRechargeTimer >= equipment.attackLength)
        {
            attackLengthTimer = 0;
            attackCollider.enabled = false;
        }

        if (attackRechargeTimer >= equipment.fireRate)
        {
            attackRechargeTimer = 0;
        }
    }
    public void updateMeleeUI()
    {
        damage.damageAmount = equipment.damageAmount;
        UIManager.instance.SetGun(equipment.weaponImage, "", 0, 0);
    }
    public void ChangeMelee()
    {
        swappingSystem.DestroyCurrentGun();
        Instantiate
            (
            equipment.weaponPrefab,
            swappingSystem.gunModel.transform.position,
            swappingSystem.gunModel.transform.rotation,
            swappingSystem.gunModel.transform
            );
    }
}
