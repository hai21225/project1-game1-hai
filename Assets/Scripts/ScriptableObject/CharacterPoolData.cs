using UnityEngine;



[CreateAssetMenu(menuName ="Pool/Character Pool Data")]
public class CharacterPoolData: ScriptableObject
{
    public PoolManager.Pool[] pools;
}