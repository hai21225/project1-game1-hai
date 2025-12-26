using UnityEngine;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _cancelZone;
    private bool _isCancel;

    public bool Interactable { get; private set; } = true;

    private Vector2 _startPosition;
    public event System.Action OnPress;
    public event System.Action<Vector2> OnRelease;
    public event System.Action<Vector2> OnDragEvent;
    public event System.Action OnCancel;
    private void Start()
    {
        _cancelZone.gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!Interactable)
        {
            return;
        }
        Vector2 direction = eventData.position - _startPosition;
        OnDragEvent?.Invoke(direction);
        if (_cancelZone == null) return;
        bool inCancel = RectTransformUtility.RectangleContainsScreenPoint(
                            _cancelZone,
                            eventData.position,
                            eventData.pressEventCamera
                            );
        _isCancel= inCancel;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Interactable)
        {
            return; 
        }
        _startPosition = eventData.position;
        _isCancel = false;
        if(_cancelZone  != null)
        {
            _cancelZone.gameObject.SetActive(true);
        }
        OnPress?.Invoke();
        //first
        OnDragEvent.Invoke(_startPosition - _startPosition);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!Interactable)
        {
            return;
        }
        Vector2 direction= eventData.position- _startPosition;
        if (_cancelZone != null)
        {
            _cancelZone.gameObject.SetActive(false);
        }
        if (_isCancel)
        {
            OnCancel?.Invoke();
            return;
        }
        OnRelease?.Invoke(direction.normalized);

    }

    public void SetInteractable(bool value)
    {
        Interactable = value;
    }
}