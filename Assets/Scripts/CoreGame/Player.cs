using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerNb { Unknown, One, Two };

public class Player : MonoBehaviour
{
    public PlayerNb m_playerNb;
    public uint m_startingGoldAmount = 50; // can be overriden in Unity Editor
    public uint m_startingFoodAmount = 10; // can be overriden in Unity Editor

    private bool m_isPlaying = false;
    private uint m_goldAmount;
    private uint m_foodAmount;
    private List<A_Man> m_ownedUnits = new();


    void Start()
    {
        //Debug.Log(name + " : Start() called");
        m_goldAmount = m_startingGoldAmount;
        m_foodAmount = m_startingFoodAmount;
    }

    void Update()
    {
        
    }


    // Own methods :
    public PlayerNb GetPlayerNb()
    {
        return m_playerNb;
    }
    
    public bool getIsPlaying()
    {
        return m_isPlaying;
    }
    public void setIsPlaying(bool x_isPlaying)
    {
        m_isPlaying = x_isPlaying;
    }

    // -- Player Resources Management :

    // --- GOLD Resource :
    public uint GetGoldAmount()
    {
        return m_goldAmount;
    }
    public void SetGoldAmount(uint x_newGoldAmount)
    {
        m_goldAmount = x_newGoldAmount;
        TextPlayerResources.SetNewGoldAmountToDisplay(m_goldAmount);
    }
    public void AddGoldAmount(uint x_goldAmountToAdd)
    {
        SetGoldAmount(m_goldAmount + x_goldAmountToAdd);
    }
    public bool TryPayGoldAmount(uint x_goldAmountToPay)
    {
        bool ret = false;
        // /!\ find a better & more secure way to do that...
        int payResult = (int)m_goldAmount - (int)x_goldAmountToPay;
        if (payResult >= 0)
        {
            SetGoldAmount(m_goldAmount - x_goldAmountToPay);
            ret = true;
        }
        else
        {
            Debug.Log("Player #" + m_playerNb + " : not enough gold to pay the required amount of : " + x_goldAmountToPay);
        }
        return ret;
    }

    // --- FOOD Resource :
    public uint GetFoodAmount()
    {
        return m_foodAmount;
    }
    public void SetFoodAmount(uint x_newFoodAmount)
    {
        m_foodAmount = x_newFoodAmount;
        TextPlayerResources.SetNewFoodAmountToDisplay(m_foodAmount);
    }
    public void AddFoodAmount(uint x_foodAmountToAdd)
    {
        SetFoodAmount(m_foodAmount + x_foodAmountToAdd);
    }
    public bool TryPayFoodAmount(uint x_foodAmountToPay)
    {
        bool ret = false;
        // /!\ find a better & more secure way to do that...
        int payResult = (int)m_foodAmount - (int)x_foodAmountToPay;
        if (payResult >= 0)
        {
            SetFoodAmount(m_foodAmount - x_foodAmountToPay);
            ret = true;
        }
        else
        {
            Debug.Log("Player #" + m_playerNb + " : not enough food to pay the required amount of : " + x_foodAmountToPay);
        }
        return ret;
    }

    // --- MIXED Resources :
    public bool TryPayGoldAndFoodAmounts(uint x_goldAmountToPay, uint x_foodAmountToPay)
    {
        bool ret = false;
        // /!\ find a better & more secure way to do that...
        int goldPayResult = (int)m_goldAmount - (int)x_goldAmountToPay;
        int foodPayResult = (int)m_foodAmount - (int)x_foodAmountToPay;
        if (goldPayResult >= 0 && foodPayResult >= 0)
        {
            SetGoldAmount(m_goldAmount - x_goldAmountToPay);
            SetFoodAmount(m_foodAmount - x_foodAmountToPay);
            ret = true;
        }
        else
        {
            Debug.Log("Player #" + m_playerNb + " : not enough resources to pay the required amounts of : "
                + x_goldAmountToPay + "gold and " + x_foodAmountToPay + "food");
        }
        return ret;
    }

    public void AddOwnedUnit(A_Man x_newUnit)
    {
        m_ownedUnits.Add(x_newUnit);
    }
    public bool RemoveOwnedUnit(A_Man x_unitToRemove)
    {
        return m_ownedUnits.Remove(x_unitToRemove);
    }

}
