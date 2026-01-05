using UnityEngine;

public class EnemyAi: MonoBehaviour
{
    public event System.Action OnIdle;
    public event System.Action OnWander;
    public event System.Action<Transform> OnChase;

    [SerializeField] private float _radiusDetect = 5f;
    [SerializeField] private float _scanInterval = 0.5f;
    [SerializeField] private float _losePlayerTime = 1f;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Vector2 _idleTimeRange = new(1.5f, 3f);
    [SerializeField] private Vector2 _wanderTimeRange = new(2f, 4f);

    private readonly Collider2D[] _hits= new Collider2D[8];

    private float _scanTimer = 0f;
    private float _losePlayerTimer = 0f;
    private float _stateTimer=0f;
    private Transform _currentTarget;
    private EnemyState _state;

    private void Start()
    {
        ChangeState(EnemyState.IDLE);
    }

    private void Update()
    {
        ScanPlayer();
        UpdateStateTimer();
    }

    private void ScanPlayer()
    {
        _scanTimer -= Time.deltaTime;
        if (_scanTimer > 0f) return;
        _scanTimer = _scanInterval;

        //Debug.Log("checkkkk");
        var player = FindPlayer();
        if (player != null)
        {

            _currentTarget = player.transform;
            _losePlayerTimer = _losePlayerTime;
            ChangeState(EnemyState.CHASE);
        }
        else if (_state == EnemyState.CHASE)
        {
            _losePlayerTimer -= _scanInterval;
            if (_losePlayerTimer < 0f)
            {
                _currentTarget = null;
                ChangeState(EnemyState.IDLE);
            }
        }
    }

    private void UpdateStateTimer()
    {
        if (_state==EnemyState.CHASE) return;
        _stateTimer -= Time.deltaTime;
        if(_stateTimer > 0f) return;

        if (_state == EnemyState.IDLE)
            ChangeState(EnemyState.WANDER);
        else if (_state == EnemyState.WANDER)
            ChangeState(EnemyState.IDLE);

    }

    private void ChangeState(EnemyState newState)
    {
        if(_state == newState) return;  
        _state = newState;
        switch(newState)
        {
            case EnemyState.IDLE:
                _stateTimer= Random.Range(_idleTimeRange.x, _idleTimeRange.y);
                OnIdle?.Invoke();
                break;
            case EnemyState.CHASE:
                OnChase?.Invoke(_currentTarget);
                break;
            case EnemyState.WANDER:
                _stateTimer=Random.Range(_wanderTimeRange.x, _wanderTimeRange.y);
                OnWander?.Invoke();
                break;
        }
    }

    private BaseCharacter FindPlayer()
    {
        int count = Physics2D.OverlapCircleNonAlloc
            (transform.position,
            _radiusDetect,
            _hits,
            _playerLayer
            );
        for(int i=0; i<count; i++)
        {
            var player= _hits[i].GetComponent<BaseCharacter>();
            //Debug.Log(_hits[i].name + " | layer: " + _hits[i].gameObject.layer);

            if (player != null)
            {
                return player;
            }
        }
        return null;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radiusDetect);
    }

    public void ResetState()
    {
        _scanTimer = _scanInterval;
        _stateTimer = 0;
        _losePlayerTimer = 0;
        _currentTarget = null;
        ChangeState(EnemyState.IDLE);
    }

}