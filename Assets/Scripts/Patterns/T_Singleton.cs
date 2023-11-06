using UnityEngine;

public class T_Singleton<TInstance> : MonoBehaviour where TInstance : T_Singleton<TInstance>
{
    protected static TInstance _instance = null;
    public static TInstance Instance = _instance;
    public bool isPersistant = true;

    protected virtual void Awake()
    {
        if (isPersistant)
        {
            if (!_instance)
            {
                _instance = this as TInstance;
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
        }
        else
        {
            _instance = this as TInstance;
        }
    }
}