using UnityEngine;

[CreateAssetMenu(fileName = "PickupItems", menuName = "Scriptable Objects/PickupItems")]
public class PickupItems : ScriptableObject
{
    public enum abilities
    {
        none,
        poisonDamage,
        fireDamage,
        grenadeBullets,
        laserBullets,
        clownBullets,
        homingBullets
    }

    [Header("Abilities")]
    public abilities abilitiesType;

    [Header("Inc")]
    public int healthInc;
    public int primaryAmmoInc;
    public int secondaryAmmoInc;
    public int nonLethalAmmoInc;
    public int lethalAmmoInc;

    [Header("Audio")]
    public AudioClip pickUpSound;

    [Header("Model")]
    public GameObject model;
}
