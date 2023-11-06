using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BarracksType
{
    UnknownBarracksType,
    InfantryBarracks,
    ArcheryBarracks,
    CavalryBarracks
};

public abstract class A_BarracksFactory : A_BuildingFactory
{
    protected BarracksType m_barracksType = BarracksType.UnknownBarracksType;

    // MonoBehaviour inherited "event methods" :
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }


    // T_SimpleFactory<...> inherited methods :
    public override A_Building MakeObj(PlayerNb x_playerNb)
    {
        A_Building obj = base.MakeObj(x_playerNb); // is it an issue that it is a "A_Building" instead of a "A_Barracks" ? -> not until now...
        // ...
        return obj;
    }


    // Own methods :
    public BarracksType GetBarracksType()
    {
        return m_barracksType;
    }
}
