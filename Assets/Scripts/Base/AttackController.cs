using UnityEngine;

public class AttackController: MonoBehaviour
{
    [SerializeField] private CharacterStats _stats;
    [SerializeField] private EnemyListDetected _enemy;
    [SerializeField] private GameObject _indicator;
    private float _attackSpeed = 0f;
    private float _timer = 0f;
    private float _timeSlowDuration = 0f;

    private AttackButton _button;
    private IAttackExecutor _executor;
    private void Awake()
    {
        _button= FindAnyObjectByType<AttackButton>();
        _executor= GetComponent<IAttackExecutor>();
    }
    private void Start()
    {
        _attackSpeed = _stats.maxAttackSpeed;
        _indicator.SetActive(false);
    }

    private void Update()
    {
        if (_button == null) return;
        _timer-= Time.deltaTime;
        ResetSpeed();
        if (!_button.IsHolding)
        {
            return;
        }
        if(_timer> 0f ) { return; }
        var enemy = _enemy.Detected();
        if (enemy == null) { ShowIndicator(); return; }
        PerformAttack(enemy.transform);
    }

    private void PerformAttack(Transform target)
    {
        _timer = _attackSpeed;
        _executor.ExecuteAttack(target);
    }

    private void ResetSpeed()
    {
        if(_timeSlowDuration > 0f)
        {
            _timeSlowDuration-= Time.deltaTime;
            if(_timeSlowDuration < 0f)
            {
                _timeSlowDuration = 0f;
                _attackSpeed = _stats.maxAttackSpeed;
            }
        }
    }
    private void ShowIndicator()
    {
        if (_indicator == null) return;
        _indicator.SetActive(true);
        Invoke(nameof(HideIndicator), 0.12f);
    }
    private void HideIndicator()
    {
        _indicator.SetActive(false);
    }

    //API
    public void Slow(float slowPercent, float duration)
    {
        _attackSpeed += slowPercent * _stats.maxAttackSpeed;
        _timeSlowDuration = duration;
    }

}
