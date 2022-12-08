using System;
using System.Collections.Generic;
using UnityEngine;

public class GenericObjectPool : MonoBehaviour
{
    [Serializable]
    /// <summary>
    /// Struct of pool.
    /// Assign properties in inspector.
    /// </summary>
    /// <returns>Inactive pooled object of given type.</returns>
    public struct Pool<T> where T : MonoBehaviour, new()
    {
        public List<T> objects;
        public int amountToPool;
        public T objPrefab;
    }
    private static GenericObjectPool _instance;
    public static GenericObjectPool Instance
    {
        get
        {
            if (!_instance) _instance = FindObjectOfType(typeof(GenericObjectPool)) as GenericObjectPool;
            return _instance;
        }
    }
    [SerializeField] private Pool<MonoBehaviour>[] pools;
    private static List<MonoBehaviour> m_allPooledObjects = new List<MonoBehaviour>();
    public static List<MonoBehaviour> AllPooledObjects
    {
        get
        {
            return m_allPooledObjects;
        }
    }
    private void Start()
    {
        AddObjectsToPool();
    }
    /// <summary>
    /// Get inactive pooled object of type.
    /// </summary>
    /// <returns>Inactive pooled object of given type.</returns>
    public T GetPooledObject<T>() where T : MonoBehaviour, new()
    {
        for (var i = 0; i < pools.Length; i++)
        {
            var objs = pools[i].objects;
            var objType = pools[i].objPrefab.GetType();

            if (objType != typeof(T)) continue;

            for (var j = 0; j < objs.Count; j++)
            {
                var obj = objs[j];
                if (obj.gameObject.activeInHierarchy) continue;
                return obj as T;
            }
        }
        return null;
    }
    /// <summary>
    /// Iterates through all pools,
    /// instantiates corresponding pool's prefab,
    /// and adds instantiated object to the corresponding pool.
    /// </summary>
    private void AddObjectsToPool()
    {
        for (var j = 0; j < pools.Length; j++)
        {
            for (var i = 0; i < pools[j].amountToPool; i++)
            {
                var obj = Instantiate(pools[j].objPrefab, transform.position, Quaternion.identity);
                obj.gameObject.SetActive(false);
                pools[j].objects.Add(obj);
                AllPooledObjects.Add(obj);
            }
        }
    }
}