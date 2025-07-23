using UnityEngine;

public class BulletAfterEffect : MonoBehaviour
{
    [SerializeField] GameObject afterEffect;
    private Transform origParent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        origParent = transform.parent;
        transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        StayWithOrig();
    }

    void StayWithOrig()
    {
        if (origParent != null)
        {
            transform.position = origParent.position;
        }
        else
        {
            PlayEffect();
        }
    }

    void PlayEffect()
    {
        Instantiate(afterEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
