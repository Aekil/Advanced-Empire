using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface I_PoolableObject
{
    void Init();
    void Reset();
}


public abstract class A_MonoBehaviourPoolableObject : MonoBehaviour, I_PoolableObject
{
    // IPoolableObject inherited methods to implement :
    public abstract void Init();
    public abstract void Reset();


    // MonoBehaviour inherited methods :
    protected abstract void Start();
    protected abstract void Update();
}


public class T_ObjectsPool<T> where T : I_PoolableObject
{
    private Queue<T> m_available;
    private List<T> m_used;
    private Func<T> _Create;
    private const int m_minExpandStep = 1;
    private int m_expandCount = m_minExpandStep;

    public T_ObjectsPool(Func<T> x_objCreator,
        int x_startingCapacity, int x_expandStep = m_minExpandStep)
    {
        m_available = new Queue<T>(x_startingCapacity);
        m_used = new List<T>(x_startingCapacity);
        _Create = x_objCreator;
        m_expandCount = Mathf.Max(x_expandStep, m_minExpandStep);
        CreateAndAddObjects(x_startingCapacity);
    }

    private void CreateAndAddObjects(int x_count)
    {
        for (int i = 0; i < x_count; i++)
        {
            CreateAndAddObject();
        }
    }
    private void CreateAndAddObject()
    {
        T obj = _Create();
        obj.Reset();
        m_available.Enqueue(obj);
    }

    private void Expand()
    {
        CreateAndAddObjects(m_expandCount);
    }

    public T Get()
    {
        if (m_available.Count <= 0)
        {
            Expand();
        }
        T obj = m_available.Dequeue();
        m_used.Add(obj);
        obj.Init();
        return obj;
    }

    public void Recycle(T x_obj)
    {
        if (m_used.Remove(x_obj))
        {
            x_obj.Reset();
            m_available.Enqueue(x_obj);
        }
    }
}
