using UnityEngine;

public class CharacterSpawner : MonoBehaviour
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
            return;
        }

        _skillController.SetCharacter(character, skillUser);
        if (character.PoolData == null)
        {
            Debug.Log("null roif do con cho ngu");
        }

       PoolManager.Instance.InitPools(character.PoolData.pools);
    }
}
