using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Throwable : MonoBehaviour
{
    [SerializeField] Rigidbody throwRB;
    [SerializeField] LineRenderer throwLR;

    [SerializeField] bool isSpining;
    [SerializeField] int spinSpeed;


    [SerializeField] bool isSticky;
    [SerializeField] bool isImpact;

    private bool thrown;

    [SerializeField] int forceMult;
    [SerializeField] int bouncyness;
    [SerializeField] float detonationCountdown;
    [SerializeField] int damageAmount;

    [SerializeField] SphereCollider explosionRadius;
    [SerializeField] LayerMask trajectoryLayerMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (thrown == false) 
            Throw();
        else if (isSpining == true)
            Spin();

        if (detonationCountdown <= 0 && isImpact == false)
        {
            Explode();
        }
        else if (detonationCountdown > 0 && isImpact == false && thrown == true)
        {
            detonationCountdown -= Time.deltaTime; 
        }
    }

    void Throw()
    {
        if (Input.GetMouseButton(0))
        {
            // adds instantaneous force in the forward direction of camera
            throwLR.enabled = false;
            throwRB.isKinematic = false;
            transform.SetParent(null); // sets object to root
            throwRB.AddForce(transform.forward * forceMult, ForceMode.Impulse);
            thrown = true;
        }
        Trajectory();
    }

    void Spin()
    {
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }

    void Explode()
    {
        Vector3 explosionCenter = transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(explosionCenter, explosionRadius.radius);

        foreach (Collider collider in hitColliders)
        {
            IDamage dmg = collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.TakeDamage(damageAmount);
            }
        }


        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isSticky == true)
        {
            transform.SetParent(collision.transform, true);
            throwRB.isKinematic = true;
        }

        if(isImpact == true)
            Explode();
    }

    void Trajectory()
    {
        List<Vector3> lineRendPonts = new List<Vector3>();
        int totSteps = (int)(10 / 0.01f); // duration div by the amount of time between each check
        Vector3 startPos = transform.position;
        Vector3 forceVelocity = forceMult * transform.forward;
        
        float time = 0;
        for (int i = 0; i < totSteps; ++i)
        {
            Vector3 calcPosition = (forceVelocity * time) + (Physics.gravity / 2 * time * time) + startPos;
            lineRendPonts.Add(calcPosition);
            time += 0.01f;

            if(RayCollisionCheck(calcPosition, 0.1f))
                break;
        }

        throwLR.positionCount = lineRendPonts.Count;
        for (int i = 0; lineRendPonts.Count > 0; ++i)
        {
            throwLR.SetPosition(i, lineRendPonts[i]);
        }
    }

    private bool RayCollisionCheck(Vector3 position, float checkRadius)
    {
        bool retVal = false;
        Collider[] collisions = Physics.OverlapSphere(position, checkRadius, trajectoryLayerMask);
        if (collisions.Length > 0)
        {
            retVal = true;
        }
        return retVal;
    }

    void updateThrowableUI()
    {

    }
}
