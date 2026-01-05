using UnityEngine;

[CreateAssetMenu(menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterId;
    public Sprite portrait;
    public BaseCharacter characterPrefab;
}
