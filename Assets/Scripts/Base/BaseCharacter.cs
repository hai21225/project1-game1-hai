using UnityEngine;

public class BaseCharacter: MonoBehaviour
{
    [SerializeField] private CharacterStats _stats;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private GameObject _damageText;
    [SerializeField] private Transform _damageTextPos;

    private float _currentHp=0f;
    private float _currentSpeed=0f;
    private float _horizontal=0f;
    private float _vertical = 0f;
    private float _timeSlow=0f;
    private bool _isDead =false;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    public Vector2 FacingDirection {  get; private set; } 


    private void Awake()
    {
        if(_joystick == null)
        {
            _joystick= FindAnyObjectByType<Joystick>();
        }

        _rb= GetComponent<Rigidbody2D>();
        _spriteRenderer= GetComponent<SpriteRenderer>();
        _animator= GetComponent<Animator>();
    }
    private void Start()
    {
        _currentHp = _stats.maxHp;
        _currentSpeed= _stats.maxSpeed;
        _isDead = false;
       
    }
    private void FixedUpdate()
    {
        Movement();
    }
    private void Update()
    {
        UpdateAnimation();
        Flip();
        ResetSpeed();
    }

    private void Movement()
    {
        if(_isDead) return;

        //_horizontal = _joystick.Horizontal;
        //_vertical = _joystick.Vertical;

        _horizontal = (_joystick.Horizontal != 0) ? _joystick.Horizontal : Input.GetAxis("Horizontal");
        _vertical = (_joystick.Vertical != 0) ? _joystick.Vertical : Input.GetAxis("Vertical");

        var move = new Vector3(_horizontal, _vertical, 0f);
        if (move != Vector3.zero)
        {
            FacingDirection = new Vector2(move.x, move.y).normalized;
        }
        var moveNormalized = move.normalized * _currentSpeed;

        _rb.linearVelocity = new Vector3(moveNormalized.x, moveNormalized.y, 0f);

    }
    private void UpdateAnimation()
    {
        if (_horizontal != 0 || _vertical != 0)
        {
            _animator.SetBool("isRunning", true);
        }
        else if(_horizontal==0 && _vertical == 0){
            _animator.SetBool("isRunning", false);
        }

        _animator.SetBool("isDead", _isDead);
    }
    private void Flip()
    {
        if (_horizontal > 0f)
        {
            _spriteRenderer.flipX= false;
        }
        else if(_horizontal < 0f)
        {
            _spriteRenderer.flipX= true;
        }
    }
    private void Die()
    {
        _isDead = true;
        Debug.Log("nguuu");
    }
    private void ResetHp()
    {
        
    }
    private void ResetSpeed()
    {
        if(_currentSpeed<_stats.maxSpeed)
        {
            _timeSlow -= Time.deltaTime;
            if(_timeSlow < 0f)
            {
                _timeSlow = 0f;
                _currentSpeed= _stats.maxSpeed;
            }
        }
    }
    private void ShowDamage(float damage)
    {
        GameObject dmgText = Instantiate(
            _damageText,
            _damageTextPos.position,
            Quaternion.identity
        );
        dmgText.GetComponent<DamageText>().SetDamage(damage);
    }

    //API//
    public void TakeDamage(float damage)
    {
        _currentHp -= damage;
        ShowDamage(damage);
        if (_currentHp <= 0f)
        {
            Die();
        }
    }
    public void Slow(float amountSpeedPercent, float timeSlow)
    {
        _currentSpeed =_stats.maxSpeed* amountSpeedPercent;
        _timeSlow = timeSlow;
    }
    public void Knockback(Vector2 dir)
    {

    }
    
}