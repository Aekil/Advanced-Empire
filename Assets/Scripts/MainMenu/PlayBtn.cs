using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayBtn : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
        AudioManager.PlayFXSound(FXSoundID.startGame);
    }
}
