using UnityEngine;

/*
 * TPooledObjFactory<T> is a template class to instance a factory of pooled objects as "unity singleton"
 */
public class T_PooledObjFactory<TPooledObjFactoryInstance, TPooledObj> : T_Singleton<TPooledObjFactoryInstance>
    where TPooledObjFactoryInstance : T_Singleton<TPooledObjFactoryInstance>
    where TPooledObj : A_MonoBehaviourPoolableObject
{
    public TPooledObj m_objBlueprint;
    protected T_ObjectsPool<TPooledObj> m_objPool;
    protected int m_nbObjectsFromStart = 10;
    protected int m_nbObjectsToExtend = 5;


    // MonoBehaviour inherited "event methods" (indirectly inherited from TSingleton<T>) :
    virtual protected void Start()
    {
        if (m_objBlueprint != null)
        {
            m_objPool = new T_ObjectsPool<TPooledObj>(
                () => // anonymous function
                {
                    return Instantiate(m_objBlueprint, Vector3.zero, Quaternion.identity);
                }, m_nbObjectsFromStart, m_nbObjectsToExtend);
        }
        else
        {
            Debug.LogWarning(name + " : m_objBlueprint is null on Start()");
        }
    }
}



/*
 * TSimpleFactory<T> is a template class to instance a simple factory of objects as "unity singleton"
 */
public class T_SimpleFactory<TSimpleFactoryInstance, TObj> : T_Singleton<TSimpleFactoryInstance>
    where TSimpleFactoryInstance : T_Singleton<TSimpleFactoryInstance>
    where TObj : MonoBehaviour
{
    public TObj m_objBlueprint;


    // MonoBehaviour inherited "event methods" (indirectly inherited from TSingleton<T>) :
    protected virtual void Start()
    {
        if (m_objBlueprint != null)
        {
            // ... nothing needed here ?
        }
        else
        {
            Debug.LogWarning(name + " : m_objBlueprint is null on Start()");
        }
    }

    // Own methods :
    public virtual TObj MakeObj()
    {
        // position can also be "Vector3.zero", and be initialized to the desired position later...
        TObj obj = Instantiate(m_objBlueprint, m_objBlueprint.transform.position, Quaternion.identity);
        return obj;
    }
} // this T_SimpleFactory<...> templated class needs to be tested !