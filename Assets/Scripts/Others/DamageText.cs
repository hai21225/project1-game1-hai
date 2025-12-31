using TMPro;
using UnityEngine;

public class DamageText: MonoBehaviour
{
    /// <summary>
    /// maybe upgrade to object pooling
    /// </summary>

    [SerializeField] private float _moveUpSpeed= 1f;
    [SerializeField] private float _lifeTime = 0.5f;

    [SerializeField] private TextMeshProUGUI _textMesh;
    private  Color _color;

    private void Start()
    {
        _color = _textMesh.color;
    }

    public void SetDamage(float damage)
    {
        _textMesh.text = (damage).ToString();
    }

    private void Update()
    {
        transform.position += Vector3.up * _moveUpSpeed * Time.deltaTime;

        _color.a -= Time.deltaTime / _lifeTime;
        _textMesh.color = _color;

        if (_color.a <= 0)
        {
            Destroy(gameObject);
        }
    }

}