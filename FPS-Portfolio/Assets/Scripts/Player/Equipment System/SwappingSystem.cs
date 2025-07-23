using UnityEngine;

public class SwappingSystem : MonoBehaviour
{
    [Header("Weapons")]
    public WeaponSelection primary;
    public WeaponSelection secondary;
    [SerializeField] MeleeLogic melee;

    [Header("Throwable Spawner")]
    public ThrowableSpawner nonLethalSpawner;
    public ThrowableSpawner lethalSpawner;

    [Header("Throwable Prefabs")]
    [SerializeField] GameObject nonLethalFab;
    [SerializeField] GameObject lethalFab;

    [Header("Misc.")]
    [SerializeField] PlayerController playerConScript;
    public GameObject gunModel;

    private bool nonLethalSpawned = false;
    private bool lethalSpawned = false;

    private bool isNonLethal = false;
    private bool isLethal = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerConScript.Gun = primary;
        primary.enabled = true;
        primary.ChangeGun();
        primary.updateGunUI();
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
            isNonLethal = false;
            isLethal = false;
            if (nonLethalSpawner.transform.childCount > 0)
                nonLethalSpawner.SwapOut();
            if (lethalSpawner.transform.childCount > 0)
                lethalSpawner.SwapOut();

            primary.ChangeGun();
            primary.updateGunUI();
        }
        else if (Input.GetButtonDown("SecondarySwap"))
        {
            Debug.Log("Swapped to secondary");
            playerConScript.Gun = secondary;
            secondary.enabled = true;

            primary.enabled = false;
            melee.enabled = false;
            isNonLethal = false;
            isLethal = false;
            if (nonLethalSpawner.transform.childCount > 0)
                nonLethalSpawner.SwapOut();
            if (lethalSpawner.transform.childCount > 0)
                lethalSpawner.SwapOut();

            secondary.ChangeGun();
            secondary.updateGunUI();
        }
        else if (Input.GetButtonDown("MeleeSwap"))
        {
            Debug.Log("Swapped to melee");
            DestroyCurrentGun();
            playerConScript.Gun = null;
            melee.enabled = true;

            primary.enabled = false;
            secondary.enabled = false;
            isNonLethal = false;
            isLethal = false;
            if (nonLethalSpawner.transform.childCount > 0)
                nonLethalSpawner.SwapOut();
            if (lethalSpawner.transform.childCount > 0)
                lethalSpawner.SwapOut();

            melee.updateMeleeUI();
        }
        else if (Input.GetButtonDown("NonLethalSwap")
            || isNonLethal == true && nonLethalSpawner.needsReload == true && nonLethalSpawner.reloadTime == false
            )
        {
            Debug.Log("Swapped to nonLethal");
            DestroyCurrentGun();
            playerConScript.Gun = null;

            primary.enabled = false;
            secondary.enabled = false;
            melee.enabled = false;
            isLethal = false;
            nonLethalSpawner.needsReload = false;
            if (nonLethalSpawner.transform.childCount > 0)
                nonLethalSpawner.SwapOut();
            if (lethalSpawner.transform.childCount > 0)
                lethalSpawner.SwapOut();

            if (nonLethalSpawner.currentAmmo > 0)
            {
                nonLethalSpawner.SpawnThrowable(nonLethalFab);
                nonLethalSpawned = true;
            }

            nonLethalSpawner.updateThrowableUI();
            isNonLethal = true;
        }
        else if (Input.GetButtonDown("LethalSwap") && lethalSpawner.currentAmmo > 0
            || isLethal == true && lethalSpawner.needsReload == true && lethalSpawner.reloadTime == false
            )
        {
            Debug.Log("Swapped to lethal");
            DestroyCurrentGun();
            playerConScript.Gun = null;

            primary.enabled = false;
            secondary.enabled = false;
            melee.enabled = false;
            isNonLethal = false;
            lethalSpawner.needsReload = false;
            if (nonLethalSpawner.transform.childCount > 0)
                nonLethalSpawner.SwapOut();
            if (lethalSpawner.transform.childCount > 0)
                lethalSpawner.SwapOut();

            if (lethalSpawner.currentAmmo > 0)
            {
                lethalSpawner.SpawnThrowable(lethalFab);
                lethalSpawned = true;
            }

            lethalSpawner.updateThrowableUI();
            isLethal = true;
        }
    }
    public void DestroyCurrentGun()
    {
        if (gunModel.transform.childCount > 1)
            Destroy(gunModel.transform.GetChild(1).gameObject);
    }
}
