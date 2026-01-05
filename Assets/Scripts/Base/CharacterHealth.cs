using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth: MonoBehaviour {

    [SerializeField] private CharacterStats _stats;
    [SerializeField] private float _health;
    [SerializeField] private Image _healthbar;
    [SerializeField] private Transform _damageTextPos;
    public event Action OnDead;

    private void Start()
    {
        _health=_stats.maxHp;
    }

    private void Update()
    {
        _healthbar.fillAmount=(float)Mathf.Clamp01(_health/_stats.maxHp);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        ShowDamage(damage);
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDead?.Invoke();
    }


    private void ShowDamage(float damage)
    {
        var dmgText = PoolManager.Instance.Spawn
            (PoolGroup.Common, "DamageText", _damageTextPos.position, Quaternion.identity)
            .GetComponent<DamageText>();
        dmgText.SetDamage(damage);
        dmgText.OnSpawn();
    }

}
