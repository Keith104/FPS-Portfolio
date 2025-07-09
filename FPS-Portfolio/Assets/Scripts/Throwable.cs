using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Throwable : MonoBehaviour
{
    [SerializeField] Equipment equipment;

    [SerializeField] Rigidbody throwRB;
    [SerializeField] LineRenderer throwLR;
    [SerializeField] SphereCollider explosionRadius;
    [SerializeField] LayerMask trajectoryLayerMask;

    private bool thrown;
    private bool hasExploded = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (thrown == false) 
            Throw();
        else if (equipment.isSpining == true)
            Spin();

        if (equipment.detonationCountdown <= 0 && equipment.isImpact == false)
        {
            Explode();
        }
        else if (equipment.detonationCountdown > 0 && equipment.isImpact == false && thrown == true)
        {
            equipment.detonationCountdown -= Time.deltaTime; 
        }
        else if (hasExploded == true)
        {
            // sometimes Destroy doesn't work in Explode() so this is a backup to make sure it gets destroyed
            Destroy(gameObject);
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
            throwRB.AddForce(transform.forward * equipment.forceMult, ForceMode.Impulse);
            thrown = true;
        }
        Trajectory();
    }

    void Spin()
    {
        transform.Rotate(0, equipment.spinSpeed * Time.deltaTime, 0);
    }

    void Explode()
    {
        hasExploded = true;
        Vector3 explosionCenter = transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(explosionCenter, explosionRadius.radius);

        foreach (Collider collider in hitColliders)
        {
            IDamage dmg = collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.TakeDamage(equipment.damageAmount);
            }
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(equipment.isSticky == true)
        {
            transform.SetParent(collision.transform, true);
            throwRB.isKinematic = true;
        }

        if(equipment.isImpact == true)
            Explode();
    }

    void Trajectory()
    {
        List<Vector3> lineRendPonts = new List<Vector3>();
        int totSteps = (int)(10 / 0.01f); // duration div by the amount of time between each check
        Vector3 startPos = transform.position;
        Vector3 forceVelocity = equipment.forceMult * transform.forward;
        
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
