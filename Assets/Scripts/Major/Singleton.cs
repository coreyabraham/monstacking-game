using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get => (T)_singleton; private set => _singleton = value; }
    private static Singleton<T> _singleton = null;
    private void Awake()
    {
        if (!_singleton)
        {
            _singleton = this;
            Initialize();
        }
    }
    protected abstract void Initialize();
}