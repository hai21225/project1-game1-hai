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
        ShowText(damage,Color.red);
        if (_health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        OnDead?.Invoke();
    }

    

    private void ShowText(float damage,Color color)
    {
        var dmgText = GameSession.instance.Spawn
                (PoolGroup.Common, "DamageText", _damageTextPos.position, Quaternion.identity)
                .GetComponent<DamageText>();
        if (dmgText == null) return;
        dmgText.SetDamage(damage,color);
        dmgText.OnSpawn();
    }

    public void ResetHealth()
    {
        _health = _stats.maxHp;
    }


    public void Healing(float amount)
    {
        if (_health == _stats.maxHp) return;
        ShowText(amount,Color.green);
        _health += amount;
        if(_health>=_stats.maxHp)
        {
            _health = _stats.maxHp;
        }
    }
}
