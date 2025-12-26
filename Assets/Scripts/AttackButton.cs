using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler
{
    public bool IsHolding { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsHolding = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            IsHolding= true;
        }
        else if( Input.GetKeyUp(KeyCode.Space))
        {
            IsHolding=false;
        }
    }
}
