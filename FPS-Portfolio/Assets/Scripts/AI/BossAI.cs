using UnityEngine;

public class BossAI : EnemyAI, IDamage
{
    [SerializeField] GameObject bomb;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Shoot()
    {
        base.Shoot();
        Instantiate(bomb, shootPos.position, transform.localRotation);
    }
}
