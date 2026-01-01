
using UnityEngine;
using UnityEngine.UI;

public class CharacterSkillController : MonoBehaviour
{
    //[SerializeField] private GameObject[] _indicators;
    [SerializeField] private SkillButton[] _skillButtons;
    [SerializeField] private Image[] _skillImages;
    [SerializeField] private Image[] _skillImagesBackground;
    private SkillIndicatorHandler _indicatorHandler;

    private ISkillUser _currentUser;
    private BaseCharacter _currentCharacter;

    private void Start()
    {
        for(int i= 0;i<_skillButtons.Length;i++)
        {
            int index = i;
            _skillButtons[index].OnPress += () => OnPressSkill(index);
            _skillButtons[index].OnDragEvent += (dir) => OnDragSkill(index, dir);
            _skillButtons[index].OnRelease += (dir) => OnReleaseSkill(index, dir);
            _skillButtons[index].OnCancel += () => CancleAllSkill();
        }
    }

    public void SetCharacter(BaseCharacter character, ISkillUser user)
    {
        Debug.Log("SetCharacter called");

        if (user == null)
        {
            Debug.LogError("ISkillUser is NULL");
            return;
        }

        if (user.SkillSet == null)
        {
            Debug.LogError("SkillSet is NULL");
            return;
        }

        _currentCharacter = character;
        _currentUser = user;
        _indicatorHandler =
    character.GetComponent<SkillIndicatorHandler>();
        _indicatorHandler.HideAll();
        for (int i = 0; i < 3; i++)
        {
            var skill = user.SkillSet.GetSkill(i);

            if (skill == null)
            {
                Debug.LogError($"Skill {i} is NULL");
                continue;
            }

            if (_skillImages[i] == null)
            {
                Debug.LogError($"SkillImage {i} is NULL");
                continue;
            }

            _skillImages[i].sprite = skill.icon;
            _skillImagesBackground[i].sprite=skill.icon;
            Debug.Log($"Set icon skill {i}");
        }
    }

    private void Update()
    {
        if (_currentUser == null) return;
        for (int i= 0; i < 3; i++)
        {
            _skillImages[i].fillAmount= _currentUser.GetCooldownPercent(i);// visual cooldown
            _skillButtons[i].SetInteractable(_currentUser.CanUseSkill(i));
        }
    }


    // event handle
    private void OnPressSkill(int index)
    {
        if (!_currentUser.CanUseSkill(index))
        {
            return;
        }
        _indicatorHandler.Show(index);
    }
    private void OnDragSkill(int index, Vector2 dir)
    {
        if (dir == Vector2.zero)
        {
            dir= _currentCharacter.FacingDirection;
        }
        _indicatorHandler.Rotate(index, dir);
    }
    private void OnReleaseSkill(int index,Vector2 dir)
    {
        _indicatorHandler.Hide(index);
        if(dir== Vector2.zero)
        {
            dir= _currentCharacter.FacingDirection;
        }
        _currentUser.UseSkill(index,dir);
    }
    private void CancleAllSkill()
    {
        _indicatorHandler.HideAll();    
    }

    

}