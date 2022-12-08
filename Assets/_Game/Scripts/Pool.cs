using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : FastSingleton<Pool>
{
    [SerializeField] private List<GameObject> pooledObjects;
    [SerializeField] private GameObject objectToPool;
    [SerializeField] public Transform pool;
    [SerializeField] private int amountToPool;

    public List<GameObject> PooledObjects { get; set;}

    protected override void Awake()
    {
        base.Awake();
        OnInit();
    }

    public void OnInit()
    {
        pooledObjects = new List<GameObject>();

        for (int i  = 0 ; i < amountToPool; i++)
        {
            GameObject tmp = Instantiate(objectToPool,pool);
            tmp.name = "Brick" + i;
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    public GameObject GetObjectFromPool()
    {
        for (int i =0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    public void BackToPool(GameObject go)
    {
        go.SetActive(false);
        go.transform.parent = pool;
    }

    /*public int CountActive()
    {
        int count = 0;
        for (int i = 0; i < amountToPool; i++)
        {
            if (pooledObjects[i].activeInHierarchy)
            {
                count++;
            }
        }
        return count;
    }*/
}
