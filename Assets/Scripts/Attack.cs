using System;
using UnityEngine;
public class Attack : MonoBehaviour
{
    [SerializeField]private AttackButton AttackButton;
    [SerializeField] private float _attackSpeed = 0.5f;
    private float _delay;
    [SerializeField] private GameObject _handAttack;
    [SerializeField] private GameObject _attackIndicator;
    [SerializeField] private GameObject _normalAttackPrefab;
    [SerializeField] private GameObject _strongAttackPrefab;
    [SerializeField] private Transform _attackPosition;
    [SerializeField] private EnemyListDetected _currentEnemy;

    private BaronBase _baronBase;
    private bool _isGod= false;

    private void Start()
    {
        _delay = 0;
        _handAttack.SetActive(false);
        _attackIndicator.SetActive(false);
        _baronBase= GetComponent<BaronBase>();  

        
    }

    private void Update()
    {
        _delay-= Time.deltaTime;
        if(!AttackButton.IsHolding)
        {
            return;
        }
        if (_delay > 0)
        {
            return;
        }
        if (_currentEnemy.Detected() == null)
        {
            _attackIndicator.SetActive(true);
            Invoke(nameof(AttackIndicatorDisable), 0.12f);
            return;
        }
        PerformAttack();
    }

    private void PerformAttack()
    {
        _delay = _attackSpeed;
        _handAttack.SetActive(true);
        //Transform target = _attackRange.currentEnemy.GetComponent<Transform>();
        Transform target = _currentEnemy.Detected().GetComponent<Transform>();
        RotatedToTarget(target);
        Flip(target);
        if (_baronBase.IsStrongAttack())
        {
            PerformStrongAttack(target);
        }
        else
        {
            GameObject normalAttack = Instantiate(_normalAttackPrefab, _attackPosition.position, Quaternion.identity);
            normalAttack.GetComponent<NormalAttack>().Init(target);
            normalAttack.GetComponent<NormalAttack>().SetGodState(_isGod);
            _baronBase.SetAmountAttack();
        }
        Invoke(nameof(Disabale), 0.15f);
    }


    private void RotatedToTarget(Transform target)
    {
        Vector3 dir = target.position - _handAttack.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        _handAttack.transform.rotation = Quaternion.Euler(0, 0, angle);

    }

    private void PerformStrongAttack(Transform target)
    {
        GameObject strongAttack= Instantiate(_strongAttackPrefab,_attackPosition.position, Quaternion.identity);
        strongAttack.GetComponent<StrongAttack>().Init(target);
        _baronBase.SetStrongAttack(false);
    }
    private void Flip(Transform target)
    {
        if(_handAttack.transform.position.x > target.position.x)
        {
            _handAttack.transform.localScale = new Vector3(1, -1, 1);
        }
        else if (_handAttack.transform.position.x < target.position.x)
        {
            _handAttack.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    

    private void Disabale()
    {
        _handAttack.SetActive(false);
        
    }
    public void SetGodState(bool isGod)
    {
        _isGod = isGod;
    }

    private void AttackIndicatorDisable()
    {
        _attackIndicator.SetActive(false);
    }

}
