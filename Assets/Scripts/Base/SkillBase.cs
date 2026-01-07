using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    public float cooldown;
    protected float cooldownTimer;
    public event System.Action OnSkillUsed;
    public event System.Action<EnemyHealth> OnHitEnemy;

    protected BaseCharacter character;

    private void Awake()
    {
        //Debug.Log("aloooooooooooooooooooooooo");
        character = GetComponent<BaseCharacter>();
    }
    private void OnEnable()
    {
        character.OnResetState += ResetTimer;
    }
    private void OnDisable()
    {
        character.OnResetState -= ResetTimer;
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void ResetTimer()
    {
        cooldownTimer = 0f;
    }

    protected void RaiseHitEnemy(EnemyHealth enemy)
    {
        //Debug.Log("checkkkkkdmmmmmm");
        OnHitEnemy?.Invoke(enemy);
    }
    public bool CanUse()
    {
        return cooldownTimer <= 0f;
    }

    public void TryUse(Vector2 dir,bool ignoreCooldown= false)
    {
        //Debug.Log($"[TRY] {name} before = {cooldownTimer}");
        if (!ignoreCooldown&&!CanUse()) return;

        cooldownTimer = cooldown;
        Execute(dir);
        OnSkillUsed?.Invoke();
    }

    protected abstract void Execute(Vector2 dir);

    public float CooldownPercent()
    {
        float t = cooldownTimer / cooldown;
        return Mathf.Clamp01(1-t);
    }
}
