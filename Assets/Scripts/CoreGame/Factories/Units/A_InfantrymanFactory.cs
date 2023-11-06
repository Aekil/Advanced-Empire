using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InfantryType
{
    UnknownInfantryType,
    SwordInfantryman
};

public abstract class A_InfantrymanFactory : A_UnitFactory//T_PooledObjFactory<A_InfantrymanFactory, A_Infantryman>
{
    protected InfantryType m_infantryType = InfantryType.UnknownInfantryType;
    //public List<A_Infantryman> m_allInfantrymans = new List<A_Infantryman>();
    /*private A_Infantryman m_lastSelectedInfantry = null;


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
    //public abstract A_Infantryman MakeInfantryman();


    // /!\ Note (deprecated ?) : when recycle method will be called in Card class,
    // we should encounter an issue because Card doesn't know Recycle...
    // multiple solutions, choose one to implement...
    // example at minut25 of '8PoolingGestionMemoire' video from Atelier Unity GameCodeur "monstres"
    // (with abstract class for Card and inheritance..)

    *//*public static void RecycleInfantryman(Infantryman x_Infantryman)
    {
        _instance.m_InfantrymansPool.Recycle(x_Infantryman);
    }*//*
    public virtual void RecycleInfantryman(A_Infantryman x_infantryman)
    {
        // both works : 
        //m_objPool.Recycle(x_infantryman);
        _instance.m_objPool.Recycle(x_infantryman);
        // so : should we use "_instance." here or not ?
    }

    // intended to work with m_allInfantrymans (new list) but never fully implemented/tested... :
    *//*public static void DeleteCard(Card x_card)
    {
        _instance.m_allCards.Remove(x_card);
        x_card.enabled = false;
    }*//*

    public bool TrySelectInfantryAtPos(Vector3 x_pos)
    {
        bool ret = false;
        foreach (A_Infantryman infantryman in m_allInfantrymans)
        {
            BoxCollider2D coll = infantryman.GetComponent<BoxCollider2D>();
            if (coll.OverlapPoint((Vector2)x_pos))
            {
                // pos collided with an infantryman
                m_lastSelectedInfantry = infantryman;
                ret = true;
                break;
            }
        }
        return ret;
    }

    // same as TrySelect, but don't change m_lastSelected...
    public bool IsInfantryAtPos(Vector3 x_pos, out A_Unit x_unit)
    {
        bool ret = false;
        x_unit = null;
        foreach (A_Infantryman infantryman in m_allInfantrymans)
        {
            BoxCollider2D coll = infantryman.GetComponent<BoxCollider2D>();
            if (coll.OverlapPoint((Vector2)x_pos))
            {
                x_unit = infantryman;
                ret = true;
                break;
            }
        }
        return ret;
    }

    public A_Infantryman GetLastSelectedInfantry()
    {
        return m_lastSelectedInfantry;
    }*/

    public InfantryType GetInfantryType()
    {
        return m_infantryType;
    }
}
