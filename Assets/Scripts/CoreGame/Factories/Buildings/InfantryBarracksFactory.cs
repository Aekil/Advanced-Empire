using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InfantryBarracksFactory : A_BarracksFactory
{
    public Tile m_tileModelPlayerOne;
    public Tile m_tileModelPlayerTwo;

    private SwordInfantrymanFactory m_swordInfantrymanFactory = null;

    // MonoBehaviour inherited "event methods" :
    protected override void Awake()
    {
        base.Awake();
        m_barracksType = BarracksType.InfantryBarracks;
    }

    protected override void Start()
    {
        base.Start();

        FactoriesManager factMan;
        if ((factMan = GetComponentInParent<FactoriesManager>()) != null)
        {
            if ((m_swordInfantrymanFactory = factMan.GetSwordInfantrymanFactory()) == null)
            {
                Debug.LogError(name + " : m_swordInfantrymanFactory is null");
            }
        }
        else
        {
            Debug.LogError(name + " : cannot found FactoriesManager component in parent");
        }
    }


    // T_SimpleFactory<...> inherited methods :
    /*public override A_Building MakeObj()
    {
        A_Building obj = base.MakeObj(); // is it an issue that it is a "A_Building" instead of a "A_Barracks" ? -> not until now...
        // ...
        InfantryBarracks infantryBarracks = obj as InfantryBarracks;
        if (m_swordInfantrymanFactory != null)
        {
            infantryBarracks.SetInfantryFactory(m_swordInfantrymanFactory);
        }
        else
        {
            Debug.LogWarning("m_swordInfantrymanFactory is null, cannot set it to new InfantryBarracks");
        }
        return obj;
    }*/
    public override A_Building MakeObj(PlayerNb x_playerNb)
    {
        A_Building obj = base.MakeObj(x_playerNb);
        InfantryBarracks infantryBarracks = obj as InfantryBarracks;
        switch (x_playerNb)
        {
            case PlayerNb.One:
                infantryBarracks.SetTile(m_tileModelPlayerOne);
                break;
            case PlayerNb.Two:
                infantryBarracks.SetTile(m_tileModelPlayerTwo);
                break;
            case PlayerNb.Unknown:
            default:
                Debug.LogWarning(name + " : Unknown playerNb");
                break;
        }
        if (m_swordInfantrymanFactory != null)
        {
            infantryBarracks.SetInfantryFactory(m_swordInfantrymanFactory);
        }
        else
        {
            Debug.LogWarning("m_swordInfantrymanFactory is null, cannot set it to new InfantryBarracks");
        }
        return obj;
    }
    /*public override A_Building MakeInfantryBarracks(PlayerNb x_playerNb)
    {
        InfantryBarracks obj = base.MakeObj(x_playerNb) as InfantryBarracks;
        switch (x_playerNb)
        {
            case PlayerNb.One:
                obj.SetTile(m_tileModelPlayerOne);
                break;
            case PlayerNb.Two:
                obj.SetTile(m_tileModelPlayerTwo);
                break;
            case PlayerNb.Unknown:
            default:
                Debug.LogWarning(name + " : Unknown playerNb");
                break;
        }
        InfantryBarracks infantryBarracks = obj as InfantryBarracks;
        if (m_swordInfantrymanFactory != null)
        {
            infantryBarracks.SetInfantryFactory(m_swordInfantrymanFactory);
        }
        else
        {
            Debug.LogWarning("m_swordInfantrymanFactory is null, cannot set it to new InfantryBarracks");
        }
        return obj;
    }*/
}
