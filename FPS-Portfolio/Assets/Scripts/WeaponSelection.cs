using UnityEngine;

public class WeaponSelection : MonoBehaviour
{
    [SerializeField] int range;
    [SerializeField] int damage;
    [SerializeField] int reloadSpeed;
    [SerializeField] int reloadTimer;

    [SerializeField] int fireRate;
    [SerializeField] float fireRateTimer;

    [SerializeField] int ammo;
    [SerializeField] int currentAmmo;

    public enum FireType
    {
        Single,
        Burst,
        Auto
    }
    public FireType currentFire;
    [SerializeField] int burstAmount; // only used when in Burst
    [SerializeField] int burstCount; // only used when in Burst
    [SerializeField] bool fired; // used for everything but auto

    [SerializeField] float spreadRange; // put 0 for no spread, I suggest not going above 1
    [SerializeField] int pellets; // amount of bullets per shot, only used when spreadRange is in effect
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAmmo = ammo;
    }

    // Update is called once per frame
    void Update()
    {
        shoot();
    }
    void shoot()
    {
        if (Input.GetMouseButton(0) && fired == false && currentAmmo > 0 || burstCount > 0)
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * range, Color.red);
            if(spreadRange != 0)
            {
                Vector3 dirUp = Camera.main.transform.forward;
                dirUp.y += spreadRange;
                Debug.DrawRay(Camera.main.transform.position, dirUp * range, Color.blue);

                Vector3 dirDown = Camera.main.transform.forward;
                dirDown.y -= spreadRange;
                Debug.DrawRay(Camera.main.transform.position, dirDown * range, Color.blue);

                Vector3 dirLeft = Camera.main.transform.forward;
                dirLeft.x += spreadRange;
                Debug.DrawRay(Camera.main.transform.position, dirLeft * range, Color.blue);

                Vector3 dirRight = Camera.main.transform.forward;
                dirRight.x -= spreadRange;
                Debug.DrawRay(Camera.main.transform.position, dirRight * range, Color.blue);

                for (int shotDex = 0; shotDex < pellets; shotDex++)
                {
                    Vector3 dirRan = Camera.main.transform.forward;
                    dirRan.y += Random.Range(-spreadRange, spreadRange);
                    dirRan.x += Random.Range(-spreadRange, spreadRange);
                    Debug.DrawRay(Camera.main.transform.position, dirRan * range, Color.yellow);
                }
            }

            switch (currentFire)
            {
                case FireType.Single:
                    currentAmmo--;
                    fired = true;
                    break;

                case FireType.Burst:
                    currentAmmo -= burstAmount;
                    burstCount++;
                    if (burstCount >= burstAmount)
                    {
                        burstCount = 0;
                        fired = true;
                    }
                    break;

                case FireType.Auto:
                    currentAmmo--;
                    break;
            }
        }
        else if (fired == true)
        {
            fireRateTimer += Time.deltaTime;
            if (fireRateTimer >= fireRate)
            {
                fireRateTimer = 0;
                fired = false;
            }
        }
        else if (currentAmmo <= 0)
        {
            Reload();
        }
    }
    void Reload()
    {
        reloadTimer++;
        if (reloadTimer > reloadSpeed)
        {
            reloadTimer = 0;
            currentAmmo = ammo;
        }
    }
}
