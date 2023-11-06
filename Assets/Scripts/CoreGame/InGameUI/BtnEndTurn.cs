using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnEndTurn : MonoBehaviour
{
    public void EndTurn()
    {
        // (i) need to call 'EndCurrentTurn()' before getting 'newCurrentPlayer'
        GameManager.EndCurrentTurn();
        Player newCurrentPlayer = GameManager.GetPlayerCurrentlyPlaying();
        TextPlayerNb.SetNewPlayerNb(newCurrentPlayer.GetPlayerNb());
        // text resource update still needed here ?
        TextPlayerResources.SetNewFoodAmountToDisplay(newCurrentPlayer.GetFoodAmount());
        TextPlayerResources.SetNewGoldAmountToDisplay(newCurrentPlayer.GetGoldAmount());

        AudioManager.PlayFXSound(FXSoundID.clickOnBtn1);
    }
}
