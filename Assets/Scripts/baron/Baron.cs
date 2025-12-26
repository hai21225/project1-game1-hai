using System.Collections;
using UnityEngine;

public enum UltimateState
{
    None,
    Rising,
    Landing,
    LightningGod
}
public class Baron : MonoBehaviour
{
    [SerializeField] private GameObject _lightningPrefab;
    [SerializeField] private GameObject _ultimateEffect;
    [SerializeField] private GameObject _rangeStrike;
    [SerializeField] private float _dashSpeed = 10f;
    [SerializeField] private float _dashTime = 0.2f;
    [SerializeField] private float _timeRising = 0.2f;
    [SerializeField] private float _ultimateTimeExist = 5f;
    private bool _isDashing;
    private float _ultimateTimeCurrent = 0f;
    private float _rising = 0f;
    private bool _isFreeCooldownFromAttack= false;
 

    
    [SerializeField] private float _skill1Cooldown = 1f;
    [SerializeField] private float _skill2Cooldown = 1f;
    [SerializeField] private float _skill3Cooldown = 10f;

    private float _lastTimeUseSkill1= 0f;
    private float _lastTimeUseSkill2= 0f;
    private float _lastTimeUseSkill3= 0f;


    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Attack _attack;
    private BaronBase _baronBase;
    [SerializeField]private LayerMask _enemyLayer;
    private UltimateState _ultimateState = UltimateState.None;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _attack = GetComponent<Attack>();
        _baronBase = GetComponent<BaronBase>(); 
        _rangeStrike.SetActive(false);
        _ultimateEffect.SetActive(false);
    }
    private void Update()
    {
        if (_baronBase.GetAmountAttack())
        {
            _lastTimeUseSkill1 = 0;
            _lastTimeUseSkill2=0;
            _isFreeCooldownFromAttack = true;
        }
        if (_lastTimeUseSkill1 >= 0f)
        {
            _lastTimeUseSkill1-= Time.deltaTime;
        }
        if( _lastTimeUseSkill2 >= 0f)
        {
            _lastTimeUseSkill2-= Time.deltaTime;
        }
        if ( _lastTimeUseSkill3 >= 0f)
        {
            _lastTimeUseSkill3-= Time.deltaTime;
        }

        UpdateUltimate();
    }

    public void Skill1(Vector2 direction)
    {
        if (_isDashing) return;
        if (_lastTimeUseSkill1>0f) return;
        _baronBase.SetStrongAttack(true);
        if(_isFreeCooldownFromAttack)
        {
            _isFreeCooldownFromAttack = false;
            _baronBase.ResetAmountAttack();
        }
        _lastTimeUseSkill1 = _skill1Cooldown;
        StartCoroutine(Dash(direction.normalized));
    }

    private IEnumerator Dash(Vector2 dir)
    {
        _isDashing = true;
        float timer = 0f;
        if (Mathf.Abs(dir.x) > 0.01f)
            _spriteRenderer.flipX = dir.x < 0f;
        while (timer < _dashTime)
        {
            transform.Translate(dir * _dashSpeed * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        _isDashing = false;
    }

    public void Skill2(Vector2 direction)
    {
        if (_lastTimeUseSkill2 > 0f) return;
        _lastTimeUseSkill2= _skill2Cooldown;
        _baronBase.SetStrongAttack(true);
        if (_isFreeCooldownFromAttack)
        {
            _isFreeCooldownFromAttack = false;
            _baronBase.ResetAmountAttack();
        }
        GameObject lightningPrefab = Instantiate(_lightningPrefab, this.transform.position, Quaternion.identity);
        LightningLogic logic= lightningPrefab.GetComponent<LightningLogic>();
        logic.SetDirection(direction);
    }

    public void Skill3(Vector2 direction)
    {
        if (_lastTimeUseSkill3 > 0f) return;
        _lastTimeUseSkill3 = _skill3Cooldown;
        _ultimateState = UltimateState.Rising;
        _rising = _timeRising;
    }

    private void UpdateUltimate()
    {
        switch (_ultimateState)
        {
            case UltimateState.Rising:
                RisingState();
                break;
            case UltimateState.LightningGod:
                GodState();
                break;
        }
    }

    private void RisingState()
    {
        if (_rising < 0f)
        {
            _ultimateState= UltimateState.LightningGod;
            _rangeStrike.SetActive(false);
            _ultimateEffect.SetActive(false);
            _ultimateTimeCurrent = _ultimateTimeExist;
            return;
        }
        _rangeStrike.SetActive(true);
        _ultimateEffect.SetActive(true);
        transform.position += Vector3.up* Time.deltaTime;
        _rising-= Time.deltaTime;
    }
    private void GodState()
    {

        _animator.SetBool("isUltimateOn", true);
        _attack.SetGodState(true);
        if (_ultimateTimeCurrent > 0f)
        {
            _ultimateTimeCurrent -= Time.deltaTime;
        }
        if (_ultimateTimeCurrent < 0f)
        {
            _ultimateState = UltimateState.None;
            _animator.SetBool("isUltimateOn", false);
            _attack.SetGodState(false);
        }
    }
    public bool CanUseSkill1()
    {
        if (_lastTimeUseSkill1 > 0f) return false;
        return true;
    }
    public bool CanUseSkill2()
    {
        if ( _lastTimeUseSkill2 > 0f) { return false; }
        return true;
    }
    public bool CanUseSkill3()
    {
        if (_lastTimeUseSkill3 > 0f) return false ;
        return true;
    }

    public float TimeRemainingSkill1()
    {
        return TimeRemainPercent(_lastTimeUseSkill1, _skill1Cooldown);
    }
    public float TimeRemainingSkill2()
    {
        return TimeRemainPercent(_lastTimeUseSkill2,_skill2Cooldown);
    }
    public float TimeRemainingSkill3()
    {
        return TimeRemainPercent(_lastTimeUseSkill3,_skill3Cooldown);
    }
    private float TimeRemainPercent(float time, float cooldown)
    {
        return Mathf.Clamp01(1 - time / cooldown);
    }
}