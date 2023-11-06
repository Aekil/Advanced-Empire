using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPlayerResources : MonoBehaviour
{
    public TextMeshProUGUI m_playerResourcesTMPUI;

    private const string m_FOOD_TEXT = "Food : ";
    private static uint m_foodAmountToDisplay;
    private uint m_lastFrameFoodATD;

    private const string m_GOLD_TEXT = "Gold : ";
    private static uint m_goldAmountToDisplay;
    private uint m_lastFrameGoldATD;


    void Start()
    {
        if (m_playerResourcesTMPUI != null)
        {
            m_lastFrameFoodATD = 0;
            m_foodAmountToDisplay = 0;
            //UpdateFoodText();
            m_lastFrameGoldATD = 0;
            m_goldAmountToDisplay = 0;
            //UpdateGoldText();
            UpdateResourcesText();
        }
        else
        {
            Debug.LogError(name + " : 'TextMeshProUGUI m_playerResourcesTMPUI' ref is null");
        }
    }

    void Update()
    {
        if (m_foodAmountToDisplay != m_lastFrameFoodATD ||
            m_goldAmountToDisplay != m_lastFrameGoldATD)
        {
            //UpdateGoldText();
            UpdateResourcesText();
            m_lastFrameFoodATD = m_foodAmountToDisplay;
            m_lastFrameGoldATD = m_goldAmountToDisplay;
        }
    }

    /*private void UpdateFoodText()
    {
        m_playerResourcesTMPUI.text = m_FOOD_TEXT + m_foodAmountToDisplay.ToString();
    }*/
    public static void SetNewFoodAmountToDisplay(uint x_newFoodAmountToDisplay)
    {
        m_foodAmountToDisplay = x_newFoodAmountToDisplay;
    }


    /*private void UpdateGoldText()
    {
        m_playerResourcesTMPUI.text = m_GOLD_TEXT + m_goldAmountToDisplay.ToString();
    }*/
    public static void SetNewGoldAmountToDisplay(uint x_newGoldAmountToDisplay)
    {
        m_goldAmountToDisplay = x_newGoldAmountToDisplay;
    }

    private void UpdateResourcesText()
    {
        m_playerResourcesTMPUI.text =
            m_FOOD_TEXT + m_foodAmountToDisplay.ToString() + " | " +
            m_GOLD_TEXT + m_goldAmountToDisplay.ToString();
    }
}
