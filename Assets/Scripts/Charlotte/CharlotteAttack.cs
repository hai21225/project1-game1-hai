using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharlotteAttack: MonoBehaviour,IAttackExecutor
{
    [SerializeField] private CharacterStats _stats;
    [SerializeField] private Image _manaImage;
    [SerializeField] private GameObject _sword;
    [SerializeField] private string _normalAttack;
    [SerializeField] private string _empoweredAttack;
    [SerializeField] private CharacterHealth _health;
    private float _damage;
    private int _currentMana;
    private bool _isEmpoweredAttack;

    private Quaternion _defaultRotation;
    private Coroutine _swingRoutine;



    private void Start()
    {
        _defaultRotation = _sword.transform.localRotation;
        _damage = _stats.maxDamage;
        _currentMana = 0;
        _isEmpoweredAttack = false;

    }
    private void Update()
    {
        if (_currentMana >= _stats.mana)
        {
            _isEmpoweredAttack=true;
        }
        FillManaBar();
    }

    public void ExecuteAttack(Transform target)
    {
        if (_swingRoutine != null)
            StopCoroutine(_swingRoutine);
        _swingRoutine = StartCoroutine(AttackRotateSmooth(target));
        if (_isEmpoweredAttack)
        {
            EmpoweredAttack(target);
            _isEmpoweredAttack=false;
            _currentMana = 0;
            _health.Healing(_stats.healing);
        }
        else
        {
            NormalAttack(target);
            _health.Healing(_stats.healing*0.5f);
        }
    }
    private IEnumerator AttackRotateSmooth(Transform target)
    {
        Quaternion start = _defaultRotation;
        Quaternion end = target.position.x > transform.position.x
            ? Quaternion.Euler(0, 0, -145)
            : Quaternion.Euler(0, 0, 145);

        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime * 15f;
            _sword.transform.localRotation = Quaternion.Slerp(start, end, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime * 15f;
            _sword.transform.localRotation = Quaternion.Slerp(end, start, t);
            yield return null;
        }
    }

    private void NormalAttack(Transform target)
    {
        //var obj = PoolManager.Instance.Spawn(PoolGroup.Character, _normalAttack, target.position,Quaternion.identity).GetComponent<SlashAttack>();
        var obj = GameSession.instance.Spawn(PoolGroup.Character, _normalAttack, target.position, Quaternion.identity).GetComponent<SlashAttack>();
        obj.Init(target,transform);
        obj.OnSpawn();  

        obj.OnHitEnemy += enemy =>
        {
            //Debug.Log("trung enenmy"+ _damage);
            enemy.TakeDamage(_damage);
        };
    }

    private void EmpoweredAttack(Transform target)
    {
        transform.position = target.position;
        //var obj = PoolManager.Instance.Spawn(PoolGroup.Character,_empoweredAttack, target.position, Quaternion.identity)
        //    .GetComponent<SlashAttack>();
        var obj = GameSession.instance.Spawn(PoolGroup.Character, _empoweredAttack, target.position, Quaternion.identity)
    .GetComponent<SlashAttack>();
        obj .Init(target,transform);
        obj.OnSpawn();
        obj.OnHitEnemy += enemy =>
        {
            enemy.TakeDamage(_damage+10);
        };
    }

    private void FillManaBar()
    {
        _manaImage.fillAmount = (float)Mathf.Clamp01((float)_currentMana /(float) _stats.mana);
    }


    //api
    public void SetEmpoweredAttack()
    {
        if(_currentMana<_stats.mana)
        {
            _currentMana++;
        }
    }
}