using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BtnsCreateBuilding : MonoBehaviour
{
    // Own methods :
    public void CreateInfantryBarracks()
    {
        // (i) assume here that GetSingleSelectedCellPos() call returns the good value,
        //      and that when we can click on the button linked to this func,
        //      the good conditions for this function to be run are gathered.
        Vector3Int selectedCellPos = GameMap.GetSingleSelectedCellPos();
        Vector3 centeredCellPos = GameMap.GetCenteredCellPos(selectedCellPos);
        GameManager.CreateInfantryBarracksAtPos(centeredCellPos);
        GameManager.GetGameMap().DisableBuildingCreationPanel();
    }

    public void CreateWall()
    {
        // not implemented yet
    }
}

