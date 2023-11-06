using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GameMap : MonoBehaviour
{
    private static Grid m_uGrid;
    private Tilemap[] m_tilemaps = null;
    // (i) TilemapIndex values below should be coherent with sortingOrder setted for each of these tilemaps in Unity Editor
    private enum TilemapIndex { WalkableGround = 0, UnwalkableGround = 1, GroundElements = 2, SelectArea = 3 };
    public int m_selectAreaSortingOrder = 9; // (i) default value, can be overrided in Unity Editor
    // Regular directions :
    private Vector3Int m_topCellVec = new(0, 1, 0);
    private Vector3Int m_bottomCellVec = new(0, -1, 0);
    private Vector3Int m_leftCellVec = new(-1, 0, 0);
    private Vector3Int m_rightCellVec = new(1, 0, 0);

    public Tile m_whiteSelectTile = null;
    public Tile m_blueSelectTile = null;
    public Tile m_greenSelectTile = null;
    public Tile m_redSelectTile = null;
    public Tile m_forestElemTile = null;
    public Tile m_moutainElemTile = null;
    public uint m_forestReducedRange = 0; // (i) default value, can be overrided in Unity Editor
    public uint m_moutainReducedRange = 1; // (i) default value, can be overrided in Unity Editor

    public ActivablePanel m_buildingCreationPanel = null;
    public ActivablePanel m_selectedBuildingPanel = null;
    public ActivablePanel m_selectedUnitPanel = null;

    private bool m_isMoveRangeSelectAreaActive = false;

    private bool m_isSingleSelectedCellAreaActive = false;
    private static Vector3Int m_singleSelectedCellPos;

    private InfantryBarracksFactory m_infantryBarracksFactory = null;
    private A_InfantrymanFactory m_infantrymanFactory = null;

    private readonly bool m_debugBuildingMode = true;

#if UNITY_ANDROID
    private Touch m_touch;
    //private Vector3 m_touchPos;
    private bool m_touchMoved = false;
    private bool m_evSysGOTouched = false;
#endif


    void Start()
    {
        //Debug.Log("gameMap start()");
        if (InitGrid())
        {
            if (VerifyTilemapsInit())
            {
                if ((m_infantryBarracksFactory = GameManager.GetInfantryBarracksFactory()) == null)
                {
                    Debug.LogError(name + " : m_infantryBarracksFactory is null");
                }
                if ((m_infantrymanFactory = GameManager.GetSwordInfantrymanFactory()) == null)
                {
                    Debug.LogError(name + " : m_infantrymanFactory is null");
                }
            }
        }
        if (m_buildingCreationPanel == null)
        {
            Debug.LogError(name + " : m_buildingCreationPanel ref is null");
        }
    }

    void Update()
    {
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            m_touch = Input.GetTouch(0);
            if (m_touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(m_touch.fingerId))
                {
                    m_evSysGOTouched = true;
                }
            }
            if (m_touch.phase == TouchPhase.Moved)
            {
                m_touchMoved = true;
                // to be improved, but good enough for now...
                Vector2 deltaMove = -(m_touch.deltaPosition / 3 * m_touch.deltaTime);
                Camera.main.transform.localPosition += (Vector3)deltaMove;
            }
            if (m_touch.phase == TouchPhase.Ended)
            {
                if (m_touchMoved)
                {
                    m_touchMoved = false;
                }
                else
                {
                    if (m_evSysGOTouched)
                    {
                        m_evSysGOTouched = false;
                    }
                    else
                    {
                        Vector3 touchPos = Camera.main.ScreenToWorldPoint(m_touch.position);
                        Vector3Int cellPos = m_uGrid.WorldToCell(touchPos);

                        if (m_debugBuildingMode)
                        {
                            CheckSelectedCell(cellPos);
                        }
                    }
                }
            }
        }
        // EASY & QUICK ~ Debug & Test purpose
        else if (m_debugBuildingMode && Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 mouseWorldPos = Utils.GetMouseWorldPosition();
                Vector3Int cellPos = m_uGrid.WorldToCell(mouseWorldPos);
                CheckSelectedCell(cellPos);
            }
        }
#endif
    }

    private bool InitGrid()
    {
        bool ret = false;
        m_uGrid = FindAnyObjectByType<Grid>();
        if (m_uGrid != null)
        {
            ret = true;
        }
        else
        {
            Debug.LogError("Cannot get grid component, m_uGrid is null");
        }
        return ret;
    }

    private bool VerifyTilemapsInit()
    {
        bool ret = false;
        bool walkableTmExist = false, unwalkableTmExist = false,
            elementsTmExist = false, selectAreaTmExist = false;
        m_tilemaps = m_uGrid.GetComponentsInChildren<Tilemap>();
        foreach (Tilemap tilemap in m_tilemaps)
        {
            TilemapRenderer tmR = tilemap.GetComponent<TilemapRenderer>();
            if (tmR.sortingOrder == (int)TilemapIndex.WalkableGround)
            { // 0 == WalkableGround tilemap
                Debug.Log("WalkableGround tilemap found");
                walkableTmExist = true;
            }
            else if (tmR.sortingOrder == (int)TilemapIndex.UnwalkableGround)
            { // 1 == UnwalkableGround tilemap
                Debug.Log("UnwalkableGround tilemap found");
                unwalkableTmExist = true;
            }
            else if (tmR.sortingOrder == (int)TilemapIndex.GroundElements)
            { // 2 == GroundElements tilemap
                Debug.Log("GroundElements tilemap found");
                elementsTmExist = true;
            }
            else if (tmR.sortingOrder == m_selectAreaSortingOrder)
            { // 9 == SelectArea tilemap
                Debug.Log("SelectArea tilemap found");
                selectAreaTmExist = true;
                tilemap.gameObject.SetActive(false);
            }
            else
            { // ? == unknown tilemap
                Debug.LogWarning("unknown (and unexpected) tilemap found");
            }
        }
        if (walkableTmExist && unwalkableTmExist && elementsTmExist && selectAreaTmExist)
        {
            ret = true;
        }
        else
        {
            Debug.LogError("Some required tilemaps are not found");
        }
        return ret;
    }

    private void CheckSelectedCell(Vector3Int x_cellPos)
    {
        Vector3 centeredCellPos = GetCenteredCellPos(x_cellPos);

        if (m_isSingleSelectedCellAreaActive)
        {
            if (IsSingleCellAreaSelected(x_cellPos) == false)
            {
                DisableAllActivatedPanels();
                DisableSingleSelectedCellArea();
            }
        }
        else if (m_isMoveRangeSelectAreaActive) // "MoveRange Selection Area" gameplay
        {
            Tile selectingTile;
            if (IsMoveRangeAreaCellSelected(x_cellPos, out selectingTile))
            {
                OnMoveRangeAreaCellSelected(centeredCellPos, selectingTile);
            }
            DisableMoveRangeSelectArea();
        }
        else
        { // "Classic Turn" gameplay

            // check if something is selected
            if (IsUnitSelected(centeredCellPos))
            {
                OnSelectedUnit(x_cellPos);
            }
            else if (IsBuildingSelected(centeredCellPos, out A_Building selectedBuilding))
            {
                Player currentPlayer = GameManager.GetPlayerCurrentlyPlaying();
                PlayerNb currentPlayerNb = currentPlayer.GetPlayerNb();
                if (selectedBuilding.GetPlayerOwnerNb() == currentPlayerNb)
                {
                    EnableSelectedBuildingPanel(x_cellPos);
                    TextBuildingTypeName.SetBuildingTypeToDisplay(selectedBuilding.GetBuildingType());
                    TextBuildingHP.SetBuildingHPToDisplay(selectedBuilding.GetCurrentHP(), selectedBuilding.GetMaxHP());

                    // temporary (while selectedBuildingPanel has the CreateUnit btn...) :
                    if (selectedBuilding.GetBuildingType() == BuildingType.Barracks)
                    {
                        A_Barracks selectedBarr = selectedBuilding as A_Barracks;
                        if (selectedBarr.GetBarracksType() == BarracksType.InfantryBarracks)
                        {
                            InfantryBarracks selectedInfBarr = selectedBarr as InfantryBarracks;
                            // assume here that infantryBarracs only creates SwordInf for now...
                            TextCreateUnit.SetUnitResourcesCostToDisplay(
                                SwordInfantryman.GetGoldTrainingCost(), SwordInfantryman.GetFoodTrainingCost());
                        }
                    }
                }

                // /!\ OLD LOGIC (deprecated for now, but code can still be useful elsewhere) :
                /*
                switch (selectedBuildingType)
                {
                    case BuildingType.Barracks:
                        onSelectedBarracks(centeredCellPos);
                        break;
                    case BuildingType.Farm:
                        // ...
                        break;
                    case BuildingType.Wall:
                        // ...
                        break;
                    case BuildingType.Unknown:
                    default:
                        Debug.LogWarning(name + " : Unknown BuildingType");
                        break;
                }*/
            }
            else if (m_debugBuildingMode) // no unit and no building on selected cell...
            {
                Tilemap unwalkTm = m_tilemaps[(int)TilemapIndex.UnwalkableGround];
                Tilemap elemsTm = m_tilemaps[(int)TilemapIndex.GroundElements];
                Tile unwalkTile = unwalkTm.GetTile<Tile>(x_cellPos);
                Tile elemsTile = elemsTm.GetTile<Tile>(x_cellPos);
                if (unwalkTile == null && elemsTile == null)
                { // if no unwalkableTile and no elem on it

                    // (i) display a selectCell with UI button to create building
                    //EnableSingleSelectedCellArea(x_cellPos);
                    EnableBuildingCreationPanel(x_cellPos);
                    AudioManager.PlayFXSound(FXSoundID.select1, EVolume.v50p);
                }
                // else : do not build on unwalkableGround nor on GroundElements tiles
            }
        }
    }

    private bool IsUnitSelected(Vector3 x_selectCellPos)
    {
        bool ret = false;
        if (m_infantrymanFactory != null &&
            m_infantrymanFactory.TrySelectUnitAtPos(x_selectCellPos))
        {
            ret = true;
        }
        return ret;
    }
    private bool IsUnitAtPos(Vector3 x_selectCellPos, out A_Unit x_unit)
    {
        bool ret = false;
        x_unit = null;
        if (m_infantrymanFactory != null &&
            m_infantrymanFactory.IsUnitAtPos(x_selectCellPos, out x_unit))
        {
            ret = true;
        }
        return ret;
    }
    private bool IsEnemyUnitAtPos(Vector3Int x_lCellPos)
    {
        bool ret = false;
        if (IsUnitAtPos(GetCenteredCellPos(x_lCellPos), out A_Unit unit))
        {
            if (unit.GetPlayerOwnerNb() != GameManager.GetPlayerNbCurrentlyPlaying())
            { // enemy unit
                SetProperSelectCellOnMoveRangeArea(x_lCellPos);
                ret = true;
            }
        }
        return ret;
    }

    private bool IsBuildingSelected(Vector3 x_selectCellPos, out BuildingType x_selectedBuildingType)
    {
        bool ret = false;
        x_selectedBuildingType = BuildingType.Unknown;
        if (m_infantryBarracksFactory != null &&
            m_infantryBarracksFactory.TrySelectBuildingAtPos(x_selectCellPos, out x_selectedBuildingType))
        {
            ret = true;
        }
        return ret;
    }
    private bool IsBuildingSelected(Vector3 x_selectCellPos, out A_Building x_selectedBuilding)
    {
        bool ret = false;
        x_selectedBuilding = null;
        if (m_infantryBarracksFactory != null &&
            m_infantryBarracksFactory.TrySelectBuildingAtPos(x_selectCellPos, out x_selectedBuilding))
        {
            ret = true;
        }
        return ret;
    }
    private bool IsBuildingAtPos(Vector3 x_selectCellPos, out A_Building x_building)
    {
        bool ret = false;
        x_building = null;
        if (m_infantryBarracksFactory != null &&
            m_infantryBarracksFactory.IsBuildingAtPos(x_selectCellPos, out x_building))
        {
            ret = true;
        }
        return ret;
    }


    // DEPRECATED FUNC (but some code can still be useful)
    private void onSelectedBarracks(Vector3 x_selectedCenteredCellPos)
    {
        A_Building selectedBuilding = m_infantryBarracksFactory.GetLastSelectedBuilding();
        if (selectedBuilding != null)
        {
            Player currentPlayer = GameManager.GetPlayerCurrentlyPlaying();
            PlayerNb currentPlayerNb = currentPlayer.GetPlayerNb();
            if (selectedBuilding.GetPlayerOwnerNb() == currentPlayerNb)
            {
                //Debug.Log("selected building is owned by current player");
                A_Barracks barr = selectedBuilding as A_Barracks;
                BarracksType barracksType = barr.GetBarracksType();
                switch (barracksType)
                {
                    // TO RETHINK : code architecture here should change,
                    // when we will have multiple types of infantry to create inside one type of barracks;
                    // also it will be triggered by a UI button "onClick" func, instead of here directly on selected cell...

                    case BarracksType.InfantryBarracks:
                        InfantryBarracks infBarr = barr as InfantryBarracks;
                        SwordInfantrymanFactory infFact = infBarr.GetInfantryFactory() as SwordInfantrymanFactory;
                        if (infFact != null)
                        {
                            // /!\ temporary magic values
                            uint goldAmount = 10;
                            uint foodAmount = 2;

                            // TO THINK : how does it cost in resource (and which resource(s) ?) to train a swordInf ?
                            // and what about other units ? (think about "in-game resources economy")

                            //if (currentPlayer.TryPayGoldAmount(goldAmount))
                            if (currentPlayer.TryPayGoldAndFoodAmounts(goldAmount, foodAmount))
                            { // goldAmount successfully payed
                                AudioManager.PlayFXSound(FXSoundID.select2);
                                SwordInfantryman swordInf = infFact.MakeSwordInfantryman(x_selectedCenteredCellPos, currentPlayerNb);
                                swordInf.SetCreatedThisTurn(true);
                            }
                            else
                            { // not enough resources to pay the required amounts
                                AudioManager.PlayFXSound(FXSoundID.select2, EVolume.v10p);
                                // do nothing more for now...
                            }
                        }
                        break;
                    case BarracksType.ArcheryBarracks:
                        break;
                    case BarracksType.CavalryBarracks:
                        break;
                    case BarracksType.UnknownBarracksType:
                    default:
                        Debug.LogWarning("Unknown barracks type");
                        break;
                }
            }
            else
            {
                // (i) do nothing for now... (but we might do things here later)
                //Debug.Log(name + " : trying to select enemy building");
            }
        }
        else
        {
            Debug.LogError(name + " : got selectedBuilding as null");
        }
    }

    private void OnSelectedUnit(Vector3Int x_cellPos)
    {
        A_Unit selectedUnit = m_infantrymanFactory.GetLastSelectedUnit();
        if (selectedUnit.GetPlayerOwnerNb() == GameManager.GetPlayerNbCurrentlyPlaying())
        {
            if (selectedUnit.WasCreatedThisTurn() ||
                selectedUnit.HasMovedThisTurn())
            {
                // cannot move this turn
                AudioManager.PlayFXSound(FXSoundID.select2, EVolume.v25p);
            }
            else // can move this turn
            {
                AudioManager.PlayFXSound(FXSoundID.select2);
                EnableMoveRangeSelectArea(x_cellPos, selectedUnit.GetMoveRange());
            }
        }
    }

    private void EnableSingleSelectedCellArea(Vector3Int x_cellPos)
    {
        //m_buildingCreationPanel.Activate();
        m_singleSelectedCellPos = x_cellPos;
        m_isSingleSelectedCellAreaActive = true;
        m_tilemaps[(int)TilemapIndex.SelectArea].gameObject.SetActive(true);
        m_tilemaps[(int)TilemapIndex.SelectArea].SetTile(x_cellPos, m_whiteSelectTile);
    }
    public void DisableSingleSelectedCellArea()
    {
        //m_buildingCreationPanel.Deactivate();
        m_isSingleSelectedCellAreaActive = false;
        m_tilemaps[(int)TilemapIndex.SelectArea].ClearAllTiles();
        m_tilemaps[(int)TilemapIndex.SelectArea].gameObject.SetActive(false);
    }

    private void EnableBuildingCreationPanel(Vector3Int x_cellPos)
    {
        m_buildingCreationPanel.Activate();
        EnableSingleSelectedCellArea(x_cellPos);
    }
    public void DisableBuildingCreationPanel()
    {
        m_buildingCreationPanel.Deactivate();
        DisableSingleSelectedCellArea();
    }

    private void EnableSelectedBuildingPanel(Vector3Int x_cellPos)
    {
        m_selectedBuildingPanel.Activate();
        EnableSingleSelectedCellArea(x_cellPos);
    }
    public void DisableSelectedBuildingPanel()
    {
        m_selectedBuildingPanel.Deactivate();
        DisableSingleSelectedCellArea();
    }

    public void DisableAllActivatedPanels()
    {
        if (m_selectedBuildingPanel.IsActivated())
        {
            m_selectedBuildingPanel.Deactivate();
        }
        if (m_buildingCreationPanel.IsActivated())
        {
            m_buildingCreationPanel.Deactivate();
        }
    }


    private void EnableMoveRangeSelectArea(Vector3Int x_cellPos, uint x_moveRange)
    {
        m_isMoveRangeSelectAreaActive = true;
        m_tilemaps[(int)TilemapIndex.SelectArea].gameObject.SetActive(true);
        DisplayMoveRangeSelectArea(x_cellPos, x_moveRange);
    }
    private void DisableMoveRangeSelectArea()
    {
        m_isMoveRangeSelectAreaActive = false;
        m_tilemaps[(int)TilemapIndex.SelectArea].ClearAllTiles();
        m_tilemaps[(int)TilemapIndex.SelectArea].gameObject.SetActive(false);
    }

    private void DisplayMoveRangeSelectArea(Vector3Int x_cellPos, uint x_moveRange)
    {
        SetSelectCells(x_cellPos, x_moveRange, m_topCellVec);
        SetSelectCells(x_cellPos, x_moveRange, m_bottomCellVec);
        SetSelectCells(x_cellPos, x_moveRange, m_leftCellVec);
        SetSelectCells(x_cellPos, x_moveRange, m_rightCellVec);
    }

    private bool IsMoveRangeAreaCellSelected(Vector3Int x_cellPos, out Tile x_SelectingTile)
    {
        bool ret = false;
        x_SelectingTile = m_tilemaps[(int)TilemapIndex.SelectArea].GetTile<Tile>(x_cellPos);
        if (x_SelectingTile != null)
        {
            ret = true;
        }
        return ret;
    }
    private bool IsSingleCellAreaSelected(Vector3Int x_cellPos/*, out Tile x_SelectingTile*/)
    {
        bool ret = false;
        Tile x_SelectingTile = m_tilemaps[(int)TilemapIndex.SelectArea].GetTile<Tile>(x_cellPos);
        if (x_SelectingTile != null)
        {
            ret = true;
        }
        return ret;
    }

    public static Vector3Int GetSingleSelectedCellPos()
    {
        return m_singleSelectedCellPos;
    }

    private void OnMoveRangeAreaCellSelected(Vector3 x_centeredCellPos, Tile x_selectingTile)
    {
        //Debug.Log("moveRange Area Cell is selected !");
        if (x_selectingTile == m_whiteSelectTile ||
            x_selectingTile == m_blueSelectTile)
        {
            //Debug.Log("white or blue cell selected");

            // TO DO : add a "turn" limitation to avoid being able to move the same unit multiple times a turn

            A_Unit selectedUnit = m_infantrymanFactory.GetLastSelectedUnit();
            if (selectedUnit.transform.position != x_centeredCellPos)
            {
                // move the selected unit to selected cell inside moveRange area
                selectedUnit.transform.position = x_centeredCellPos;
                selectedUnit.SetMovedThisTurn(true);
                // ? : set a variable inside unit instance to remember that it already moved this turn ?
                //Debug.Log("unit moved");
            }

            // TO THINK & TO DO : if blue, how to -> process the building action related to the blue cell ?
        }
        else if (x_selectingTile == m_greenSelectTile)
        {
            // (i) greenSelect means cell with a friendly unit on it, so cannot move to it
            Debug.Log("unit cannot move to a green cell (with another friendly unit on it)");
            // do nothing
        }
        else if (x_selectingTile == m_redSelectTile)
        {
            // TO THINK : redSelect means cell with a enemy unit on it, how to manage this ?
            // can move to start a fight, but means only the unit that wins can stay ?
            // or we can only move in front of an enemy unit, to start fight (but no needs to kill one unit this turn)
            Debug.Log("unit cannot move to a red cell (with an enemy unit on it)");
            // ...
        }
    }

    public static Vector3 GetCenteredCellPos(Vector3Int x_cellPos)
    {
        Vector3 centeredCellPos = x_cellPos;
        centeredCellPos += (m_uGrid.cellSize / 2);
        //centeredCellPos = centeredCellPos * m_uGrid.transform.localScale;
        return centeredCellPos;
    }

    private void SetSelectCells(Vector3Int x_lCellPos, uint x_moveRange, Vector3Int x_gapRule)
    {
        for (uint i = 0; i < x_moveRange; i++)
        {
            x_lCellPos += x_gapRule;
            if (m_tilemaps[(int)TilemapIndex.UnwalkableGround].GetTile(x_lCellPos) != null)
            { // unwalkable
                break; // don't allow to move on this cell and beyond
            }
            /*else if (IsUnitAtPos(GetCenteredCellPos(x_lCellPos), out A_Unit man))
            {
                if (man.GetPlayerOwnerNb() != GameManager.GetPlayerNbCurrentlyPlaying())
                { // enemy unit
                    SetProperSelectCellOnMoveRangeArea(x_lCellPos);
                    break; // don't allow to move beyond
                }
            }*/
            else if (IsEnemyUnitAtPos(x_lCellPos))
            {
                break; // don't allow to move beyond
            }
            else
            {
                x_moveRange = CheckGroundElementsReducingRange(x_lCellPos, x_moveRange);

                SetProperSelectCellOnMoveRangeArea(x_lCellPos);

                // manage all "new dirs"
                if ((int)i < (int)x_moveRange - 1) // /!\ need to cast to int to avoid infinite recursive loop issue
                {
                    uint nextMoveRange = x_moveRange - (i + 1);
                    if (nextMoveRange > 0)
                    {
                        if (x_gapRule == m_topCellVec)
                        {
                            SetSelectCells(x_lCellPos, nextMoveRange, m_leftCellVec);
                            SetSelectCells(x_lCellPos, nextMoveRange, m_rightCellVec);
                        }
                        else if (x_gapRule == m_bottomCellVec)
                        {
                            SetSelectCells(x_lCellPos, nextMoveRange, m_leftCellVec);
                            SetSelectCells(x_lCellPos, nextMoveRange, m_rightCellVec);
                        }
                        else if (x_gapRule == m_leftCellVec)
                        {
                            SetSelectCells(x_lCellPos, nextMoveRange, m_topCellVec);
                            SetSelectCells(x_lCellPos, nextMoveRange, m_bottomCellVec);
                        }
                        else if (x_gapRule == m_rightCellVec)
                        {
                            SetSelectCells(x_lCellPos, nextMoveRange, m_topCellVec);
                            SetSelectCells(x_lCellPos, nextMoveRange, m_bottomCellVec);
                        }
                    }
                }
            }
        }
    }

    private uint CheckGroundElementsReducingRange(Vector3Int x_lCellPos, uint x_moveRange)
    {
        Tile elem;
        if ((elem = m_tilemaps[(int)TilemapIndex.GroundElements].GetTile<Tile>(x_lCellPos)) != null)
        { // groundElements tile found
            if (elem == m_moutainElemTile)
            { // reduce moveRange for mountain
                x_moveRange -= m_moutainReducedRange;
            }
            else if (elem == m_forestElemTile)
            {
                Debug.Log("ForestElem tile found");
                x_moveRange -= m_forestReducedRange;
            }
        }
        return x_moveRange;
    }

    private void SetProperSelectCellOnMoveRangeArea(Vector3Int x_lCellPos)
    {
        Tile properSelectCell = m_whiteSelectTile; // white is default
        Vector3 centeredCellPos = GetCenteredCellPos(x_lCellPos);
        if (IsUnitAtPos(centeredCellPos, out A_Unit unit))
        {
            if (unit.GetPlayerOwnerNb() == GameManager.GetPlayerNbCurrentlyPlaying())
            { // friendly unit (relative to player currently playing)
                properSelectCell = m_greenSelectTile;
            }
            else
            { // enemy unit
                properSelectCell = m_redSelectTile;
            }
        }
        else if (IsBuildingAtPos(centeredCellPos, out A_Building building))
        {
            if (building != null)
            {
                if (building.GetBuildingType() != BuildingType.Unknown)
                {
                    if (building.GetPlayerOwnerNb() == GameManager.GetPlayerNbCurrentlyPlaying())
                    {
                        // TO DO : in a 2nd time, only show blue cell for "available action on building",
                        // for example a farm or gold mine with no unit on it...
                        properSelectCell = m_blueSelectTile;
                    }
                    else
                    {
                        properSelectCell = m_redSelectTile;
                    }
                }
                else
                {
                    Debug.LogWarning(name + " : trying to set a selectCell on a building with an unknown BuildingType");
                }
            }
            else
            {
                Debug.LogWarning(name + " : IsBuildingAtPos(...) returned true, but 'out building' param is returned null");
            }
        }
        m_tilemaps[(int)TilemapIndex.SelectArea].SetTile(x_lCellPos, properSelectCell);
    }


}
