using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private CharacterSkillController _skillController;
    [SerializeField] private UIManager _manager;
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
            _spawnPos * Random.Range(0, 2f),
            Quaternion.identity
        ); ;

        if (!character.TryGetComponent<ISkillUser>(out var skillUser))
        {
            return;
        }

        _skillController.SetCharacter(character, skillUser);
        _manager.SetCharacter(character);
       //PoolManager.Instance.InitPools(character.PoolData.pools);
    }
}
