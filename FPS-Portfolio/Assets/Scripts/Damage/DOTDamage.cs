using System.Collections;
using UnityEngine;

public class DOTDamage : MonoBehaviour
{

    public float dotTimer;
    public float damageAmount;
    public float damageRate;
    bool isDamaging;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(this, dotTimer);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDamaging)
        {
            StartCoroutine(DamageOther());
        }
    }

    IEnumerator DamageOther()
    {
        isDamaging = true;
        gameObject.GetComponent<EnemyAI>().TakeDamage(damageAmount, Damage.damagetype.DOT);
        yield return new WaitForSeconds(damageRate);
        isDamaging = false;
    }


}
