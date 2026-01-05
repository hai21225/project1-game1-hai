using UnityEngine;

public class BaronSkill3 : SkillBase
{
    [SerializeField] private GameObject _rangeStrike;
    [SerializeField] private GameObject _effect;
    [SerializeField] private float _timeExist=1f;

    private void Start()
    {
        _rangeStrike.SetActive(false);
        _effect.SetActive(false);
    }

    protected override void Execute(Vector2 dir)
    {
        CancelInvoke(nameof(Disablee));

        _rangeStrike.SetActive(true);
        _effect.SetActive(true);
        

        Invoke(nameof(Disablee), _timeExist);
    }


    private void Disablee()
    {
        _effect.SetActive(false);   
        _rangeStrike.SetActive(false);
    }

}