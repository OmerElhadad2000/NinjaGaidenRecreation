using System;
using System.Collections.Generic;
using Mono_Pool;
using Unity.VisualScripting;
using UnityEngine;

public class SoundMonoPool<T> : MonoSingleton<SoundMonoPool<T>> where T : MonoBehaviour, IPoolableSoundSource
{
    [SerializeField]
    private int initialSize;
    
    [SerializeField] 
    private T prefab;
    
    [SerializeField] 
    private Transform parent;
    
    private Stack<T> _pool;
    private int _activeObjectsCounter;
    private int _spawnedObjectsCounter;
    
    public void Awake()
    {
        _pool = new Stack<T>();
        AddObjectsToPool();
    }
    
    public T Get()
    {
        if (_pool.Count == 0)
        {
            AddObjectsToPool();
        }
        var obj = _pool.Pop();
        obj.gameObject.SetActive(true);
        _activeObjectsCounter += 1;
        _spawnedObjectsCounter += 1;
        obj.Reset();
        return obj;
    }
    
    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Push(obj);
        if (_activeObjectsCounter > 0)
            _activeObjectsCounter -= 1;
    }
    
    private void AddObjectsToPool()
    {
        for (var i = 0; i < initialSize; i++)
        {
            var obj = Instantiate(prefab, parent, true);
            obj.gameObject.SetActive(false);
            _pool.Push(obj);
        }
    }
}