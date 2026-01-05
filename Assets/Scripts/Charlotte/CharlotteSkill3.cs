using UnityEngine;

public class CharlotteSkill3 : SkillBase
{
    [SerializeField] private GameObject _ulti;
    [SerializeField] private float _timeExist = 0.5f;
    [SerializeField] private CharlotteTrigger _logic;

    private void Start()
    {
        _ulti.SetActive(false);
        _logic.OnEnemy += RaiseHitEnemy;
    }
    protected override void Execute(Vector2 dir)
    {
        CancelInvoke(nameof(Disable));
        _ulti.SetActive(true);
        Invoke(nameof(Disable), _timeExist);
    }
    private void Disable()
    {
        _ulti?.SetActive(false);
    }

}