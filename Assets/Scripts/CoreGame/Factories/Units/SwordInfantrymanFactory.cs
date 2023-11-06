using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SwordInfantrymanFactory : A_InfantrymanFactory
{
    public Tile m_tileModelPlayerOne;
    public Tile m_tileModelPlayerTwo;

    // MonoBehaviour inherited "event methods" :
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        if (m_tileModelPlayerOne == null || m_tileModelPlayerTwo == null)
        {
            Debug.LogError(name + " : at least one tileModelPlayerX is null");
        }
    }


    /*public SwordInfantryman MakeSwordInfantryman()
    {
        return MakeSwordInfantryman(PlayerNb.Unknown);
    }*/
    public SwordInfantryman MakeSwordInfantryman(PlayerNb x_playerNb)
    {
        SwordInfantryman swordInf = null;
        if (m_objPool != null)
        {
            //Debug.Log(name + " : m_objPool is not null, MakeInfantryman() should work");
            swordInf = m_objPool.Get() as SwordInfantryman;
            swordInf.SetPlayerOwnerNb(x_playerNb);
            switch (x_playerNb)
            {
                case PlayerNb.One:
                    swordInf.SetTile(m_tileModelPlayerOne);
                    break;
                case PlayerNb.Two:
                    swordInf.SetTile(m_tileModelPlayerTwo);
                    break;
                case PlayerNb.Unknown:
                default:
                    Debug.LogWarning(name + " : Unknown playerNb");
                    break;
            }
            m_allInfantrymans.Add(swordInf);
        }
        else
        {
            Debug.LogError(name + " : m_objPool is null, MakeInfantryman() failed");
        }
        return swordInf;
    }
    public SwordInfantryman MakeSwordInfantryman(Vector3 x_centeredCellPos, PlayerNb x_playerNb)
    {
        SwordInfantryman swordInf = MakeSwordInfantryman(x_playerNb);
        swordInf.transform.position = x_centeredCellPos;
        return swordInf;
    }

    public override void RecycleInfantryman(A_Infantryman x_infantryman)
    {
        base.RecycleInfantryman(x_infantryman);
    }
}
