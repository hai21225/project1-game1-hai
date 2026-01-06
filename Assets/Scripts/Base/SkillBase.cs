using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    public float cooldown;
    protected float lastUseTime;
    public event System.Action OnSkillUsed;
    public event System.Action<EnemyHealth> OnHitEnemy;

    protected BaseCharacter character;

    private void Awake()
    {
        Debug.Log("aloooooooooooooooooooooooo");
        character = GetComponent<BaseCharacter>();
    }

    private void Start()
    {
        if (character == null)
        {
            Debug.Log("character nulll");
        }
        character.OnResetState += () => { Debug.Log("checkadakdaksdkasdk"); lastUseTime = 0f; };
    }

    private void Update()
    {
        if (lastUseTime > 0f)
        {
            lastUseTime -= Time.deltaTime;
        }
    }

    protected void RaiseHitEnemy(EnemyHealth enemy)
    {
        //Debug.Log("checkkkkkdmmmmmm");
        OnHitEnemy?.Invoke(enemy);
    }
    public bool CanUse()
    {
        return lastUseTime <= 0f;
    }

    public void TryUse(Vector2 dir,bool ignoreCooldown= false)
    {
        Debug.Log($"[TRY] {name} before = {lastUseTime}");
        if (!ignoreCooldown&&!CanUse()) return;

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
