using UnityEngine;

public interface IEnemyAttack
{
    public void Attack(Transform target);
    public void OnSpawn();
    public void OnDespawn();
}