using UnityEngine;

public class ChortAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _delay = 1f;
    [SerializeField] private EnemyMovement _move;

    private float _timer;

    private void Update()
    {
        _timer -= Time.deltaTime;
    }

    public void Attack(Transform target)
    {
        if (_timer > 0f) return;

        _timer = _delay;
        var player = target.GetComponent<CharacterHealth>();
        player.TakeDamage(_damage);
    }

    public void OnDespawn()
    {
        _move.OnAttack -= Attack;
    }

    public void OnSpawn()
    {
        _move.OnAttack += Attack;
    }
}