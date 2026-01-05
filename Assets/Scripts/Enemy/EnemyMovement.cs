using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private EnemyAi _ai;
    [SerializeField] private float _wanderRadius = 3f;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _chaseSpeed = 5f;
    [SerializeField] private float _radiusAttack = 3f;
    [SerializeField] private EnemyAnimator _animator;

    public event System.Action<Transform> OnAttack;

    private EnemyActionState _actionState;
    private Transform _target;
    private Rigidbody2D _rb;
    private Vector2 _wanderTarget;
    private bool _hasWanderTarget;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        switch (_actionState)
        {
            case EnemyActionState.IDLE:
                break;
            case EnemyActionState.WANDER:
                WanderMove();
                break;
            case EnemyActionState.CHASE:
                ChasePlayer(); break;

        }
    }
    private void OnIdle()
    {
        _actionState = EnemyActionState.IDLE;
        _animator.SetRunAnimation(false);
        _target = null;
        _hasWanderTarget = false;
        _rb.linearVelocity = Vector2.zero;
    }
    private void OnWander()
    {
        _actionState = EnemyActionState.WANDER;
        StartWander();
    }
    private void OnChase(Transform target)
    {
        _actionState = EnemyActionState.CHASE;
        _target = target;
        _hasWanderTarget = false;
    }
    private void StartWander()
    {
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        _wanderTarget = (Vector2)transform.position + randomDir * Random.Range(1f, _wanderRadius);
        _hasWanderTarget = true;
    }
    private void WanderMove()
    {
        if (!_hasWanderTarget) { return; }

        Vector2 dir = (_wanderTarget - (Vector2)transform.position);
        _animator.Flip(_wanderTarget.x < transform.position.x ? true : false);
        _animator.SetRunAnimation(true);
        _rb.linearVelocity = dir.normalized * _moveSpeed;
        _hasWanderTarget = false;
    }
    private void ChasePlayer()
    {
        if (_target == null) return;

        _animator.Flip(_target.position.x < transform.position.x ? true : false);
        _animator.SetRunAnimation(true);

        Vector2 dir = _target.position - transform.position;
        if(dir.sqrMagnitude<=_radiusAttack*_radiusAttack)
        {
            OnAttack?.Invoke(_target);
        }
        _rb.linearVelocity = dir.normalized * _chaseSpeed;

    }
    public void OnDespawn()
    {
        _ai.OnIdle -= OnIdle;
        _ai.OnWander -= OnWander;
        _ai.OnChase -= OnChase;
        _target = null;
    }

    public void OnSpawn()
    {
        _actionState = EnemyActionState.IDLE;
        _target = null;
        _hasWanderTarget = false;

        _rb.linearVelocity = Vector2.zero;
        _animator.SetRunAnimation(false);

        _ai.OnIdle += OnIdle;
        _ai.OnWander += OnWander;
        _ai.OnChase += OnChase;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radiusAttack);
    }
}