using UnityEngine;

public class BossAI : EnemyAI, IDamage
{
    [SerializeField] GameObject bomb;
    [SerializeField] Transform bombPos;

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
        Instantiate(bomb, bombPos.position, transform.localRotation);
    }
}
