using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// game play use game session instead use poolmanager. Poolmanager is a provider pools for gamesession
/// </summary>
public class GameSession : MonoBehaviour
{
    public static GameSession instance;

    private readonly HashSet<IGameSessionObject> _objects = new();

    private void Awake()
    {
        instance = this;
    }

    public GameObject Spawn(PoolGroup group, string nameObject, Vector3 pos, Quaternion rot)
    {
        var go = PoolManager.Instance.Spawn(group, nameObject, pos, rot);
        if (go == null) return null;

        var obj = go.GetComponent<IGameSessionObject>();
        _objects.Add(obj);

        return go;
    }

    public void Despawn(PoolGroup group, string nameObject, IGameSessionObject obj, GameObject go)
    {
        _objects.Remove(obj);
        PoolManager.Instance.Despawn(group, nameObject, go);
    }

    public void ResetSession()
    {
        var snapshot = new List<IGameSessionObject>(_objects);

        foreach (var obj in snapshot)
        {
            obj.Return();
        }

        _objects.Clear();
    }

}