using System;
using UnityEngine;

public class SkillIndicatorHandler: MonoBehaviour
{
    [SerializeField] private GameObject[] _indicators;

    public void Show(int index)
    {
        if (index >= _indicators.Length) return;
        _indicators[index].SetActive(true);
    }

    public void Hide(int index)
    {
        if (index >= _indicators.Length) return;
        _indicators[index].SetActive(false);
    }
    public void HideAll()
    {
        foreach (var i in _indicators)
            i.SetActive(false);
    }
    public void Rotate(int index, Vector2 dir)
    {
        if (dir == Vector2.zero) return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _indicators[index].transform.rotation =
            Quaternion.Euler(0, 0, angle);
    }
}