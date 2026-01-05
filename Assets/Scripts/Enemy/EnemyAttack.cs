using UnityEngine;

public class EnemyAttack: MonoBehaviour{

    [SerializeField] private float _delay = 1.5f;
    [SerializeField] private float _damage = 36f;
    [SerializeField] private EnemyMovement _move;
    [SerializeField] private EnemyAnimator _animator;

    private float _timer = 0f;
    private void Attack(Transform target)
    {
        if (_timer > 0f) return;
        _timer = _delay;
        var player= target.GetComponent<CharacterHealth>();
        if (player == null) return;
        player.TakeDamage(_damage);
        _animator.SetAttackAnimation();
    }

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
    }

}