using UnityEngine;

public class BigDemonAttack: MonoBehaviour,IEnemyAttack{

    [SerializeField] private float _delay = 1.5f;
    [SerializeField] private float _damage = 36f;
    [SerializeField] private EnemyMovement _move;
    [SerializeField] private EnemyAnimator _animator;

    private float _timer = 0f;
    private Transform _currentTarget;
    private void Update()
    {
        _timer-=Time.deltaTime;
    }

    public void OnSpawn()
    {
        _timer = 0f;
        _move.OnAttack += Attack;
    }

    public void OnDespawn()
    {
        _move.OnAttack-= Attack;
        _currentTarget = null;
    }

    public void Attack(Transform target)
    {
        if (_timer > 0f) return;
        _timer = _delay;
        _currentTarget = target;
        _animator.SetAttackAnimation();
    }

    public void DealDamage()
    {
        if (_currentTarget == null) return;

        if (_currentTarget.TryGetComponent(out CharacterHealth player))
        {
            player.TakeDamage(_damage);
        }
    }
}