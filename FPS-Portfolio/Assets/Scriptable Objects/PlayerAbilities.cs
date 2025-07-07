using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAbilities", menuName = "Scriptable Objects/PlayerAbilities")]
public class PlayerAbilities : ScriptableObject
{
    public string abilityName;
    enum AbilityType { movement, damage, information }
    [SerializeField] AbilityType abilities;

    public AudioClip abilitySound;
    public GameObject abilityObject;
}

