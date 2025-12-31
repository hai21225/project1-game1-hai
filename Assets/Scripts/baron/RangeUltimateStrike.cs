using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeUltimateStrike : MonoBehaviour
{
    [SerializeField] private GameObject _lightningPos;
    [SerializeField] private float _tickDelay = 1f;
    [SerializeField] private float _damage = 0.15f;

    private Coroutine _coroutine;
    private List<Enemy> _enemiesInRange = new List<Enemy>(4);
    private List<Enemy> _pendingAdd = new(4);
    private List<Enemy> _pendingRemove = new(4);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            if (!_enemiesInRange.Contains(enemy) && !_pendingAdd.Contains(enemy))
                _pendingAdd.Add(enemy);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            if (!_pendingRemove.Contains(enemy))
                _pendingRemove.Add(enemy);
        }

    }

    private void OnEnable()
    {
        StartChain();
    }

    private void OnDisable()
    {
        StopChain();
    }
    private void StartChain()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);  
        }
        _coroutine = StartCoroutine(ChainLightning());
    }

    private IEnumerator ChainLightning()
    {
        while (true)
        {
            if(_pendingAdd.Count > 0)
            {
                foreach(var  item in _pendingAdd)
                {
                    if (_enemiesInRange.Count >= 4)
                        break;
                    _enemiesInRange.Add(item);
                }
                _pendingAdd.Clear();
            }
            if(_pendingRemove.Count > 0)
            {
                foreach(var item in _pendingRemove)
                {
                    _enemiesInRange.Remove(item);
                }
                _pendingRemove.Clear();
            }

            for (int i = _enemiesInRange.Count - 1; i >= 0; i--)
            {
                Enemy enemy = _enemiesInRange[i];
                if (enemy == null)
                {
                    _enemiesInRange.RemoveAt(i);
                    continue;
                }
                _enemiesInRange[i].TakeDamage(_damage*21f);
                var obj= PoolManager.Instance.Spawn("ChainLightningUlti",_lightningPos.transform.position,Quaternion.identity)
                    .GetComponent<ChainLightningEffect>();
                obj.Init(_lightningPos.transform.position, _enemiesInRange[i].transform.position, "ChainLightningUlti");
                obj.OnSpawn();  
            }

            yield return new WaitForSeconds(_tickDelay);
        }
    }

    private void StopChain()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

}