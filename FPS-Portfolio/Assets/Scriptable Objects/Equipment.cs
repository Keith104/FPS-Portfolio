using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Scriptable Objects/Equipment")]
public class Equipment : ScriptableObject
{
    public enum WeaponType
    {
        Firearm,
        MeleeWeapon,
        Throwable
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
    public string WeaponName;
    public WeaponType CurrentWeapon;
    public FireType FiringMode;
    public int Range;
    public float DamageAmount;
    public int ReloadSpeed;
    public float RireRate;
    public AudioClip ShootSound;
    public GameObject WeaponPrefab;
    public int MaxAmmo;
    public int CurrentMag;
}
