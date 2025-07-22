using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Image playerHPBar;
    public GameObject playerDamagePanel;
    public GameObject playerHealPanel;
    public GameObject playerStunPanel;

    public TextMeshProUGUI playerGunText;
    public Image playerGunSprite;
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
    public void SetGun(Sprite sprite, int ammoLeft, int totalAmmo)
    {
        playerGunSprite.sprite = sprite;
        Debug.Log($"Current Sprite: {playerGunSprite.sprite} needed sprite {sprite}");
        playerGunText.text = ammoLeft + "/" + totalAmmo;
    }
}
