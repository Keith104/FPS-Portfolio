using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Scriptable Objects/Equipment")]
public class Equipment : ScriptableObject
{
    public enum WeaponType
    {
        firearm,
        meleeWeapon,
        throwable
    }
    
    public string weaponName;
    public WeaponType currentWeapon;
    public bool singleFireMode;
    public bool burstFireMode;
    public bool fullAutoFireMode;
    public int range;
    public int damageAmount;

    public int burstAmount;
    public float spreadRange; // put 0 for no spread, I suggest not going above 1
    public int pellets; // amount of bullets per shot, only used when spreadRange is in effect

    public int reloadSpeed;
    public float fireRate;
    public int maxAmmo;
    public int currentMag;

    public AudioClip shootSound;
    public GameObject weaponPrefab;
    public Sprite weaponImage;

    [Header("Throwable Data")]
    public bool isSpining;
    public int spinSpeed;
    public bool isSticky;
    public bool isImpact;
    public int forceMult;
    public float detonationCountdown;

}
