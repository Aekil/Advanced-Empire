using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerFactory : A_UnitFactory
{
    /*public List<Farmer> m_allFarmers = new List<Farmer>();

    // Own methods :
    public Farmer MakeFarmer()
    {
        Farmer farmer = null;
        if (m_objPool != null)
        {
            Debug.Log(name + " : m_objPool is not null, MakeFarmer() should work");
            farmer = m_objPool.Get() as Farmer;

            // /!\ components that might already exist in prefab, don't need to be added here (at least without checks)

            m_allFarmers.Add(farmer); // useful list ?
        }
        else
        {
            Debug.LogError(name + " : m_objPool is null, MakeFarmer() failed");
        }
        return farmer;
    }*/
}
