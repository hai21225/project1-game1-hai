using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _icon;
    private CharacterData _characterData;

    //private bool _isDragging;
    private Vector2 _downPos;
    private const float TAP_THRESHOLD = 20f;

    public void Init(CharacterData data)
    {
        //Debug.Log("nguuuuuuuuuuuuuuuuuuuu");
        _characterData = data;
        _icon.sprite = data.portrait;
        //Debug.Log("checkakdkakdad");
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

        CharacterSelectManager.Instance.SelectCharacter(_characterData);
    }
}
