using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnService : MonoBehaviour
{
    public static EnemySpawnService Instance;

    [SerializeField] private EnemySpawner[] _spawners;
    private Dictionary<string, EnemySpawner> _spawnerDict;

    private void Awake()
    {
        Instance = this;
        _spawnerDict = new();

        foreach (var spawner in _spawners)
        {
            _spawnerDict.Add(spawner.EnemyName, spawner);
        }
    }

    public EnemyController Spawn(string enemyName, Vector2 pos)
    {
        if (!_spawnerDict.TryGetValue(enemyName, out var spawner))
            return null;

        return spawner.SpawnExtraEnemy(pos);
    }
}
