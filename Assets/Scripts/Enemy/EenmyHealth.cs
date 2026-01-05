using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Image _hpbar;
    [SerializeField] private float _maxHp = 360f;

    public event Action OnDead;

    private float _hp;
    public void TakeDamage(float damage)
    {
        _hp-=damage;
        if (_hp <= 0)
        {
            Die();
        }
    }

    private void Update()
    {
        float hp= _hp/_maxHp;
        _hpbar.fillAmount = (float)Mathf.Clamp01(hp);
    }

    private void Die()
    {
        OnDead?.Invoke();
    }

    public void OnDespawn()
    {
        OnDead=null;
    }
    public void Onspawn()
    {
        _hp=_maxHp;
    }

}