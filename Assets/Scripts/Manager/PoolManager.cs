using System.Collections.Generic;
using UnityEngine;

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
    private Dictionary<string, Queue<GameObject>> poolDict = new();

    //private void Awake()
    //{
    //    Instance= this;
    //    foreach(var pool in pools)
    //    {
    //        var queue= new Queue<GameObject>();
    //        for(int i=0; i<pool.size; i++)
    //        {
    //            var obj= Instantiate(pool.prefab,transform);
    //            obj.SetActive(false);
    //            queue.Enqueue(obj);
    //        }
    //        poolDict.Add(pool.name, queue);
    //    }
    //}

    private void Awake()
    {
        Instance = this;
    }
    public void InitPools(Pool[] newPools)
    {
        poolDict.Clear();

        foreach (Transform child in transform)
            Destroy(child.gameObject);

        foreach (var pool in newPools)
        {
            var queue = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                var obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            poolDict.Add(pool.name, queue);
        }
    }

    public GameObject Spawn(string name, Vector3 pos, Quaternion rotation)
    {
        if (!poolDict.TryGetValue(name, out var queue))
            return null;

        var obj = queue.Dequeue();
        obj.transform.SetPositionAndRotation(pos, rotation);
        obj.SetActive(true);
        return obj;
    }

    public void Despawn(string name, GameObject obj)
    {
        if (!poolDict.ContainsKey(name)) return;

        obj.GetComponent<IPoolable>()?.OnDespawn();
        obj.SetActive(false);
        poolDict[name].Enqueue(obj);
    }
}