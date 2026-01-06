using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnClick;
    private bool _isPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;
        transform.localScale = Vector3.one * 0.95f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isPressed) return;

        _isPressed = false;
        transform.localScale = Vector3.one;
        OnClick?.Invoke();
    }
}