using UnityEngine;

[CreateAssetMenu(fileName = "LoadoutData", menuName = "Scriptable Objects/LoadoutData")]
public class LoadoutData : ScriptableObject
{
    public Equipment loadoutPrimary;
    public Equipment loadoutSecondary;
    public Equipment loadoutMelee;
    public Equipment loadoutNonLethal;
    public Equipment loadoutLethal;
}
