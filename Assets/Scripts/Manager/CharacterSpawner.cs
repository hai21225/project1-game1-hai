using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private CharacterSkillController _skillController;
    [SerializeField] private Vector3 _spawnPos = Vector3.zero;

    private void Start()
    {
        var prefab = GameManager.Instance.SelectedCharacter;


        if (prefab == null)
        {
            Debug.LogError("No character prefab!");
            return;
        }

        BaseCharacter character = Instantiate(
            prefab,
            _spawnPos,
            Quaternion.identity
        );

        if (!character.TryGetComponent<ISkillUser>(out var skillUser))
        {
            return;
        }

        _skillController.SetCharacter(character, skillUser);

       //PoolManager.Instance.InitPools(character.PoolData.pools);
    }
}
