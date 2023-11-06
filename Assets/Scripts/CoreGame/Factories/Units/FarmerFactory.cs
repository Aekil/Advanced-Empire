using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerFactory : T_PooledObjFactory<FarmerFactory, Farmer>
{
    //public List<Farmer> m_allFarmers = new List<Farmer>();


    // MonoBehaviour inherited "event methods" :
    protected override void Awake() // inherited from T_Singleton<...>
    {
        base.Awake();
        m_nbObjectsFromStart = 15;
        m_nbObjectsToExtend = 10;
    }

    protected override void Start() // inherited from T_PooledObjFactory<...>
    {
        base.Start();
        // ...
    }


    // Own methods :
    public Farmer MakeFarmer()
    {
        Farmer farmer = null;
        if (m_objPool != null)
        {
            Debug.Log(name + " : m_objPool is not null, MakeFarmer() should work");
            farmer = m_objPool.Get();

            // /!\ components that might already exist in prefab, don't need to be added here (at least without checks)

            //m_allFarmers.Add(farmer); // useful list ?
        }
        else
        {
            Debug.LogError(name + " : m_objPool is null, MakeFarmer() failed");
        }
        return farmer;
    }


    // /!\ Note (deprecated ?) : when recycle method will be called in Card class,
    // we should encounter an issue because Card doesn't know Recycle...
    // multiple solutions, choose one to implement...
    // example at minut25 of '8PoolingGestionMemoire' video from Atelier Unity GameCodeur "monstres"
    // (with abstract class for Card and inheritance..)

    /*public static void RecycleInfantryman(Infantryman x_Infantryman)
    {
        _instance.m_InfantrymansPool.Recycle(x_Infantryman);
    }*/
    public void RecycleFarmer(Farmer x_farmer)
    {
        // both works : 
        //m_objPool.Recycle(x_farmer);
        _instance.m_objPool.Recycle(x_farmer);
        // so : should we use "_instance." here or not ?
    }

    // intended to work with m_allInfantrymans (new list) but never fully implemented/tested... :
    /*public static void DeleteCard(Card x_card)
    {
        _instance.m_allCards.Remove(x_card);
        x_card.enabled = false;
    }*/
}
