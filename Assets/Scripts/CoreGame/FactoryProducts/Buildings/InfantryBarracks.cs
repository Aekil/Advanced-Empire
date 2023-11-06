using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InfantryBarracks : A_Barracks
{
    private A_InfantrymanFactory m_infantryFactory = null;
    private Tile m_tile;
    protected static uint m_infBarrGoldCreateCost = 20;

    // MonoBehaviour inherited "event methods" :
    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        InitSprite();
        BuildingInit();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }


    // A_Barracks inherited methods :
    protected override void BuildingInit()
    {
        base.BuildingInit();
        m_barracksType = BarracksType.InfantryBarracks;
    }


    // Own methods :
    private void InitSprite()
    {
        if (m_tile != null)
        {
            SpriteRenderer spriteR = GetComponent<SpriteRenderer>();
            spriteR.sprite = m_tile.sprite;
        }
        else
        {
            Debug.LogWarning(name + " : m_tile is null");
        }
    }

    public void SetInfantryFactory(A_InfantrymanFactory x_factory)
    {
        m_infantryFactory = x_factory;
    }

    public A_InfantrymanFactory GetInfantryFactory()
    {
        /*if (m_infantryFactory == null)
        {
            Debug.LogWarning(name + " : GetInfantryFactory() is returning null...");
        }*/
        return m_infantryFactory;
    }

    public void SetTile(Tile x_tile)
    {
        m_tile = x_tile;
    }

    public Tile GetTile()
    {
        return m_tile;
    }

    public static uint GetGoldCreateCost()
    {
        return m_infBarrGoldCreateCost;
    }
}
