using UnityEngine;

public class SwappingSystem : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] WeaponSelection primary;
    [SerializeField] WeaponSelection secondary;
    [SerializeField] WeaponSelection melee;

    [Header("Throwables")]
    [SerializeField] Throwable nonLethal;
    [SerializeField] Throwable lethal;

    [Header("Misc.")]
    [SerializeField] PlayerController playerConScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Swap();
    }

    void Swap()
    {
        if (Input.GetButtonDown("PrimarySwap"))
        {
            Debug.Log("Swapped to primary");
            playerConScript.Gun = primary;
            primary.enabled = true;

            secondary.enabled = false;
            melee.enabled = false;
            nonLethal.enabled = false;
            lethal.enabled = false;

            primary.updateGunUI();
        }
        else if (Input.GetButtonDown("SecondarySwap"))
        {
            Debug.Log("Swapped to secondary");
            playerConScript.Gun = secondary;
            secondary.enabled = true;

            primary.enabled = false;
            melee.enabled = false;
            nonLethal.enabled = false;
            lethal.enabled = false;

            secondary.updateGunUI();
        }
        else if (Input.GetButtonDown("MeleeSwap"))
        {
            Debug.Log("Swapped to melee");
            playerConScript.Gun = melee;
            melee.enabled = true;

            primary.enabled = false;
            secondary.enabled = false;
            nonLethal.enabled = false;
            lethal.enabled = false;

            melee.updateGunUI();
        }
        else if (Input.GetButtonDown("NonLethalSwap"))
        {
            Debug.Log("Swapped to nonLethal");
            playerConScript.Gun = nonLethal;
            nonLethal.enabled = true;

            primary.enabled = false;
            secondary.enabled = false;
            melee.enabled = false;
            lethal.enabled = false;

            nonLethal.updateThrowableUI();
        }
        else if (Input.GetButtonDown("LethalSwap"))
        {
            Debug.Log("Swapped to lethal");
            playerConScript.Gun = lethal;
            lethal.enabled = true;

            primary.enabled = false;
            secondary.enabled = false;
            melee.enabled = false;
            nonLethal.enabled = false;

            lethal.updateThrowableUI();
        }
    }
}
