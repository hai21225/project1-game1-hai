using UnityEngine;

[CreateAssetMenu(menuName ="Character/Stats")]

public class CharacterStats : ScriptableObject
{
    [Header("Base stats")]
    public float maxHp = 36f;
    public float maxSpeed = 36f;
    public int mana = 36;

    [Header("Combat")]
    public float maxDamage = 36f;
    public float maxAttackSpeed = 36f;
    public float rangeAttack = 36f;

    public float damageSkill1 = 16f;
    public float damageSkill2 = 5f;
    public float damageSkill3 = 40f;

    public float healing = 10f;

    public float GetDamageSkil(int index)
    {
        switch (index)
        {
            case 0:
                return damageSkill1;
                
            case 1:
                return damageSkill2;
            case 2:
                return damageSkill3;
            default: return 0f;
        }
    }
}