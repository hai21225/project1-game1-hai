using System;
using UnityEngine;

public class BaseCharacter: MonoBehaviour
{
    [SerializeField] private PoolData _poolData;
    public PoolData PoolData => _poolData;

    [SerializeField] private CharacterHealth _health;
    [SerializeField] private CharacterMovement _move;

    public event Action OnDead;

    public event Action OnResetState;

    private bool _isDead;


    private void Start()
    {
        _health.OnDead += Dead;
    }

    public bool IsDead {  get { return _isDead; } }


    private void Dead()
    {
        _isDead = true;
        OnDead?.Invoke();
    }

    public void ResetState()
    {
        _health.ResetHealth();
        _move.ResetState();
        _isDead = false;

        OnResetState?.Invoke();
    }

}