using UnityEngine;

public class CharacterMovement: MonoBehaviour
{
    [SerializeField] private CharacterStats _stats;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private CharacterHealth _health;

    private float _horizontal=0f;
    private float _vertical = 0f;
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
    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _isDead = false;

        CameraManager.Instance.SetTarget(transform);
        _health.OnDead += Dead;

    }
    private void FixedUpdate()
    {
        Movement();
    }
    private void Update()
    {
        UpdateAnimation();
        Flip();
    }

    private void Movement()
    {
        if (_isDead) { _rb.linearVelocity = Vector2.zero; return; }

        _horizontal = _joystick.Horizontal;
        _vertical = _joystick.Vertical;

        //_horizontal = (_joystick.Horizontal != 0) ? _joystick.Horizontal : Input.GetAxis("Horizontal");
        //_vertical = (_joystick.Vertical != 0) ? _joystick.Vertical : Input.GetAxis("Vertical");

        var move = new Vector3(_horizontal, _vertical, 0f);
        if (move != Vector3.zero)
        {
            FacingDirection = new Vector2(move.x, move.y).normalized;
        }
        var moveNormalized = move.normalized * _stats.maxSpeed;

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

        //_animator.SetBool("isDead", _isDead);
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

    private void Dead()
    {
        _isDead= true;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        //_rb.linearVelocity= Vector3.zero;
        _rb.bodyType= RigidbodyType2D.Static;
    }

    public void ResetState(Vector3 spawnPos)
    {
        _isDead= false;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        _rb.linearVelocity= Vector3.zero;
        _rb.bodyType = RigidbodyType2D.Dynamic;
        transform.position = spawnPos;
    }
}