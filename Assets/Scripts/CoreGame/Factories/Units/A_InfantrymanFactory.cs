using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InfantryType
{
    UnknownInfantryType,
    SwordInfantryman
};

public abstract class A_InfantrymanFactory : A_UnitFactory
{
    protected InfantryType m_infantryType = InfantryType.UnknownInfantryType;

    public InfantryType GetInfantryType()
    {
        return m_infantryType;
    }
}
