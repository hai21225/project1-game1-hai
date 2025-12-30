using UnityEngine;


[CreateAssetMenu(menuName ="Skill/Skill Data")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public float cooldown;
    public bool needDirection;
}