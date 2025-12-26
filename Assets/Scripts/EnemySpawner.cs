using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private int _countPerWave = 5;
    [SerializeField] private float _spawnRadius = 2f;
    [SerializeField] private float _delayBetweenWaves = 2f;

    private readonly List<Enemy> _aliveEnemies = new();
    private bool _isSpawning;

    private void Start()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        _isSpawning = true;

        yield return new WaitForSeconds(_delayBetweenWaves);

        for (int i = 0; i < _countPerWave; i++)
        {
            Vector2 pos =
                (Vector2)transform.position +
                Random.insideUnitCircle * _spawnRadius;

            Enemy enemy = Instantiate(_enemyPrefab, pos, Quaternion.identity);
            enemy.Init(this);                // 👈 gán spawner ở đây
            _aliveEnemies.Add(enemy);
        }

        _isSpawning = false;
    }

    public void NotifyEnemyDead(Enemy enemy)
    {
        _aliveEnemies.Remove(enemy);

        // Hết enemy → spawn wave mới
        if (_aliveEnemies.Count == 0 && !_isSpawning)
        {
            StartCoroutine(SpawnWave());
        }
    }
}
