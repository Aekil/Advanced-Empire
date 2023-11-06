using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class A_Barracks : A_Building
{
    protected BarracksType m_barracksType;
    protected const uint m_BARRACKS_MAX_HP = 8;


    // MonoBehaviour inherited "event methods" :
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        //Debug.Log("Start() called on " + gameObject.name);
        BuildingInit();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }


    // A_Building inherited methods :
    protected override void BuildingInit()
    {
        m_buildingType = BuildingType.Barracks;
        m_barracksType = BarracksType.UnknownBarracksType;
        m_maxHP = m_BARRACKS_MAX_HP;
        m_currentHP = m_maxHP;
    }

    // Own methods :
    public BarracksType GetBarracksType()
    {
        return m_barracksType;
    }
    protected void SetBarracksType(BarracksType x_barracksType)
    {
        m_barracksType = x_barracksType;
    }
}
