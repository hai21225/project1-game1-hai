using System.Collections;
using UnityEngine;

public class BigZombieAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private float _delay = 1.5f;
    [SerializeField] private float _damage = 36f;
    [SerializeField] private EnemyMovement _move;
    [SerializeField] private EnemyAnimator _animator;
    [SerializeField] private GameObject _hamer;
    [SerializeField] private float _swingTime = 6f; 
    [SerializeField] private float _returnTime = 6f;

    private float _timer = 0f;

    private Coroutine _attackRoutine;
    private Quaternion _defaultRotation;
    private void Update()
    {
        _timer -= Time.deltaTime;
    }


    public void Attack(Transform target)
    {
        if (_timer > 0f) return;
        _timer = _delay;

        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);

        _attackRoutine = StartCoroutine(AttackRotateSmooth(target));
    }

    public void OnSpawn()
    {
        _move.OnAttack += Attack;
        _defaultRotation = _hamer.transform.localRotation;
    }

    public void OnDespawn()
    {

        _move.OnAttack -= Attack;

        if (_attackRoutine != null)
        {
            StopCoroutine(_attackRoutine);
            _attackRoutine = null;
        }

        _hamer.transform.localRotation = _defaultRotation;
        _timer = 0f;
    }

    private IEnumerator AttackRotateSmooth(Transform target)
    {
        Quaternion start = _defaultRotation;
        Quaternion end = Quaternion.Euler(0, 0, -95);


        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime * _swingTime;
            _hamer.transform.localRotation = Quaternion.Slerp(start, end, t);
            yield return null;
        }
        var player = target.GetComponent<CharacterHealth>();
        if (player != null)
        {
            player.TakeDamage(_damage);
        }
        yield return new WaitForSeconds(0.05f);

        t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime * _returnTime;
            _hamer.transform.localRotation = Quaternion.Slerp(end, start, t);
            yield return null;
        }
    }


}