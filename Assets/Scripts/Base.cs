using UnityEngine;

public class Base : MonoBehaviour
{
    private float _horizontal, _vertical;


    //[SerializeField] private float _hp = 100f;
    [SerializeField] private float _speed = 10f;


    [SerializeField] private Joystick _joystick;
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _sprite;
    [SerializeField] private GameObject _indicatorSkill1;
    
    public Vector2 FacingDirection { get; private set; }


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _indicatorSkill1.SetActive(false);
    }

    private void FixedUpdate()
    {
        Movement();
    }
    private void Update()
    {
        FlipSprite();
        SetAnimation();
    }

    private void Movement()
    {
        _horizontal = (_joystick.Horizontal != 0) ? _joystick.Horizontal : Input.GetAxis("Horizontal");
        _vertical = (_joystick.Vertical != 0) ? _joystick.Vertical : Input.GetAxis("Vertical");

        var move = new Vector3(_horizontal, _vertical,0f);
        if ( move!= Vector3.zero)
        {
            FacingDirection = new Vector2(move.x,move.y).normalized;
        }
        var moveNormalized = move.normalized * _speed;

        _rb.linearVelocity = new Vector3(moveNormalized.x, moveNormalized.y, 0f);
    }
    private void FlipSprite()
    {
        if (_horizontal > 0)
        {
            _sprite.flipX = false;
        }
        else if (_horizontal < 0)
        {
            _sprite.flipX= true;
        }
    }

    private void SetAnimation()
    {
        if(_horizontal != 0 || _vertical !=0)
        {
            _animator.SetBool("isRunning", true);
        }
        else if (_horizontal == 0 && _vertical == 0)
        {
            _animator.SetBool("isRunning", false);
        }
    }

}
