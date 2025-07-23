using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class LoadoutManager : MonoBehaviour
{
    public static LoadoutManager instance;
    public TMP_Dropdown primaryDropdown;
    public TMP_Dropdown secondaryDropdown;
    public TMP_Dropdown meleeDropdown;
    public TMP_Dropdown nonLethalDropdown;
    public TMP_Dropdown lethalDropdown;

    void Awake() => instance = this;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.parent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
