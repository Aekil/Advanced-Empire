using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoriesManager : T_Singleton<FactoriesManager>
{
    // add here in public all needed factories (with getter), and set ref from Unity Editor
    public InfantryBarracksFactory m_infantryBarracksFactory = null;
    public SwordInfantrymanFactory m_swordInfantrymanFactory = null;
    

    override protected void Awake()
    {
        base.Awake();
        transform.parent = null; // make sure it is a "root level gameObject" (to avoid being destroyed unintentionally by its parent)
        DontDestroyOnLoad(this.gameObject);

        if (m_infantryBarracksFactory == null)
        {
            Debug.LogError(name + " : m_infantryBarracksFactory is null");
        }
        if (m_swordInfantrymanFactory == null)
        {
            Debug.LogError(name + " : m_swordInfantrymanFactory is null");
        }
    }

    private void Start()
    {
        //Debug.Log("FactoriesManager start()");
    }

    // Own methods :
    public InfantryBarracksFactory GetInfantryBarracksFactory()
    {
        return m_infantryBarracksFactory;
    }
    public SwordInfantrymanFactory GetSwordInfantrymanFactory()
    {
        return m_swordInfantrymanFactory;
    }
}
