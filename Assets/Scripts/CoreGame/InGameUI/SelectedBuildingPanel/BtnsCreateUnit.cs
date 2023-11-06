using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BtnsCreateUnit : MonoBehaviour
{
    // Own methods :
    public void CreateSwordInfantry()
    {
        // (i) assume here that GetSingleSelectedCellPos() call returns the good value,
        //      and that when we can click on the button linked to this func,
        //      the good conditions for this function to be run are gathered.
        Vector3Int selectedCellPos = GameMap.GetSingleSelectedCellPos();
        Vector3 centeredCellPos = GameMap.GetCenteredCellPos(selectedCellPos);
        GameManager.CreateSwordInfantryAtPos(centeredCellPos);
        //GameManager.GetGameMap().DisableUnitCreationPanel();
        GameManager.GetGameMap().DisableSelectedBuildingPanel(); // temporary, while this button is inside "selectedBuildingPanel"
    }

}

