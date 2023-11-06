using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : A_Man
{
    private static uint m_FARMER_HP = 4;
    private static uint m_FARMER_ATK = 1;

    private static uint m_farmerGoldCreateCost = 0;
    private static uint m_farmerFoodCreateCost = 5;
    private static uint m_farmerFoodMaintainCost = 1;


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
        FarmerInit();
    }

    protected override void Update()
    {
        base.Update();
    }


    // A_Man inherited methods :
    // ...


    // Own methods :
    protected virtual void FarmerInit()
    {
        base.ManInit();
        m_maxHP = m_FARMER_HP;
        m_currentHP = m_maxHP;
        m_baseAttack = m_FARMER_ATK;
    }

    public static uint GetGoldTrainingCost()
    {
        return m_farmerGoldCreateCost;
    }
    public static uint GetFoodTrainingCost()
    {
        return m_farmerFoodCreateCost;
    }

    public static uint GetFoodMaintenanceCost()
    {
        return m_farmerFoodMaintainCost;
    }

}
