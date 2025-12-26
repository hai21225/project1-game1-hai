using UnityEngine;
using UnityEngine.UI;

public class BaronBase : MonoBehaviour
{
    [SerializeField] private Image _mana;
    private bool _isStrongAttack =false;
    [SerializeField] private int _amountAttack = 2;
    private int _currentAmountAttack = 0;

    private void Start()
    {
        _isStrongAttack = false;
    }

    private void Update()
    {
        FillManaBar();
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
    public void SetStrongAttack(bool value)
    {
        _isStrongAttack=value;
    }
    public bool IsStrongAttack()
    {
        return _isStrongAttack;
    }

    public void SetAmountAttack()
    {
        if(_currentAmountAttack>= _amountAttack) { return; }
        _currentAmountAttack++;
    }

    public bool GetAmountAttack()
    {
        // if current > amount, reset cooldown skill1 and 2
        return ( _currentAmountAttack >= _amountAttack );
    }
    public void ResetAmountAttack()
    {
        _currentAmountAttack = 0;
    }

}