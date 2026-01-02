using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private float _scaleUp = 1.2f;
    [SerializeField] private float _scaleTime = 0.08f;

    public bool IsHolding { get; private set; }
    private Coroutine _coroutine;
    private void Start()
    {
        _rect = GetComponent<RectTransform>();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        IsHolding = true;
        PlayPulse();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsHolding = false;
        ResetScale();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            IsHolding= true;
            PlayPulse();
        }
        else if( Input.GetKeyUp(KeyCode.Space))
        {
            IsHolding=false;
            ResetScale();
        }
    }

    private void PlayPulse()
    {
        if(_coroutine != null)
        {
            StopCoroutine( _coroutine );
        }
        _coroutine = StartCoroutine(PulseRoutine());
    }

    private IEnumerator PulseRoutine()
    {
        yield return ScaleTo(Vector3.one* _scaleUp);
        yield return ScaleTo(Vector3.one);
    }
    private IEnumerator ScaleTo(Vector3 target)
    {
        Vector3 start = _rect.localScale;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / _scaleTime;
            _rect.localScale = Vector3.Lerp(start, target, t);
            yield return null;
        }
        _rect.localScale=target;
    }
    private void ResetScale()
    {
        if(_coroutine != null)
        {
            StopCoroutine( _coroutine );
        }
        _rect.localScale= Vector3.one;
    }
}
