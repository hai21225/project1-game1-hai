using System;
using System.Collections.Generic;
using UnityEngine;
using static PoolManager;


public enum PoolGroup { 
    Common,
    Character
}

public class PoolManager: MonoBehaviour
{
    public static PoolManager Instance;

    public event Action OnPoolInitFinished;

    private int _initGroupCounter;

    [System.Serializable] 
    public class Pool
    {
        public string name;
        public GameObject prefab;
        public int size;
    }

    public class PoolRuntime
    {
        public Queue<GameObject> idle = new();
        public HashSet<GameObject> active = new();
    }

    [SerializeField] private Pool[] pools;
    private Dictionary<string, PoolRuntime> _characterPools = new();
    private Dictionary<string,PoolRuntime> _commonPools = new();

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
        _initGroupCounter++;
        InitGroup(_characterPools, newPools);
        OnGroupInitFinished();
    }

    public void InitCommonPools(Pool[] pools)
    {
        _initGroupCounter++;
        InitGroup(_commonPools, pools); 
        OnGroupInitFinished ();
    }

    private void InitGroup(Dictionary<string, PoolRuntime> dict, Pool[] pools)
    {
        foreach (var pool in pools)
        {
            if (dict.ContainsKey(pool.name)) continue;

           
            var runtime= new PoolRuntime();

            for (int i = 0; i < pool.size; i++)
            {
                var obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                runtime.idle.Enqueue(obj);
            }
            dict.Add(pool.name, runtime);
        }
    }


    public GameObject Spawn(PoolGroup group,string name, Vector3 pos, Quaternion rotation)
    {
        var dict = group == PoolGroup.Common ? _commonPools : _characterPools;

        if (!dict.TryGetValue(name, out var pool))
            return null;

        if (pool.idle.Count == 0)
            return null;

        var obj = pool.idle.Dequeue();
        pool.active.Add(obj);

        obj.transform.SetPositionAndRotation(pos, rotation);
        obj.SetActive(true);
        return obj;
    }

    public void Despawn(PoolGroup group,string name, GameObject obj)
    {
        var dict = group == PoolGroup.Common ? _commonPools : _characterPools;

        if (!dict.TryGetValue(name, out var pool))
            return;

        if (!pool.active.Remove(obj))
            return;

        obj.GetComponent<IPoolable>()?.OnDespawn();
        obj.SetActive(false);
        pool.idle.Enqueue(obj);
    }

    public void ClearGroup(PoolGroup group)
    {
        var dict = group == PoolGroup.Common ? _commonPools : _characterPools;

        foreach (var pool in dict.Values)
        {
            foreach (var obj in pool.active)
            {
                if (obj != null)
                    Destroy(obj);
            }

            foreach (var obj in pool.idle)
            {
                if (obj != null)
                    Destroy(obj);
            }
        }

        dict.Clear();
    }

    private void OnGroupInitFinished()
    {
        _initGroupCounter--;
        if (_initGroupCounter <= 0)
        {
            OnPoolInitFinished?.Invoke();    
        }
    }

}