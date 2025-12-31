using UnityEngine;

public class SkillTestBootstrap : MonoBehaviour
{
    [SerializeField] private BaseCharacter _characterPrefab;
    [SerializeField] private CharacterSkillController _skillController;
    [SerializeField] private Vector3 _spawnPos = Vector3.zero;

    private void Start()
    {
        BaseCharacter character = Instantiate(
            _characterPrefab,
            _spawnPos,
            Quaternion.identity
        );

        if (!character.TryGetComponent<ISkillUser>(out var skillUser))
        {
            Debug.LogError("Character does not implement ISkillUser");
            return;
        }

        Debug.Log("aaaaaaaaaaa");
        _skillController.SetCharacter(character, skillUser);
        Debug.Log("cacmacmacmamcmac");
    }
}
