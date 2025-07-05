using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Image playerHPBar;

    public TextMeshProUGUI playerGunText;
    [SerializeField] WeaponSelection gun;
    // should be formated like the following:
    // Gun Name
    // ~/~
    //
    // Example:
    // Ak-47
    // 30/30

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetHealth();

        SetGun();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void SetHealth()
    {
        // add stuff from the playerController here
    }
    public void SetGun()
    {
        playerGunText.text = gun.gunName + "\n" + gun.currentAmmo + "/" + gun.ammo;
    }
}
