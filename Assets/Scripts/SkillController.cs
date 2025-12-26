using UnityEngine;
using UnityEngine.UI;

public class SkillController : MonoBehaviour
{
    // button for each skill
    [SerializeField] private SkillButton _skill1;
    [SerializeField] private SkillButton _skill2;
    [SerializeField] private SkillButton _skill3;

    //  indicator for each skill
    [SerializeField] private GameObject _indicatorSkill1;
    [SerializeField] private GameObject _indicatorSkill2;
    //[SerializeField] private GameObject _indicatorSkill3;

    // image for each skill, it bind with cooldown to make ui time remain
    [SerializeField] private Image _imageSkill1;
    [SerializeField] private Image _imageSkill2;
    [SerializeField] private Image _imageSkill3;


    [SerializeField] private Baron _baronSkill;
    [SerializeField] private Base _baron;
    private void Awake()
    {
        _baronSkill.GetComponent<Baron>();
        _indicatorSkill1.SetActive(false);
        _indicatorSkill2.SetActive(false);
    }


    private void Start()
    {
        if(_skill1 != null)
        {
            _skill1.OnPress += ShowIndicatorSkill1;
            _skill1.OnDragEvent += RotateIndicator1;
            _skill1.OnRelease += ActiveSkill1;
            _skill1.OnCancel += CancelSkill;
        }
        if(_skill2 != null)
        {
            _skill2.OnPress += ShowIndicatorSkill2;
            _skill2.OnDragEvent += RotateIndicator2;
            _skill2.OnRelease += ActiveSkill2;
            _skill2.OnCancel += CancelSkill;
        }
        if (_skill3 != null)
        {
            _skill3.OnPress += ShowIndicatorSkill3;
            _skill3.OnDragEvent += RotateIndicator3;
            _skill3.OnRelease += ActiveSkill3;
            _skill3.OnCancel += CancelSkill;
        }
    }

    private void Update()
    {
        TimeRemainSkillUi();
    }

    private void ShowIndicatorSkill1()
    {
        if (_baronSkill.CanUseSkill1())
        {
            _indicatorSkill1.SetActive(true);
        }
    }
    private void ShowIndicatorSkill2()
    {
        if (_baronSkill.CanUseSkill2())
        {
            _indicatorSkill2.SetActive(true);
        }
    }
    private void ShowIndicatorSkill3()
    {

    }
    private void RotateIndicator1(Vector2 direction)
    {
        if(direction== Vector2.zero)
        {
            direction= _baron.FacingDirection;
        }
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg ;
        _indicatorSkill1.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    private void RotateIndicator2(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            direction= _baron.FacingDirection;
        }
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _indicatorSkill2.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    private void RotateIndicator3(Vector2 direction)
    {

    }

    private void ActiveSkill1(Vector2 dir)
    {
        _indicatorSkill1.SetActive(false);
        Vector2 finaldir = dir;
        if (finaldir == Vector2.zero)
        {
            finaldir=_baron.FacingDirection;
        }
        _baronSkill.Skill1(finaldir);
    }
    private void ActiveSkill2(Vector2 dir)
    {
        _indicatorSkill2.SetActive(false);
        Vector2 finaldir = dir;
        if (finaldir == Vector2.zero)
        {
            finaldir= _baron.FacingDirection;
        }
        _baronSkill.Skill2(finaldir);
    }
    private void ActiveSkill3(Vector2 dir)
    {
        _baronSkill.Skill3(dir);
    }
    private void TimeRemainSkillUi()
    {
        _imageSkill1.fillAmount = _baronSkill.TimeRemainingSkill1();
        _imageSkill2.fillAmount= _baronSkill.TimeRemainingSkill2();
        _imageSkill3.fillAmount= _baronSkill.TimeRemainingSkill3();


        bool value1 = _baronSkill.CanUseSkill1();
        _skill1.SetInteractable(value1);

        bool value2= _baronSkill.CanUseSkill2();
        _skill2.SetInteractable(value2);

        bool value3= _baronSkill.CanUseSkill3();
        _skill3.SetInteractable(value3);
    }

    private void CancelSkill()
    {
        _indicatorSkill1.SetActive(false );
        _indicatorSkill2.SetActive(false);
        //_indicatorSkill3.SetActive(false);
    }
}