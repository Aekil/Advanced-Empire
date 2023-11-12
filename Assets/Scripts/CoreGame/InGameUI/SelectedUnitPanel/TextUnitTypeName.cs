using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextUnitTypeName : MonoBehaviour
{
    public TextMeshProUGUI m_unitTypeNameTextMPUI;

    private static string m_unitTypeText;
    private string m_lastFrameText;
    private const string m_FARMER_TEXT = "Farmer";
    private const string m_INFANTRY_TEXT = "Infantry";
    private const string m_UNKNOWN_TEXT = "Unknown UnitType";

    private void Start()
    {
        if (m_unitTypeNameTextMPUI == null)
        {
            Debug.LogError(name + " : 'TextMeshProUGUI m_unitTypeNameTextMPUI' ref is null");
        }
        m_lastFrameText = string.Empty;
    }

    private void Update()
    {
        if (m_unitTypeText != m_lastFrameText)
        {
            m_unitTypeNameTextMPUI.text = m_unitTypeText;
            m_lastFrameText = m_unitTypeText;
        }
    }


    // Own methods :
    public static void SetUnitTypeToDisplay(UnitType x_unitType)
    {
        switch (x_unitType)
        {
            case UnitType.Farmer:
                m_unitTypeText = m_FARMER_TEXT;
                break;
            case UnitType.Infantry:
                m_unitTypeText = m_INFANTRY_TEXT;
                break;
            /*case UnitType.Archery:
                m_unitTypeText = m_BARRACKS_TEXT;
                break;
            case UnitType.Cavalry:
                m_unitTypeText = m_BARRACKS_TEXT;
                break;
            case UnitType.Seat:
                m_unitTypeText = m_BARRACKS_TEXT;
                break;*/
            case UnitType.Unknown:
            default:
                m_unitTypeText = m_UNKNOWN_TEXT;
                Debug.LogWarning("TextUnitTypeName.cs : Unknown UnitType");
                break;
        }
    }
}
