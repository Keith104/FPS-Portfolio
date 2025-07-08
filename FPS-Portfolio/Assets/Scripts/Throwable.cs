using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] Rigidbody throwRB;

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
            throwRB.isKinematic = false;
            transform.SetParent(null); // sets object to root
            throwRB.AddForce(transform.forward * forceMult, ForceMode.Impulse);
            thrown = true;
        }
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

    void updateThrowableUI()
    {

    }
}
