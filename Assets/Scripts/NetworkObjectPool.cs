using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkObjectPool : NetworkBehaviour
{
    public static NetworkObjectPool Singleton { get; private set; }

    [SerializeField] private GameObject prefabToPool;
    [SerializeField] private int poolSize = 10;

    private readonly Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        if (Singleton != null && Singleton != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Singleton = this;
        }
    }

    public void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefabToPool, Vector3.zero, Quaternion.identity);
            obj.name = prefabToPool.name + "_Pooled";
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    // Get an object from the pool
    public GameObject GetObject()
    {
        GameObject obj = pool.Dequeue();
        obj.SetActive(true);
        pool.Enqueue(obj); // Place it back at the end of the queue
        return obj;
    }

    // Return an object to the pool
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}