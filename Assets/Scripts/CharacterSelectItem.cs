using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _icon;
    private CharacterData _characterData;
    [SerializeField] private GameObject _selectedBorder;

    public CharacterData CharacterData => _characterData;

    //private bool _isDragging;
    private Vector2 _downPos;
    private const float TAP_THRESHOLD = 20f;

    public void Init(CharacterData data)
    {
        
        _characterData = data;
        _icon.sprite = data.portrait;
        SetSelected(false);
    }

    public void SetSelected(bool value)
    {
        if (_selectedBorder != null)
            _selectedBorder.SetActive(value);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        //_isDragging = false;
        _downPos = eventData.position;

        transform.localScale = Vector3.one * 0.95f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        transform.localScale = Vector3.one;

        float distance = Vector2.Distance(_downPos, eventData.position);

        if (distance > TAP_THRESHOLD)
            return; 

        CharacterSelectManager.Instance.OnItemSelected(this);
    }
}
