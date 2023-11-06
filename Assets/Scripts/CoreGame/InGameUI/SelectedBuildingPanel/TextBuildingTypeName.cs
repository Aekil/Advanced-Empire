using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBuildingTypeName : MonoBehaviour
{
    public TextMeshProUGUI m_buildingTypeNameTextMPUI;

    private static string m_buildingTypeText;
    private string m_lastFrameText;
    private const string m_BARRACKS_TEXT = "Barracks";
    private const string m_FARM_TEXT = "Farm";
    private const string m_WALL_TEXT = "Wall";
    private const string m_UNKNOWN_TEXT = "Unknown BuildingType";

    private void Start()
    {
        if (m_buildingTypeNameTextMPUI == null)
        {
            Debug.LogError(name + " : 'TextMeshProUGUI m_buildingTypeNameTextMPUI' ref is null");
        }
        m_lastFrameText = string.Empty;
    }

    private void Update()
    {
        if (m_buildingTypeText != m_lastFrameText)
        {
            m_lastFrameText = m_buildingTypeText;
            m_buildingTypeNameTextMPUI.text = m_buildingTypeText;
        }
    }


    // Own methods :
    public static void SetBuildingTypeToDisplay(BuildingType x_buildingType)
    {
        switch (x_buildingType)
        {
            case BuildingType.Barracks:
                m_buildingTypeText = m_BARRACKS_TEXT;
                break;
            case BuildingType.Farm:
                m_buildingTypeText = m_FARM_TEXT;
                break;
            case BuildingType.Wall:
                m_buildingTypeText = m_WALL_TEXT;
                break;
            case BuildingType.Unknown:
            default:
                m_buildingTypeText = m_UNKNOWN_TEXT;
                Debug.LogWarning("TextBuildingTypeName.cs : Unknown BuildingType");
                break;
        }
    }
}
