using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Throwable : MonoBehaviour
{
    [SerializeField] float chargeInc;

    [SerializeField] GameObject throwFab;
    [SerializeField] GameObject explosion;
    [SerializeField] Vector3 throwPos;

    [SerializeField] Rigidbody throwRB;
    [SerializeField] LineRenderer throwLR;
    [SerializeField] SphereCollider explosionRadius;
    [SerializeField] LayerMask trajectoryLayerMask;

    private bool thrown;
    private float detCountdown;
    private float currentThrowForce;
    private Equipment equipment;
    
    public ThrowableSpawner throwableSpawner;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        equipment = GetComponentInParent<ThrowableSpawner>().equipment;
        detCountdown = equipment.detonationCountdown;
        throwableSpawner = transform.parent.GetComponent<ThrowableSpawner>();
        currentThrowForce = equipment.forceMult;
    }

    // Update is called once per frame
    void Update()
    {
        ChargeForce();
        if (thrown == false)
        {
            transform.position = transform.parent.position;
            transform.localRotation = Quaternion.identity;
            Throw();
        }
        else if (equipment.isSpining == true)
            Spin();

        if (detCountdown <= 0 && equipment.isImpact == false)
        {
            Explode();
        }
        else if (detCountdown > 0 && equipment.isImpact == false && thrown == true)
        {
            detCountdown -= Time.deltaTime; 
        }
    }

    void ChargeForce()
    {
        if (Input.GetMouseButton(0))
            currentThrowForce += chargeInc;
    }

    void Throw()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // adds instantaneous force in the forward direction of camera
            throwLR.enabled = false;
            throwRB.isKinematic = false;
            transform.SetParent(null); // sets object to root
            throwRB.AddForce(transform.forward * currentThrowForce, ForceMode.Impulse);
            thrown = true;
            throwableSpawner.reloadTime = true;
            throwableSpawner.currentAmmo--;
        }
        Trajectory();
    }

    void Spin()
    {
        transform.Rotate(0, equipment.spinSpeed * Time.deltaTime, 0);
    }

    void Explode()
    {
        GameObject exObj = Instantiate(explosion, transform.position, transform.rotation);
        exObj.transform.localScale = new Vector3(explosionRadius.radius, explosionRadius.radius, explosionRadius.radius);
        exObj.GetComponent<Damage>().damageAmount = equipment.damageAmount;
        exObj.GetComponent<Damage>().destroyTime = (int)equipment.detonationCountdown;
        exObj.GetComponent<Damage>().enabled = true;
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
        Vector3 forceVelocity = currentThrowForce * transform.forward;
        
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
        for (int i = 0; i < lineRendPonts.Count; ++i)
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

}
