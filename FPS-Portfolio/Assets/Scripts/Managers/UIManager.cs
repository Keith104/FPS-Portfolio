using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Image playerHPBar;
    public GameObject playerDamagePanel;
    public GameObject playerHealPanel;

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
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetGun(string name, int ammoLeft, int totalAmmo)
    {
        playerGunText.text = name + "\n" + ammoLeft + "/" + totalAmmo;
    }
}
