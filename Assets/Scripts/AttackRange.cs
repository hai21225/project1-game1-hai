using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{

    private List<Enemy> enemies = new List<Enemy>();


    public Enemy currentEnemy
    {
        get
        {
            enemies.RemoveAll(e => e == null);
            if (enemies.Count == 0) return null;
            return enemies[0];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out Enemy enemy))
        {
            if (!enemies.Contains(enemy))
                enemies.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.TryGetComponent(out Enemy enemy))
        {
            enemies.Remove(enemy);
        }
    }
}