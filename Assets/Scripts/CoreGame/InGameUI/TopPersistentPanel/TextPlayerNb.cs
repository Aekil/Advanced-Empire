using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPlayerNb : MonoBehaviour
{
    public TextMeshProUGUI m_playerNbTMPUI;
    
    private static PlayerNb m_playerNb;
    private PlayerNb m_lastFramePlayerNb;
    private const string m_PLAYER_ONE_TEXT = "Player 1";
    private const string m_PLAYER_TWO_TEXT = "Player 2";

    void Start()
    {
        if (m_playerNbTMPUI != null)
        {
            m_playerNb = PlayerNb.One;
            m_lastFramePlayerNb = PlayerNb.One;
            m_playerNbTMPUI.text = m_PLAYER_ONE_TEXT;
        }
        else
        {
            Debug.LogError(name + " : 'TextMeshProUGUI m_playerNbTMPUI' ref is null");
        }
    }

    void Update()
    {
        if (m_playerNb != m_lastFramePlayerNb)
        {
            if (m_playerNb == PlayerNb.One)
            {
                m_playerNbTMPUI.text = m_PLAYER_ONE_TEXT;
            }
            else if (m_playerNb == PlayerNb.Two)
            {
                m_playerNbTMPUI.text = m_PLAYER_TWO_TEXT;
            }
            else
            {
                Debug.LogWarning(name + " : Unknown playerNb");
                m_playerNbTMPUI.text = "Unknown Player";
            }
            m_lastFramePlayerNb = m_playerNb;
        }
    }

    public static void SetNewPlayerNb(PlayerNb x_newPlayerNb)
    {
        if (m_playerNb != x_newPlayerNb)
        {
            m_playerNb = x_newPlayerNb;
        }
        else
        {
            Debug.LogWarning("TextPlayerNb.cs : SetNewPlayerNb(...) called, " +
                "but the given playerNb in param is the same playerNb that the current one");
        }
    }
}
