using UnityEngine;

public class CharlotteSkill2 : SkillBase
{
    [SerializeField] private SkillBase _dash;
    [SerializeField] private GameObject _skill2;
    [SerializeField] private float _timeExist = 0.1f;
    [SerializeField] private CharlotteTrigger _trigger;

    private void Start()
    {
        _skill2.SetActive(false);
        _trigger.OnEnemy += RaiseHitEnemy;
    }

    protected override void Execute(Vector2 dir)
    {
        CancelInvoke(nameof(Disable));
        _dash.TryUse(dir);
        _skill2.SetActive(true);
        Rotated(dir);
        Invoke(nameof(Disable),_timeExist);
    }

    private void Disable()
    {
        _skill2?.SetActive(false);
    }

    private void Rotated(Vector2 dir)
    {

        float angle= Mathf.Atan2(dir.y,dir.x)* Mathf.Rad2Deg;
        _skill2.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}