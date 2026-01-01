using System.Collections;
using UnityEngine;

public class Dash : SkillBase
{

    [SerializeField] private float _dashDistance = 5f;
    [SerializeField] private float _dashSpeed = 20f;
    [SerializeField] private float _dashDuration = 0.15f;
    private bool _isDashing = false;

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
        Vector3 targetPos = startPos + (Vector3)(dir * _dashDistance);

        float time = 0f;

        while (time < _dashDuration)
        {
            float t = time / _dashDuration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        _isDashing = false;
    }
}