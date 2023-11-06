using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBuildingHP : MonoBehaviour
{
    public TextMeshProUGUI m_buildingHPTextMPUI;

    private static uint m_maxHP = 0;
    private uint m_lastFrameMaxHP = 0;
    private static uint m_currentHP = 0;
    private uint m_lastFrameCurrentHP = 0;
    

    private void Start()
    {
        if (m_buildingHPTextMPUI != null)
        {
            UpdateBuildingHPText();
        }
        else
        {
            Debug.LogError(name + " : 'TextMeshProUGUI m_buildingHPTextMPUI' ref is null");
        }
    }

    private void Update()
    {
        if (m_maxHP != m_lastFrameMaxHP ||
            m_currentHP != m_lastFrameCurrentHP)
        {
            UpdateBuildingHPText();
            m_lastFrameMaxHP = m_maxHP;
            m_lastFrameCurrentHP = m_currentHP;
        }
    }


    // Own methods :
    public static void SetBuildingHPToDisplay(uint x_currentHP, uint x_maxHP)
    {
        m_maxHP = x_maxHP;
        m_currentHP = x_currentHP;
    }

    private void UpdateBuildingHPText()
    {
        m_buildingHPTextMPUI.text = "HP : " + m_currentHP + " / " + m_maxHP;
    }
}
