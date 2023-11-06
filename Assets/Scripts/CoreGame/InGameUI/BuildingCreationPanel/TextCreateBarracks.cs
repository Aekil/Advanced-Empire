using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextCreateBarracks : MonoBehaviour
{
    public TextMeshProUGUI m_createBarracksTMPUI;

    public uint m_goldCost = 30;

    void Start()
    {
        if (m_createBarracksTMPUI != null)
        {
            InitCreateBarracksText();
        }
        else
        {
            Debug.LogError(name + " : 'TextMeshProUGUI m_createBarracksTMPUI' ref is null");
        }
    }

    // Own methods :
    private void InitCreateBarracksText()
    {
        m_createBarracksTMPUI.text = "<b>Create Barracks</b>\n(Cost : " + m_goldCost.ToString() + " Gold)";
    }
}
