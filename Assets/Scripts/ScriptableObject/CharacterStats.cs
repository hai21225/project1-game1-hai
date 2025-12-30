using UnityEngine;

[CreateAssetMenu(menuName ="Character/Stats")]

public class CharacterStats : ScriptableObject
{
    [Header("Base stats")]
    public float maxHp = 36f;
    public float maxSpeed = 36f;
    public float mana = 36f;

    [Header("Combat")]
    public float maxDamage = 36f;
    public float maxAttackSpeed = 36f;
    public float rangeAttack = 36f;
}