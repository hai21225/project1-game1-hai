using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeUltimateStrike : MonoBehaviour
{
    [SerializeField] private GameObject _strikePrefab;
    [SerializeField] private GameObject _chainLightning;
    [SerializeField] private GameObject _lightningPos;
    [SerializeField] private float _tickDelay = 1f;
    [SerializeField] private float _damage = 0.15f;

    private Coroutine _coroutine;
    private List<Enemy> _enemiesInRange = new();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            if (!_enemiesInRange.Contains(enemy))
                _enemiesInRange.Add(enemy);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            _enemiesInRange.Remove(enemy);
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
            for (int i = _enemiesInRange.Count - 1; i >= 0; i--)
            {
                if (_enemiesInRange[i] == null)
                {
                    _enemiesInRange.RemoveAt(i);
                    continue;
                }

                _enemiesInRange[i].TakeDamage(_damage);
                SpawnLightning(
                    _lightningPos.transform.position,
                    _enemiesInRange[i].transform.position
                );
            }

            yield return new WaitForSeconds(_tickDelay);
        }
    }

    private void SpawnLightning(Vector3 from, Vector3 to)
    {
        GameObject go = Instantiate(_chainLightning);
        LineRenderer lr = go.GetComponent<LineRenderer>();
        lr.material.color = Color.yellow;
        int segments = 8;            
        float offsetAmount = 0.3f;   

        lr.positionCount = segments + 1;
        lr.startColor = Color.cyan;
        lr.endColor = Color.white;


        Vector3 dir = to - from;
        Vector3 normal = Vector3.Cross(dir.normalized, Vector3.forward);

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 pos = Vector3.Lerp(from, to, t);

            if (i != 0 && i != segments)
            {
                float offset = Random.Range(-offsetAmount, offsetAmount);
                pos += normal * offset;
            }

            lr.SetPosition(i, pos);
        }

        Destroy(go, 0.3f); 
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