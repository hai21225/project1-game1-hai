using UnityEngine;
using UnityEngine.UI;

public class BaronAttack : MonoBehaviour, IAttackExecutor
{
    [SerializeField] private Image _mana;
    [SerializeField] private GameObject _handAttack;
    [SerializeField] private GameObject _energyProjectile;
    [SerializeField] private GameObject _energyEmpoweredProjetctile;
    [SerializeField] private Transform _attackPos;
    [SerializeField] private int _amountAttack = 2;

    private int _currentAmountAttack = 0;
    private bool _isEmpowered=false;
    private bool _isGod=false;

    private void Start()
    {
        Disable();
    }
    private void Update()
    {
        FillManaBar();
    }
    public void ExecuteAttack(Transform target)
    {
        _handAttack.SetActive(true);
        RotatedToTarget(target);
        Flip(target);
        if (_isEmpowered)
        {
            EmpoweredAttack(target);
        }
        else
        {
            NormalAttack(target);
        }

        Invoke(nameof(Disable), 0.15f);
    }
    private void RotatedToTarget(Transform target)
    {
        Vector3 dir = target.position - _handAttack.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        _handAttack.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    private void Flip(Transform target)
    {
        if (_handAttack.transform.position.x > target.position.x)
        {
            _handAttack.transform.localScale = new Vector3(1, -1, 1);
        }
        else if (_handAttack.transform.position.x < target.position.x)
        {
            _handAttack.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    private void NormalAttack(Transform target)
    {
        var attack= Instantiate(_energyProjectile,_attackPos.position,Quaternion.identity);
        var aattack = attack.GetComponent<NormalAttack>();
        aattack.Init(target);
        aattack.SetGodState(_isGod);
        SetAmountAttack();
    }
    private void EmpoweredAttack(Transform target)
    {
        var attack = Instantiate(_energyEmpoweredProjetctile, _attackPos.position, Quaternion.identity);
        var aattack = attack.GetComponent<EmpoweredAttack>();
        aattack.Init(target);
        _isEmpowered=false;
    }
    private void SetAmountAttack()
    {
        if (_currentAmountAttack >= _amountAttack) { return; }
        _currentAmountAttack++;
    }
    private void Disable()
    {
        _handAttack.SetActive(false);
    }
    private void FillManaBar()
    {
        if (_mana != null)
        {
            float value = (float)Mathf.Clamp01((float)_currentAmountAttack / (float)_amountAttack);

            //Debug.Log("checkkkk mana value: "+value);
            _mana.fillAmount = value;

        }
    }
    //API
    public void SetGodState(bool value)
    {
        _isGod = value;
    }
    public void SetEmpoweredAttack(bool value)
    {
        _isEmpowered = value;
    }
    public bool GetAmountAttack()
    {
        return _currentAmountAttack >= _amountAttack;
    }
    public void ResetAmountAttack()
    {
        _currentAmountAttack = 0;
    }
}