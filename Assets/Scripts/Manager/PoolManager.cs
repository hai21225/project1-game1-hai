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
    private Dictionary<string, Queue<GameObject>> poolDict;

    private void Awake()
    {
        Instance= this;
        poolDict= new Dictionary<string, Queue<GameObject>>();
        foreach(var pool in pools)
        {
            var queue= new Queue<GameObject>();
            for(int i=0; i<pool.size; i++)
            {
                var obj= Instantiate(pool.prefab,transform);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            poolDict.Add(pool.name, queue);
        }
    }
    public GameObject Spawn(string name, Vector3 pos, Quaternion rotation)
    {
        if (!poolDict.ContainsKey(name))
        {
            return null;
        }
        var obj= poolDict[name].Dequeue();
        obj.transform.SetPositionAndRotation(pos, rotation);
        obj.SetActive(true);
        obj.GetComponent<IPoolable>()?.OnSpawn();
        poolDict[name].Enqueue(obj);
        return obj;
    }
}