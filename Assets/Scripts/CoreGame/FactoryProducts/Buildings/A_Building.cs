using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class A_Building : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;
    private BoxCollider2D m_boxCollider;

    protected PlayerNb m_playerOwnerNb = PlayerNb.Unknown;
    protected BuildingType m_buildingType = BuildingType.Unknown;

    protected uint m_maxHP;
    protected uint m_currentHP;


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

    protected virtual void Start()
    {
        //Debug.Log("Start() called on " + gameObject.name);
    }

    protected virtual void Update()
    {
        // ...
    }

    protected virtual void OnEnable()
    {
        //Debug.Log("OnEnable() called on " + gameObject.name);
    }

    protected virtual void OnDisable()
    {
        //Debug.Log("OnDisable() called on " + gameObject.name);
    }


    // Own methods :
    protected abstract void BuildingInit();

    public PlayerNb GetPlayerOwnerNb()
    {
        return m_playerOwnerNb;
    }
    public void SetPlayerOwnerNb(PlayerNb x_playerNb)
    {
        m_playerOwnerNb = x_playerNb;
    }

    public BuildingType GetBuildingType()
    {
        return m_buildingType;
    }
    protected void SetBuildingType(BuildingType x_buildingType)
    {
        m_buildingType = x_buildingType;
    }

    public uint GetMaxHP()
    {
        return m_maxHP;
    }
    public uint GetCurrentHP()
    {
        return m_currentHP;
    }
    protected void SetCurrentHP(uint x_newCurrentHP)
    {
        m_currentHP = x_newCurrentHP;
    }
    public void DamageBuilding(uint x_damagePoints)
    {
        SetCurrentHP(m_currentHP - x_damagePoints);
    }

    
}
