using System.Collections.Generic;
using UnityEngine;


public enum PoolGroup { 
    Common,
    Character
}

public class PoolManager: MonoBehaviour
{
    public static PoolManager Instance;

    [System.Serializable] 
    public class Pool
    {
        public string name;
        public GameObject prefab;
        public int size;
    }

    [SerializeField] private Pool[] pools;
    private Dictionary<string, Queue<GameObject>> _characterPools = new();
    private Dictionary<string,Queue<GameObject>> _commonPools = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void InitCharacterPools(Pool[] newPools)
    {
        InitGroup(_characterPools, newPools);
    }

    public void InitCommonPools(Pool[] pools)
    {
        InitGroup(_commonPools, pools); 
    }

    private void InitGroup(Dictionary<string, Queue<GameObject>> dict, Pool[] pools)
    {
        foreach (var pool in pools)
        {
            if (dict.ContainsKey(pool.name)) continue;

            var queue = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                var obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            dict.Add(pool.name, queue);
        }
    }


    public GameObject Spawn(PoolGroup group,string name, Vector3 pos, Quaternion rotation)
    {
        var dict = group == PoolGroup.Common ? _commonPools : _characterPools;

        if (!dict.TryGetValue(name, out var queue))
            return null;

        var obj = queue.Dequeue();
        obj.transform.SetPositionAndRotation(pos, rotation);
        obj.SetActive(true);
        return obj;
    }

    public void Despawn(PoolGroup group,string name, GameObject obj)
    {
        var dict = group == PoolGroup.Common ? _commonPools : _characterPools;

        if (!dict.ContainsKey(name)) return;

        obj.GetComponent<IPoolable>()?.OnDespawn();
        obj.SetActive(false);
        dict[name].Enqueue(obj);
    }
}