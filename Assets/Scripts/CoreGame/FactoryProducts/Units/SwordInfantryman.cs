using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SwordInfantryman : A_Infantryman
{
    private Tile m_tile;
    private static uint m_swordInfGoldCreateCost = m_aInfGoldCreateCost;
    private static uint m_swordInfFoodCreateCost = m_aInfFoodCreateCost;
    private static uint m_swordInfFoodMaintainCost = m_aInfFoodMaintainCost;


    // IPoolableObject inherited methods :
    public override void Init()
    {
        base.Init();
    }

    public override void Reset()
    {
        base.Reset();
    }


    // MonoBehaviour inherited "event methods" :
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        InitSprite();
        SwordInfantrymanInit();
    }

    protected override void Update()
    {
        base.Update();
    }


    // A_Infantryman inherited methods :
    // ...

    // Own methods :
    private void InitSprite()
    {
        if (m_tile != null)
        {
            SpriteRenderer spriteR = GetComponent<SpriteRenderer>();
            spriteR.sprite = m_tile.sprite;
        }
    }

    protected void SwordInfantrymanInit()
    {
        base.InfantrymanInit();
    }

    public void SetTile(Tile x_tile)
    {
        m_tile = x_tile;
    }
    public Tile GetTile()
    {
        return m_tile;
    }

    public static uint GetGoldTrainingCost()
    {
        return m_swordInfGoldCreateCost;
    }
    public static uint GetFoodTrainingCost()
    {
        return m_swordInfFoodCreateCost;
    }

    public static uint GetFoodMaintainCost()
    {
        return m_swordInfFoodMaintainCost;
    }
}
