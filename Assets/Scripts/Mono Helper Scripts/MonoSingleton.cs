using UnityEngine;


public class MonoSingleton<T>: MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    
    public static T Instance
    {
        get
        {
            if (_instance)
                return _instance;
            _instance = FindObjectOfType<T>();
            if (_instance) return _instance;
            var singletonObject = new GameObject(typeof(T).Name);
            _instance = singletonObject.AddComponent<T>();
            DontDestroyOnLoad(singletonObject);
            return _instance;
        }
    }
}