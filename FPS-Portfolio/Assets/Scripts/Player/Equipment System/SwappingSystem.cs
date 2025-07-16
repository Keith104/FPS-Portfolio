using UnityEngine;

public class SwappingSystem : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] WeaponSelection primary;
    [SerializeField] WeaponSelection secondary;
    [SerializeField] MeleeLogic melee;

    [Header("Throwable Spawner")]
    public ThrowableSpawner nonLethalSpawner;
    public ThrowableSpawner lethalSpawner;

    [Header("Throwable Prefabs")]
    [SerializeField] GameObject nonLethalFab;
    [SerializeField] GameObject lethalFab;

    [Header("Misc.")]
    [SerializeField] PlayerController playerConScript;

    private bool nonLethalSpawned = false;
    private bool lethalSpawned = false;
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
            if(nonLethalSpawner.transform.childCount > 0)
                nonLethalSpawner.SwapOut();
            if (lethalSpawner.transform.childCount > 0)
                lethalSpawner.SwapOut();

            primary.updateGunUI();
        }
        else if (Input.GetButtonDown("SecondarySwap"))
        {
            Debug.Log("Swapped to secondary");
            playerConScript.Gun = secondary;
            secondary.enabled = true;

            primary.enabled = false;
            melee.enabled = false;
            if (nonLethalSpawner.transform.childCount > 0)
                nonLethalSpawner.SwapOut();
            if (lethalSpawner.transform.childCount > 0)
                lethalSpawner.SwapOut();

            secondary.updateGunUI();
        }
        else if (Input.GetButtonDown("MeleeSwap"))
        {
            Debug.Log("Swapped to melee");
            playerConScript.Gun = null;
            melee.enabled = true;

            primary.enabled = false;
            secondary.enabled = false;
            if (nonLethalSpawner.transform.childCount > 0)
                nonLethalSpawner.SwapOut();
            if (lethalSpawner.transform.childCount > 0)
                lethalSpawner.SwapOut();

            melee.updateMeleeUI();
        }
        else if (Input.GetButtonDown("NonLethalSwap") && nonLethalSpawner.currentAmmo > 0)
        {
            Debug.Log("Swapped to nonLethal");
            playerConScript.Gun = null;

            primary.enabled = false;
            secondary.enabled = false;
            melee.enabled = false;

            nonLethalSpawner.SpawnThrowable(nonLethalFab);
            nonLethalSpawner.updateThrowableUI();
            nonLethalSpawned = true;
        }
        else if (Input.GetButtonDown("LethalSwap") && lethalSpawner.currentAmmo > 0)
        {
            Debug.Log("Swapped to lethal");
            playerConScript.Gun = null;

            primary.enabled = false;
            secondary.enabled = false;
            melee.enabled = false;

            lethalSpawner.SpawnThrowable(nonLethalFab);
            lethalSpawner.updateThrowableUI();
            lethalSpawned = true;
        }
    }
}
