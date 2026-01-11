using System.Collections;
using UnityEngine;

public class NecromancerAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private string _enemyNameSpawn = "Chort";
    [SerializeField] private string _nameObjectPool = "ComonProjectile";
    [SerializeField] private float _damage = 21f;
    [SerializeField] private float _delay = 1f;
    [SerializeField] private float _cooldown = 5f;
    [SerializeField] private Transform _pos;
    [SerializeField] private EnemyMovement _move;
    [SerializeField] private GameObject _staff;
    [SerializeField] private Transform _posProjecttile;
    [SerializeField] private float _swingTime = 6f;
    [SerializeField] private float _returnTime = 6f;

    private float _timer = 0f;
    private float _spawntimer = 0f;
    private Coroutine _attackRoutine;
    private Quaternion _defaultRotation;
    private bool _isSpawn= true;
    private int count = 0;
    private void Update()
    {
        _timer-=Time.deltaTime;
        _spawntimer -= Time.deltaTime;
        SpawnEnemy();
    }


    public void Attack(Transform target)
    {
        if (_timer > 0f) return;
        _timer = _delay;
        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);

        _attackRoutine = StartCoroutine(AttackRotateSmooth(target));
    }

    public void OnDespawn()
    {
        _move.OnAttack-=Attack;

        if (_attackRoutine != null)
        {
            StopCoroutine(_attackRoutine);
            _attackRoutine = null;
        }

        _staff.transform.localRotation = _defaultRotation;
        _timer = 0f;
        count = 0;
        _isSpawn = true;
    }

    public void OnSpawn()
    {
        _move.OnAttack += Attack;
        _defaultRotation = _staff.transform.localRotation;
    }

    private void SpawnEnemy()
    {
        if(_spawntimer > 0f || !_isSpawn) { return; }


        _spawntimer=_cooldown;
        EnemySpawnService.Instance.Spawn(_enemyNameSpawn,_pos.position);
        count++;
        if (count == 2)
        {
            _isSpawn = false;
        }
    }


    private IEnumerator AttackRotateSmooth(Transform target)
    {
        Quaternion start = _defaultRotation;
        Quaternion end = Quaternion.Euler(0, 0, -45);

        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime * _swingTime;
            _staff.transform.localRotation = Quaternion.Slerp(start, end, t);
            yield return null;
        }
        yield return new WaitForSeconds(0.05f);
        var go =GameSession.instance.Spawn(PoolGroup.Common, _nameObjectPool, _posProjecttile.position, Quaternion.identity)
            .GetComponent<ComonLinearPrjtile>();
        if (go == null) yield break;
        go.SetDirection((target.position-_posProjecttile.position).normalized);
        go.OnPlayer += (player) =>
        {
            player.TakeDamage(_damage);
        };

        t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime * _returnTime;
            _staff.transform.localRotation = Quaternion.Slerp(end, start, t);
            yield return null;
        }
    }
}
