using NUnit.Framework;
using UnityEngine;

public class ThrowableSpawner : MonoBehaviour
{
    public Equipment equipment;

    public bool reloadTime = false;
    public bool needsReload = false;
    private float reloadTimer;

    public int currentAmmo;
    public int currentHeldAmmo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        updateThrowableUI();

        currentAmmo = equipment.currentMag;
        currentHeldAmmo = equipment.maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (reloadTime == true)
        {
            Reload();
        } 
            
    }

    public void SpawnThrowable(GameObject throwFab)
    {
        if (transform.childCount == 0)
        {
            GameObject duplicate = Instantiate(throwFab);
            duplicate.transform.SetParent(transform);
            throwFab.GetComponent<Throwable>().throwableSpawner = this;
        }
    }

    private void Reload()
    {
        reloadTimer += Time.deltaTime;
        if (reloadTimer > equipment.reloadSpeed)
        {
            reloadTimer = 0;
            if (currentHeldAmmo > equipment.currentMag)
                currentAmmo = equipment.currentMag;
            else
                currentAmmo = currentHeldAmmo;

            currentHeldAmmo = currentHeldAmmo - equipment.currentMag;
            if (currentHeldAmmo < 0)
                currentHeldAmmo = 0;

            reloadTime = false;
            needsReload = true;
        }
    }

    public void SwapOut()
    {
        Destroy(transform.GetChild(0).transform.gameObject);
    }

    public void updateThrowableUI()
    {
        UIManager.instance.SetGun(equipment.weaponImage, "", currentAmmo, currentHeldAmmo);
    }
}
