using System.Collections;
using UnityEngine;

public class DocAttack : MonoBehaviour, IEnemyAttack
{

    [SerializeField] private EnemyMovement _move;
    [SerializeField] private GameObject _poisonBottle;
    [SerializeField] private float _damage = 21f;
    [SerializeField] private float _delay = 1f;
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
        _timer= _delay;

        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);

        _attackRoutine = StartCoroutine(AttackRotateSmooth(target));
    }

    public void OnDespawn()
    {
        _move.OnAttack -= Attack;

        if (_attackRoutine != null)
        {
            StopCoroutine(_attackRoutine);
            _attackRoutine = null;
        }

        _poisonBottle.transform.localRotation = _defaultRotation;
        _timer = 0f;
    }

    public void OnSpawn()
    {
        _move.OnAttack += Attack;
        _defaultRotation= _poisonBottle.transform.localRotation;
    }

    private IEnumerator AttackRotateSmooth(Transform target)
    {
        Quaternion start = _defaultRotation;
        Quaternion end = Quaternion.Euler(0, 0, -45);


        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime * _swingTime;
            _poisonBottle.transform.localRotation = Quaternion.Slerp(start, end, t);
            yield return null;
        }
        yield return new WaitForSeconds(0.05f);

        t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime * _returnTime;
            _poisonBottle.transform.localRotation = Quaternion.Slerp(end, start, t);
            yield return null;
        }
    }

}
