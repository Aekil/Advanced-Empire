using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteToggle : MonoBehaviour
{
    public Toggle m_muteToggle;


    private void Start()
    {
        if (m_muteToggle == null)
        {
            Debug.LogError(name + " : m_muteToggle is null");
        }
        else
        {
            if (AudioManager.IsAudioMute())
            {
                m_muteToggle.SetIsOnWithoutNotify(true);
            }
            else
            {
                m_muteToggle.SetIsOnWithoutNotify(false);
            }
        }
    }

    public void ToggleValueChanged()
    {
        if (m_muteToggle.isOn)
        {
            Debug.Log("toggle isON : mute global system");
            AudioManager.MuteGlobalAudioSystem();
        }
        else
        {
            Debug.Log("toggle isOFF : unmute global system");
            AudioManager.UnmuteGlobalAudioSystem();
        }
    }
}
