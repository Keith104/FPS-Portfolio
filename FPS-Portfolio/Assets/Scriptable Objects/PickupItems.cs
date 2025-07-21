using UnityEngine;

[CreateAssetMenu(fileName = "PickupItems", menuName = "Scriptable Objects/PickupItems")]
public class PickupItems : ScriptableObject
{
    public int healthInc;

    public int primaryAmmoInc;
    public int secondaryAmmoInc;
    public int nonLethalAmmoInc;
    public int lethalAmmoInc;

    public AudioClip pickUpSound;
}
