using UnityEngine;

public class FireModeSwitcher : MonoBehaviour
{
    [SerializeField] Equipment gun;

    [Header("Fire Modes")]
    [Tooltip("Controls what you can swap to")]
    [SerializeField] bool single;
    [Tooltip("Controls what you can swap to")]
    [SerializeField] bool burst;
    [Tooltip("Controls what you can swap to")]
    [SerializeField] bool fullAuto;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GunSwap();
    }

    void GunSwap()
    {
        if (Input.GetButtonDown("FireModeSwap"))
        {
            if (fullAuto == false && gun.burstFireMode == true && single == true
                || gun.fullAutoFireMode == true && single == true
                )
            {
                Debug.Log("Fire Mode Swapped (single)");
                gun.singleFireMode = true;
                gun.burstFireMode = false;
                gun.fullAutoFireMode = false;
            }
            else if (single == false && gun.fullAutoFireMode == true && burst == true
                || gun.singleFireMode == true && burst == true
                )
            {
                Debug.Log("Fire Mode Swapped (burst)");
                gun.singleFireMode = false;
                gun.burstFireMode = true;
                gun.fullAutoFireMode = false;
            }
            else if (burst == false && gun.singleFireMode == true && fullAuto == true
                || gun.burstFireMode == true && fullAuto == true
                )
            {
                Debug.Log("Fire Mode Swapped (full auto)");
                gun.singleFireMode = false;
                gun.burstFireMode = false;
                gun.fullAutoFireMode = true;
            }
        }
    }
}
