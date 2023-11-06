using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : T_Singleton<GameManager>
{
    public FactoriesManager m_factoriesManager = null;

    private static InfantryBarracksFactory m_infantryBarracksFactory = null;
    private static SwordInfantrymanFactory m_swordInfantrymanFactory = null;

    private static GameMap m_gameMap = null;

    private static Player m_playerOne = null;
    private static Player m_playerTwo = null;
    private static PlayerNb m_playerNbPlaying = PlayerNb.Unknown;


    override protected void Awake()
    {
        base.Awake();
        transform.parent = null; // make sure it is a "root level gameObject"
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if ((m_gameMap = GetComponentInChildren<GameMap>()) == null)
        {
            Debug.LogError(name + " : Cannot GetComponentInChildren<GameMap>(), m_gameMapRef is null");
        }
        if (m_factoriesManager == null)
        {
            Debug.LogError(name + " : m_factoriesManager is null");
        }
        else
        {
            m_infantryBarracksFactory = m_factoriesManager.GetInfantryBarracksFactory();
            m_swordInfantrymanFactory = m_factoriesManager.GetSwordInfantrymanFactory();
        }
        InitPlayers();
    }

    private void Update()
    {
        // nothing needed here for now...
    }

    // Own methods :
    private void InitPlayers()
    {
        m_playerNbPlaying = PlayerNb.One;
        // assume for now that the only available mode is "multi local" with 2 players mandatory
        Player[] playerArray;
        if ((playerArray = GetComponentsInChildren<Player>()) != null)
        {
            if (playerArray.Length != 2)
            {
                Debug.LogError(name + " : there should be exactly 2 players");
                return;
            }
            if (playerArray[0].GetPlayerNb() == PlayerNb.One)
            {
                if (playerArray[1].GetPlayerNb() == PlayerNb.Two)
                {
                    m_playerOne = playerArray[0];
                    m_playerTwo = playerArray[1];
                }
                else
                {
                    Debug.LogError(name + " : the second Player gameObj don't have the proper playerNb setted;" +
                        " it has : " + playerArray[1].GetPlayerNb() + ", instead of : " + PlayerNb.Two);
                }
            }
            else if (playerArray[0].GetPlayerNb() == PlayerNb.Two)
            {
                if (playerArray[1].GetPlayerNb() == PlayerNb.One)
                {
                    m_playerTwo = playerArray[0];
                    m_playerOne = playerArray[1];
                }
                else
                {
                    Debug.LogError(name + " : the second Player gameObj don't have the proper playerNb setted;" +
                        " it has : " + playerArray[1].GetPlayerNb() + ", instead of : " + PlayerNb.One);
                }
            }
            // setup the UI texts for the 1st playing turn
            TextPlayerResources.SetNewFoodAmountToDisplay(m_playerOne.GetFoodAmount());
            TextPlayerResources.SetNewGoldAmountToDisplay(m_playerOne.GetGoldAmount());
        }
        else
        {
            Debug.LogError(name + " : there should be 2 players in children");
        }
    }

    public FactoriesManager GetFactoriesManager()
    {
        return m_factoriesManager;
    }

    public static InfantryBarracksFactory GetInfantryBarracksFactory()
    {
        return m_infantryBarracksFactory;
    }
    public static SwordInfantrymanFactory GetSwordInfantrymanFactory()
    {
        return m_swordInfantrymanFactory;
    }

    public static GameMap GetGameMap()
    {
        return m_gameMap;
    }

    public static Player GetPlayerOne()
    {
        return m_playerOne;
    }
    public static Player GetPlayerTwo()
    {
        return m_playerTwo;
    }
    public static PlayerNb GetPlayerNbCurrentlyPlaying()
    {
        return m_playerNbPlaying;
    }
    public static Player GetPlayerCurrentlyPlaying()
    {
        Player ret = null;
        // /!\ we assume here that m_playerOne & m_playerTwo have different m_playerNb value
        //      and that there is only 2 players...
        if (m_playerNbPlaying == m_playerOne.GetPlayerNb())
        {
            ret = m_playerOne;
        }
        else if (m_playerNbPlaying == m_playerTwo.GetPlayerNb())
        {
            ret = m_playerTwo;
        }
        return ret;
    }

    public static void EndCurrentTurn()
    {
        // (i) in a 1st time, reset all turn-related variables of the current player
        foreach (A_Unit unit in m_swordInfantrymanFactory.m_allUnits)//m_allInfantrymans)
        {
            // reset all turn-related variables of A_Unit
            unit.SetMovedThisTurn(false);
            unit.SetCreatedThisTurn(false);
        }
        m_gameMap.DisableAllActivatedPanels();
        m_gameMap.DisableSingleSelectedCellArea();

        // (i) in a 2nd time, only now we can change the current player
        // (i) we assume here that there is only 2 players, and that the proper playerNb values are checked inside InitPlayers()
        if (m_playerNbPlaying == m_playerOne.GetPlayerNb())
        {
            m_playerNbPlaying = m_playerTwo.GetPlayerNb();
        }
        else if (m_playerNbPlaying == m_playerTwo.GetPlayerNb())
        {
            m_playerNbPlaying = m_playerOne.GetPlayerNb();
        }
    }

    public static void CreateInfantryBarracksAtPos(Vector3 x_centeredCellPos)
    {
        if (m_infantryBarracksFactory != null)
        {
            Player currentPlayer = GetPlayerCurrentlyPlaying();
            uint goldAmount = InfantryBarracks.GetGoldCreateCost();
            if (currentPlayer.TryPayGoldAmount(goldAmount))
            { // goldAmount successfully payed
                Debug.Log("created building");
                AudioManager.PlayFXSound(FXSoundID.construct1);
                InfantryBarracks infBarr = m_infantryBarracksFactory.MakeObj(m_playerNbPlaying) as InfantryBarracks;
                infBarr.transform.position = x_centeredCellPos;
            }
            else
            { // not enough gold to pay the required amount
                AudioManager.PlayFXSound(FXSoundID.select1, EVolume.v10p);
                // do nothing more for now...
            }
        }
        else
        {
            Debug.LogWarning("GameManager: m_infantryBarracksFactory is null, cannot create building");
        }
    }

    public static void CreateSwordInfantryAtPos(Vector3 x_centeredCellPos)
    {
        if (m_swordInfantrymanFactory != null)
        {
            Player currentPlayer = GetPlayerCurrentlyPlaying();
            uint goldAmount = SwordInfantryman.GetGoldTrainingCost();
            uint foodAmount = SwordInfantryman.GetFoodTrainingCost();
            if (currentPlayer.TryPayGoldAndFoodAmounts(goldAmount, foodAmount))
            {
                AudioManager.PlayFXSound(FXSoundID.select2);
                SwordInfantryman swordInf = m_swordInfantrymanFactory.MakeSwordInfantryman(
                    x_centeredCellPos, currentPlayer.GetPlayerNb());
                swordInf.SetCreatedThisTurn(true);
                currentPlayer.AddOwnedUnit(swordInf);
            }
            else
            { // not enough resources to pay the required amounts
                AudioManager.PlayFXSound(FXSoundID.select2, EVolume.v10p);
                // do nothing more for now...
            }
        }
        else
        {
            Debug.LogWarning("GameManager: m_swordInfantrymanFactory is null, cannot create swordInfantry");
        }
    }

}
