using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private static ObjectPool instance;

    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();

    private GameObject pool;

    public static ObjectPool Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;
        }
    }

    void Start()
    {
        
    }

    public GameObject GetObject(GameObject prefab)
    {
        GameObject _object;
        if(!objectPool.ContainsKey(prefab.name) || objectPool[prefab.name].Count == 0)
        {
            _object = GameObject.Instantiate<GameObject>(prefab);
            PushObject(_object);
            if (pool == null)
                pool = new GameObject("ObjectPool");
            GameObject childPool = GameObject.Find(prefab.name + "Pool");
            if (childPool == null)
            {
                childPool = new GameObject(prefab.name + "Pool");
                childPool.transform.SetParent(pool.transform);
            }
            _object.transform.SetParent(childPool.transform);
        }
        _object = objectPool[prefab.name].Dequeue();
        _object.SetActive(true);
        return _object;
    }

    public void PushObject(GameObject prefab)
    {
        string name = prefab.name.Replace("(Clone)", string.Empty);
        if (!objectPool.ContainsKey(name))
            objectPool.Add(name, new Queue<GameObject>());
        objectPool[name].Enqueue(prefab);
        prefab.SetActive(false);
    }
}
