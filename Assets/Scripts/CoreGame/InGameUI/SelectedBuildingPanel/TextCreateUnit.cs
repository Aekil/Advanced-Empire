using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextCreateUnit : MonoBehaviour
{
    public TextMeshProUGUI m_createUnitTMPUI;

    private static uint m_goldCost = 0;
    private uint m_lastFrameGoldCost = 0;
    private static uint m_foodCost = 0;
    private uint m_lastFrameFoodCost = 0;

    private void Start()
    {
        if (m_createUnitTMPUI != null)
        {
            UpdateText();
        }
        else
        {
            Debug.LogError(name + " : 'TextMeshProUGUI m_createUnitTMPUI' ref is null");
        }
    }

    private void Update()
    {
        if (m_goldCost != m_lastFrameGoldCost ||
            m_foodCost != m_lastFrameFoodCost)
        {
            UpdateText();
            m_lastFrameGoldCost = m_goldCost;
            m_lastFrameFoodCost = m_foodCost;
        }
    }

    // Own methods :
    private void UpdateText()
    {
        m_createUnitTMPUI.text = "<b>Create Unit</b>\n(Cost : "
            + m_goldCost.ToString() + " Gold & " + m_foodCost.ToString() + " Food)";
    }

    public static void SetUnitResourcesCostToDisplay(uint x_goldCost, uint x_foodCost)
    {
        m_goldCost = x_goldCost;
        m_foodCost = x_foodCost;
    }
}
