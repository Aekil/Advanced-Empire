using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class A_Unit : A_MonoBehaviourPoolableObject
{
    private SpriteRenderer m_spriteRenderer;
    private BoxCollider2D m_boxCollider;

    protected PlayerNb m_playerOwnerNb;
    protected bool m_movedThisTurn;
    protected bool m_createdThisTurn;

    protected UnitType m_unitType;

    protected uint m_maxHP;
    protected uint m_currentHP;
    protected uint m_baseAttack;
    protected uint m_attackRange;
    protected uint m_moveRange;
    //protected uint m_vision;

    protected const uint m_DEFAULT_UNIT_ATK_RANGE = 3;
    protected const uint m_DEFAULT_UNIT_MOVE_RANGE = 3;


    // IPoolableObject inherited methods :
    public override void Init()
    {
        //Debug.Log("Init() called on " + gameObject.name);
        gameObject.SetActive(true);
        m_playerOwnerNb = PlayerNb.Unknown;
        m_movedThisTurn = false;
        m_createdThisTurn = true; // try with false
        m_unitType = UnitType.Unknown;
        ManInit();
        return;
    }

    public override void Reset()
    {
        //Debug.Log("Reset() called on " + gameObject.name);
        gameObject.SetActive(false);
        return;
    }


    // MonoBehaviour inherited "event methods" :
    protected virtual void Awake()
    {
        //Debug.Log("Awake() called on " + gameObject.name);
        if (!TryGetComponent<SpriteRenderer>(out m_spriteRenderer))
        {
            Debug.LogWarning("cannot get existing SpriteRenderer in " + gameObject.name + ", using the default one");
            if ((m_spriteRenderer = gameObject.AddComponent<SpriteRenderer>()) == null)
            {
                Debug.LogError("SpriteRenderer component is null on " + gameObject.name);
            }
        }
        if (!TryGetComponent<BoxCollider2D>(out m_boxCollider))
        {
            Debug.LogWarning("cannot get existing BoxCollider2D in " + gameObject.name + ", using the default one");
            if ((m_boxCollider = gameObject.AddComponent<BoxCollider2D>()) == null)
            {
                Debug.LogError("BoxCollider2D component is null on " + gameObject.name);
            }
        }
    }

    protected override void Start()
    {
        // ...
    }

    protected override void Update()
    {
        // ...
    }


    // Own methods :
    public UnitType GetUnitType()
    {
        return m_unitType;
    }

    protected virtual void ManInit()
    {
        m_maxHP = 0;
        m_currentHP = 0;
        m_baseAttack = 0;
        m_attackRange = m_DEFAULT_UNIT_ATK_RANGE;
        m_moveRange = m_DEFAULT_UNIT_MOVE_RANGE;
    }

    public PlayerNb GetPlayerOwnerNb()
    {
        return m_playerOwnerNb;
    }
    public void SetPlayerOwnerNb(PlayerNb x_playerNb)
    {
        m_playerOwnerNb = x_playerNb;
    }

    public bool HasMovedThisTurn()
    {
        return m_movedThisTurn;
    }
    public void SetMovedThisTurn(bool x_movedThisTurn)
    {
        m_movedThisTurn = x_movedThisTurn;
    }

    public bool WasCreatedThisTurn()
    {
        return m_createdThisTurn;
    }
    public void SetCreatedThisTurn(bool x_createdThisTurn)
    {
        m_createdThisTurn = x_createdThisTurn;
    }

    public uint GetMoveRange()
    {
        return m_moveRange;
    }

}
