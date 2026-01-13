using System.Collections;
using UnityEngine;

public class Dash : SkillBase
{

    [SerializeField] private float _dashDistance = 5f;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private float _dashDuration = 0.15f;
    private bool _isDashing = false;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected override void Execute(Vector2 dir)
    {
        if (_isDashing) return;
        StartCoroutine(DashRoutine(dir));
    }

    private IEnumerator DashRoutine(Vector2 dir)
    {
        _isDashing = true;

        dir.Normalize();

        Vector3 startPos = transform.position;
        //Vector3 targetPos = startPos + (Vector3)(dir * _dashDistance);

        RaycastHit2D hit = Physics2D.Raycast(
    startPos,
    dir,
    _dashDistance,
    _wallLayer
);

        float finalDistance = hit ? hit.distance : _dashDistance;
        Vector3 targetPos = startPos + (Vector3)(dir * finalDistance);


        float time = 0f;
        if (Mathf.Abs(dir.x) > 0.01f)
            _spriteRenderer.flipX = dir.x < 0f;

        while (time < _dashDuration)
        {
            float t = time / _dashDuration;
            //_rb.transform.position = Vector3.Lerp(startPos, targetPos, t);
            _rb.MovePosition(Vector3.Lerp(startPos, targetPos, t));

            time += Time.deltaTime;
            yield return null;
        }

        _rb.transform.position = targetPos;
        _isDashing = false;
    }
}