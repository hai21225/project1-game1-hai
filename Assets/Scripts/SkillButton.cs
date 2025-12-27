using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _cancelZone;
    [SerializeField] private GameObject _joystickRoot;
    [SerializeField] private RectTransform _joystickHandle;
    [SerializeField] private RectTransform _joystickArea;
    [SerializeField] private float _joystickRadius = 80f;
    private bool _isCancel;
    private Image _handle, _area;

    public bool Interactable { get; private set; } = true;

    private Vector2 _startPosition;
    public event System.Action OnPress;
    public event System.Action<Vector2> OnRelease;
    public event System.Action<Vector2> OnDragEvent;
    public event System.Action OnCancel;
    private void Start()
    {
        _handle = _joystickHandle.GetComponent<Image>();
        _area = _joystickArea.GetComponent<Image>();
        _cancelZone.gameObject.SetActive(false);
        _joystickRoot.SetActive(false);

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!Interactable)
        {
            return;
        }
        Vector2 direction = eventData.position - _startPosition;
        if (_joystickRoot != null)
        {
            Vector2 clampedDir = Vector2.ClampMagnitude(direction, _joystickRadius);
            _joystickHandle.anchoredPosition = clampedDir;
        }
        OnDragEvent?.Invoke(direction);
        if (_cancelZone == null) return;
        bool inCancel = RectTransformUtility.RectangleContainsScreenPoint(
                            _cancelZone,
                            eventData.position,
                            eventData.pressEventCamera
                            );
        _isCancel= inCancel;
        if (_isCancel)
        {
            if(_handle != null && _area != null )
            {
                _handle.color= Color.red;
                _area.color = Color.red;
            }

        }
        else
        {
            if (_handle != null && _area != null)
            {
                _handle.color = Color.white;
                _area.color = Color.white;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Interactable)
        {
            return; 
        }
        if (_joystickRoot != null)
        {
            _joystickRoot.SetActive(true);
            
            _joystickHandle.anchoredPosition = Vector2.zero;
        }

        _startPosition = eventData.position;
        _isCancel = false;
        if(_cancelZone  != null)
        {
            _cancelZone.gameObject.SetActive(true);
        }
        OnPress?.Invoke();
        //first
        OnDragEvent.Invoke(eventData.position - _startPosition);
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
        if (_joystickRoot != null)
        {
            _joystickRoot.SetActive(false);
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