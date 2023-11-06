using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class A_Infantryman : A_Unit
{
    protected const uint m_INFANTRY_HP = 6;
    protected const uint m_INFANTRY_ATK = 3;

    protected static uint m_aInfGoldCreateCost = 1;
    protected static uint m_aInfFoodCreateCost = 8;
    protected static uint m_aInfFoodMaintainCost = 2;


    // IPoolableObject inherited methods :
    public override void Init()
    {
        base.Init();
    }

    public override void Reset()
    {
        base.Reset();
    }


    // MonoBehaviour inherited "event methods" :
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        InfantrymanInit();
    }

    protected override void Update()
    {
        base.Update();
    }


    // A_Unit inherited methods :
    // ...


    // Own methods :
    protected virtual void InfantrymanInit()
    {
        base.ManInit();
        m_maxHP = m_INFANTRY_HP;
        m_currentHP = m_maxHP;
        m_baseAttack = m_INFANTRY_ATK;
    }

}
