using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private string _enemyPoolName;
    public string EnemyName => _enemyPoolName;
    [SerializeField] private int _countPerWave = 5;
    [SerializeField] private float _spawnRadius = 2f;
    [SerializeField] private float _delayBetweenWaves = 2f;

    [SerializeField] private int _startCount = 3;
    [SerializeField] private int _maxCount = 20;
    [SerializeField] private float _growthRate = 1.3f;

    private int _currentWaveCount;
    private readonly List<EnemyController> _aliveEnemies = new();
    private bool _isSpawning;

    private void Start()
    {
        _currentWaveCount= _startCount;
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        _currentWaveCount = Mathf.Min(
    Mathf.CeilToInt(_currentWaveCount * _growthRate),
    _maxCount
);

        _countPerWave = _currentWaveCount;

        _isSpawning = true;

        yield return new WaitForSeconds(_delayBetweenWaves);

        for (int i = 0; i < _countPerWave; i++)
        {
            Vector2 pos =
                (Vector2)transform.position +
                Random.insideUnitCircle * _spawnRadius;
            var obj = PoolManager.Instance.Spawn(PoolGroup.Common,_enemyPoolName,pos,Quaternion.identity);

            if (obj == null) break;

            var enemy = obj.GetComponent<EnemyController>();
            enemy.Init(this);
            enemy.OnSpawn();
            _aliveEnemies.Add(enemy);
        }

        _isSpawning = false;
    }

    public void NotifyEnemyDead(EnemyController enemy)
    {
        _aliveEnemies.Remove(enemy);
        //Debug.Log("checkkkk :   "+_aliveEnemies.Count);
        if (_aliveEnemies.Count == 0 && !_isSpawning)
        {
            StartCoroutine(SpawnWave());
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }


    //api for necromancer spawn enemy
    public EnemyController SpawnExtraEnemy(Vector2 pos)
    {
        var obj = PoolManager.Instance.Spawn(
            PoolGroup.Common,
            _enemyPoolName,
            pos,
            Quaternion.identity
        );

        if (obj == null) return null;

        var enemy = obj.GetComponent<EnemyController>();
        enemy.Init(this);
        enemy.OnSpawn();

        _aliveEnemies.Add(enemy);
        return enemy;
    }



    public void ResetSpawner()
    {
        StopAllCoroutines();
        _isSpawning = false;

        for (int i = _aliveEnemies.Count - 1; i >= 0; i--)
        {
            if (_aliveEnemies[i] != null)
            {
                _aliveEnemies[i].ForceReturnToPool();
            }
        }
        _currentWaveCount = _startCount;

        _aliveEnemies.Clear();

        StartCoroutine(SpawnWave());
    }

}
