using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour
    where T : MonoBehaviourSingleton<T>
{
    private static T _instance;
    private static bool _isQuitting = false;
    private static readonly object _lock = new object();

    public static T Instance
    {
        get
        {
            if (_isQuitting) return null;

            lock (_lock)
            {
                if (_instance == null)
                {
                    var objs = FindObjectsByType<T>(FindObjectsSortMode.None);
                    if (objs.Length > 0)
                    {
                        _instance = objs[0];
                        if (objs.Length > 1)
                            Debug.LogError($"There is more than one {typeof(T).Name} in the scene.");
                    }
                    else
                    {
                        var obj = new GameObject(typeof(T).Name);
                        _instance = obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Debug.LogWarning($"Duplicate {typeof(T).Name} found, destroying.");
            Destroy(gameObject);
            return;
        }
        _isQuitting = false;
    }

    protected virtual void OnApplicationQuit()
    {
        _isQuitting = true;
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}

public class MonoBehaviourSingletonPersistent<T> : MonoBehaviour
    where T : MonoBehaviourSingletonPersistent<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}