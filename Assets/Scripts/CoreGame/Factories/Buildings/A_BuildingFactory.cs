using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    Unknown,
    Farm,
    Barracks,
    Wall
};

public class A_BuildingFactory : T_SimpleFactory<A_BuildingFactory, A_Building>
{
    public List<A_Building> m_allBuildings = new List<A_Building>();
    A_Building m_lastSelectedBuilding = null;


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
    public override A_Building MakeObj()
    {
        A_Building obj = base.MakeObj();
        m_allBuildings.Add(obj);
        return obj;
    }
    public virtual A_Building MakeObj(PlayerNb x_playerNb)
    {
        A_Building obj = this.MakeObj();
        obj.SetPlayerOwnerNb(x_playerNb);
        return obj;
    }


    // Own methods :
    public bool TrySelectBuildingAtPos(Vector3 x_pos, out BuildingType x_buildingType)
    {
        bool ret = false;
        x_buildingType = BuildingType.Unknown;
        foreach (A_Building building in m_allBuildings)
        {
            BoxCollider2D coll = building.GetComponent<BoxCollider2D>();
            if (coll.OverlapPoint((Vector2)x_pos))
            {
                m_lastSelectedBuilding = building; // still needed ?
                x_buildingType = building.GetBuildingType();
                ret = true;
                break;
            }
        }
        return ret;
    }
    public bool TrySelectBuildingAtPos(Vector3 x_pos, out A_Building x_selectedBuilding)
    {
        bool ret = false;
        x_selectedBuilding = null;
        foreach (A_Building building in m_allBuildings)
        {
            BoxCollider2D coll = building.GetComponent<BoxCollider2D>();
            if (coll.OverlapPoint((Vector2)x_pos))
            {
                m_lastSelectedBuilding = building; // still needed ?
                x_selectedBuilding = building;
                ret = true;
                break;
            }
        }
        return ret;
    }
    public bool IsBuildingAtPos(Vector3 x_pos, out A_Building x_building)
    {
        bool ret = false;
        x_building = null;
        foreach (A_Building building in m_allBuildings)
        {
            BoxCollider2D coll = building.GetComponent<BoxCollider2D>();
            if (coll.OverlapPoint((Vector2)x_pos))
            {
                x_building = building;
                ret = true;
                break;
            }
        }
        return ret;
    }

    public A_Building GetLastSelectedBuilding()
    {
        return m_lastSelectedBuilding;
    }
}
