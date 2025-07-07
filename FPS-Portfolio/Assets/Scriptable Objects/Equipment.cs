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
    public enum FireType
    {
        Single,
        Burst,
        FullAuto,
        Single_Burst,
        Single_FullAuto,
        Burst_FullAuto,

    }
    public string weaponName;
    public WeaponType currentWeapon;
    public FireType firingMode;
    public int range;
    public float damageAmount;
    public int reloadSpeed;
    public float fireRate;
    public AudioClip shootSound;
    public GameObject weaponPrefab;
    public int maxAmmo;
    public int currentMag;
}
