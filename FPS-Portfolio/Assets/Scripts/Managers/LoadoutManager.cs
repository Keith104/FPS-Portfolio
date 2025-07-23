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
    public Equipment loadoutPrimary;
    public Equipment loadoutSecondary;
    public Equipment loadoutMelee;
    public Equipment loadoutNonLethal;
    public Equipment loadoutLethal;

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
        //string jsonData = PlayerPrefs.GetString("loadoutPrimary");
        //loadoutPrimary = JsonUtility.FromJson<Equipment>(jsonData);
        //
        //if (PlayerPrefs.HasKey("loadoutSecondary"))
        //{
        //    string jsonData = PlayerPrefs.GetString("loadoutSecondary");
        //    loadoutSecondary = JsonUtility.FromJson<Equipment>(jsonData);
        //}

        //if (PlayerPrefs.HasKey("loadoutMelee"))
        //{
        //    string jsonData = PlayerPrefs.GetString("loadoutMelee");
        //    loadoutMelee = JsonUtility.FromJson<Equipment>(jsonData);
        //}

        //if (PlayerPrefs.HasKey("loadoutNonLethal"))
        //{
        //    string jsonData = PlayerPrefs.GetString("loadoutNonLethal");
        //    loadoutNonLethal = JsonUtility.FromJson<Equipment>(jsonData);
        //}

        //if (PlayerPrefs.HasKey("loadoutLethal"))
        //{
        //    string jsonData = PlayerPrefs.GetString("loadoutLethal");
        //    loadoutLethal = JsonUtility.FromJson<Equipment>(jsonData);
        //}
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetPrimarySelection(string newSelection)
    {
        primaryTMPro.text = newSelection;
    }
    public void SetPrimaryEquipment(Equipment newEquipment)
    {
        loadoutPrimary = newEquipment;
        
        //string jsonData = JsonUtility.ToJson(loadoutPrimary);
        //PlayerPrefs.SetString("loadoutPrimary", jsonData);
        //PlayerPrefs.Save();
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetSecondarySelection(string newSelection)
    {
        secondaryTMPro.text = newSelection;
    }
    public void SetSecondaryEquipment(Equipment newEquipment)
    {
        loadoutSecondary = newEquipment;

        //string jsonData = JsonUtility.ToJson(loadoutSecondary);
        //PlayerPrefs.SetString("loadoutSecondary", jsonData);
        //PlayerPrefs.Save();
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetMeleeSelection(string newSelection)
    {
        meleeTMPro.text = newSelection;
    }
    public void SetMeleeEquipment(Equipment newEquipment)
    {
        loadoutMelee = newEquipment;

        //string jsonData = JsonUtility.ToJson(loadoutMelee);
        //PlayerPrefs.SetString("loadoutMelee", jsonData);
        //PlayerPrefs.Save();
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetNonLethalSelection(string newSelection)
    {
        nonLethalTMPro.text = newSelection;
    }
    public void SetNonLethalEquipment(Equipment newEquipment)
    {
        loadoutNonLethal = newEquipment;

        //string jsonData = JsonUtility.ToJson(loadoutNonLethal);
        //PlayerPrefs.SetString("loadoutNonLethal", jsonData);
        //PlayerPrefs.Save();
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void SetLethalSelection(string newSelection)
    {
        lethalTMPro.text = newSelection;
    }
    public void SetLethalEquipment(Equipment newEquipment)
    {
        loadoutLethal = newEquipment;

        //string jsonData = JsonUtility.ToJson(loadoutLethal);
        //PlayerPrefs.SetString("loadoutLethal", jsonData);
        //PlayerPrefs.Save();
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
