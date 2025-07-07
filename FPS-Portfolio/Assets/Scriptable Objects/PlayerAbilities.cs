using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAbilities", menuName = "Scriptable Objects/PlayerAbilities")]
public class PlayerAbilities : ScriptableObject
{
    public string AbilityName;
    enum AbilityType { movement, damage, information }
    [SerializeField] AbilityType type;

    public AudioClip AbilitySound;
    public GameObject AbilityObject;
}

