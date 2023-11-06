using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBuildingCreation : MonoBehaviour
{
    private bool m_isActivated = false;


    // Own methods :
    public void Activate()
    {
        enabled = true;
        gameObject.SetActive(true);
        m_isActivated = true;
    }
    public void Deactivate()
    {
        enabled = false;
        gameObject.SetActive(false);
        m_isActivated = false;
    }

    public bool IsActivated()
    {
        return m_isActivated;
    }
}
