using UnityEngine;
using static Equipment;

public class WeaponSelection : MonoBehaviour
{
    [SerializeField] Equipment equipment;

    private int reloadTimer;
    private float fireRateTimer;
    private int currentAmmo;
    private int currentHeldAmmo;
    private int burstCount; // only used when in Burst
    private bool fired; // used for everything but auto
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAmmo = equipment.maxAmmo;
        updateGunUI();
    }

    // Update is called once per frame
    void Update()
    {
        shoot();
    }
    public void shoot()
    {
        if (Input.GetMouseButton(0) && fired == false && currentAmmo > 0 // for any other type
            || burstCount > 0 && currentAmmo > 0 && fired == false)      // for burst type
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * equipment.range, Color.red);
            if(equipment.spreadRange != 0)
            {
                Vector3 dirUp = Camera.main.transform.forward;
                dirUp.y += equipment.spreadRange;
                Debug.DrawRay(Camera.main.transform.position, dirUp * equipment.range, Color.blue);

                Vector3 dirDown = Camera.main.transform.forward;
                dirDown.y -= equipment.spreadRange;
                Debug.DrawRay(Camera.main.transform.position, dirDown * equipment.range, Color.blue);

                Vector3 dirLeft = Camera.main.transform.forward;
                dirLeft.x += equipment.spreadRange;
                Debug.DrawRay(Camera.main.transform.position, dirLeft * equipment.range, Color.blue);

                Vector3 dirRight = Camera.main.transform.forward;
                dirRight.x -= equipment.spreadRange;
                Debug.DrawRay(Camera.main.transform.position, dirRight * equipment.range, Color.blue);

                for (int shotDex = 0; shotDex < equipment.pellets; shotDex++)
                {
                    Vector3 dirRan = Camera.main.transform.forward;
                    dirRan.y += Random.Range(-equipment.spreadRange, equipment.spreadRange);
                    dirRan.x += Random.Range(-equipment.spreadRange, equipment.spreadRange);
                    Debug.DrawRay(Camera.main.transform.position, dirRan * equipment.range, Color.yellow);
                }
            }

            if (equipment.singleFireMode)
            {
                currentAmmo--;
                fired = true;
            }
            else if (equipment.burstFireMode)
            {
                currentAmmo--;
                burstCount++;
                if (burstCount >= equipment.burstAmount)
                {
                    burstCount = 0;
                    fired = true;
                }
            }
            else if(equipment.fullAutoFireMode)
                 currentAmmo--;
        }
        else if (fired == true)
        {
            fireRateTimer += Time.deltaTime;
            if (fireRateTimer >= equipment.fireRate)
            {
                fireRateTimer = 0;
                fired = false;
            }
        }
        else if (currentAmmo <= 0)
        {
            Reload();
        }
        updateGunUI();
    }
    void Reload()
    {
        reloadTimer++;
        if (reloadTimer > equipment.reloadSpeed)
        {
            reloadTimer = 0;
            currentAmmo = equipment.currentMag;
            currentHeldAmmo = equipment.maxAmmo - equipment.currentMag;
        }
    }

    void updateGunUI()
    {
        UIManager.instance.SetGun(equipment.weaponName, currentAmmo, currentHeldAmmo);
    }
}
