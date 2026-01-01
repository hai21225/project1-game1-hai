using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    public float cooldown;
    protected float lastUseTime;
    public event System.Action OnSkillUsed;

    private void Update()
    {
        if (lastUseTime > 0f)
        {
            lastUseTime -= Time.deltaTime;
        }
    }

    public bool CanUse()
    {
        return lastUseTime <= 0f;
    }

    public void TryUse(Vector2 dir)
    {
        if (!CanUse()) return;

        lastUseTime = cooldown;
        Execute(dir);
        OnSkillUsed?.Invoke();
    }

    protected abstract void Execute(Vector2 dir);

    public float CooldownPercent()
    {
        float t = lastUseTime / cooldown;
        return Mathf.Clamp01(1-t);
    }
}
