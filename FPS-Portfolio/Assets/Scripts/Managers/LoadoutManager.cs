using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class LoadoutManager : MonoBehaviour
{
    public static LoadoutManager instance;

    [Header("TMPro Text")]
    public TextMeshProUGUI primaryTMPro;
    public TextMeshProUGUI secondaryTMPro;
    public TextMeshProUGUI meleeTMPro;
    public TextMeshProUGUI nonLethalTMPro;
    public TextMeshProUGUI lethalTMPro;

    [Header("Loadout Equipmemt")]
    public LoadoutData LoadoutData;

    void Awake() => instance = this;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadEquipment();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadEquipment()
    {
        primaryTMPro.text = LoadoutData.loadoutPrimary.weaponName;
        secondaryTMPro.text = LoadoutData.loadoutSecondary.weaponName;
        meleeTMPro.text = LoadoutData.loadoutMelee.weaponName;
        nonLethalTMPro.text = LoadoutData.loadoutNonLethal.weaponName;
        lethalTMPro.text = LoadoutData.loadoutLethal.weaponName;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetPrimaryEquipment(Equipment newEquipment)
    {
        LoadoutData.loadoutPrimary = newEquipment;
        primaryTMPro.text = newEquipment.weaponName;
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetSecondaryEquipment(Equipment newEquipment)
    {
        LoadoutData.loadoutSecondary = newEquipment;
        secondaryTMPro.text = newEquipment.weaponName;
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetMeleeEquipment(Equipment newEquipment)
    {
        LoadoutData.loadoutMelee = newEquipment;
        meleeTMPro.text = newEquipment.weaponName;
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetNonLethalEquipment(Equipment newEquipment)
    {
        LoadoutData.loadoutNonLethal = newEquipment;
        nonLethalTMPro.text = newEquipment.weaponName;
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetLethalEquipment(Equipment newEquipment)
    {
        LoadoutData.loadoutLethal = newEquipment;
        lethalTMPro.text = newEquipment.weaponName;
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
