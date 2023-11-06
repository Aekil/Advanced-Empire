using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Unknown,
    Farmer,
    Infantry,
    Archery,
    Cavalry,
    Seat
};

public class A_UnitFactory : T_PooledObjFactory<A_UnitFactory, A_Unit>
{
    // TODO : implement this base class for all unit factories,
    // then modify current unit factories to inherit this class

    protected UnitType m_unitType = UnitType.Unknown;
    public List<A_Unit> m_allUnits = new List<A_Unit>();
    A_Unit m_lastSelectedUnit = null;


    // MonoBehaviour inherited "event methods" :
    protected override void Awake() // inherited from T_Singleton<...>
    {
        base.Awake();
        m_nbObjectsFromStart = 10;
        m_nbObjectsToExtend = 5;
    }

    protected override void Start() // inherited from T_PooledObjFactory<...>
    {
        base.Start();
        // ...
    }


    // Own methods :
    public virtual void RecycleUnit(A_Unit x_unit)
    {
        // both works : 
        //m_objPool.Recycle(x_unit);
        _instance.m_objPool.Recycle(x_unit);
        // so : should we use "_instance." here or not ?
    }

    // intended to work with m_allInfantrymans (new list) but never fully implemented/tested... :
    /*public static void DeleteCard(Card x_card)
    {
        _instance.m_allCards.Remove(x_card);
        x_card.enabled = false;
    }*/

    public bool TrySelectUnitAtPos(Vector3 x_pos)
    {
        bool ret = false;
        foreach (A_Unit unit in m_allUnits)
        {
            BoxCollider2D coll = unit.GetComponent<BoxCollider2D>();
            if (coll.OverlapPoint((Vector2)x_pos))
            {
                // pos collided with an unit
                m_lastSelectedUnit = unit;
                ret = true;
                break;
            }
        }
        return ret;
    }

    // same as TrySelect, but don't change m_lastSelected...
    public bool IsUnitAtPos(Vector3 x_pos, out A_Unit x_unit)
    {
        bool ret = false;
        x_unit = null;
        foreach (A_Unit unit in m_allUnits)
        {
            BoxCollider2D coll = unit.GetComponent<BoxCollider2D>();
            if (coll.OverlapPoint((Vector2)x_pos))
            {
                x_unit = unit;
                ret = true;
                break;
            }
        }
        return ret;
    }

    public A_Unit GetLastSelectedUnit()
    {
        return m_lastSelectedUnit;
    }

    public UnitType GetUnitType()
    {
        return m_unitType;
    }
}
