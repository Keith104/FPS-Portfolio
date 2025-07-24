using UnityEngine;

public class MeleeLogic : MonoBehaviour
{
    [SerializeField] SwappingSystem swappingSystem;
    [SerializeField] Damage damage;
    public Equipment equipment;
    [SerializeField] LayerMask attackIgnoreLayer;
    [SerializeField] BoxCollider attackCollider;
    private GameObject meleeObj;
    private float attackRechargeTimer;
    private float attackLengthTimer;
    private Color colorOg;
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
            meleeObj.GetComponent<Renderer>().material.color = Color.red;
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
            meleeObj.GetComponent<Renderer>().material.color = Color.gray;
        }

        if (attackRechargeTimer >= equipment.attackRecharge)
        {
            attackRechargeTimer = 0;
            meleeObj.GetComponent<Renderer>().material.color = colorOg;
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
        attackCollider.center = equipment.attackCenter;
        attackCollider.size = equipment.attackSize;
        meleeObj = Instantiate
            (
            equipment.weaponPrefab,
            swappingSystem.gunModel.transform.position,
            swappingSystem.gunModel.transform.rotation,
            swappingSystem.gunModel.transform
            );

        colorOg = meleeObj.GetComponent<Renderer>().material.color;
    }
}
