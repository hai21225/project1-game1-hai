using UnityEngine;

public class EnemyAnimator: MonoBehaviour
{
    [SerializeField] private string _isRunning = "isRunning";
    [SerializeField] private string _attack = "attack";

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        _animator= GetComponent<Animator>();
        _spriteRenderer= GetComponent<SpriteRenderer>();
    }

    public void SetRunAnimation(bool value)
    {
        _animator.SetBool(_isRunning,value);
    }

    public void SetAttackAnimation()
    {
        _animator.SetTrigger(_attack);
    }

    public void Flip(bool value)
    {
        if (value)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}