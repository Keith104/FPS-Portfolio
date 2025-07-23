using Unity.VisualScripting;
using UnityEngine;

public class SniperAi : EnemyAI, IDamage
{
    [SerializeField] LineRenderer lineRenderer;

    private bool see;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        if (see)
        {
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    public override bool CanSeePlayer()
    {
        see = base.CanSeePlayer();
        lineRenderer.SetPosition(0, shootPos.transform.position);
        lineRenderer.SetPosition(1, player.transform.position);

        lineRenderer.material.color = Color.Lerp(Color.white, Color.red, shootTimer / attackCooldown);

        return see;
    }
}
