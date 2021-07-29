using HAN.Lib.Basic;
using System;
using UnityEngine;

namespace HAN.Lib.Structure.Unity
{
    public class SingleGameObjectPool : GameObjectPool
    {
        private IncrementalId m_prefabId;
        private Func<IPoolEntry> m_createFunc;


        public SingleGameObjectPool( Pool a_pool
                                   , GameObject a_prefab
                                   , IncrementalId a_gameObjectId = null
                                   , Transform a_defaultParent = null )
          : base( a_pool, a_defaultParent )
        {
            if( a_gameObjectId != null ) {
                m_prefabId = a_gameObjectId;
            }
            else {
                m_prefabId = new GameObjectId( a_prefab );
            }
            m_createFunc = createFunction( a_prefab );
        }


        public GameObjectPoolEntry Retrieve()
        {
            var poolEntry = m_pool.Retrieve( m_prefabId, m_createFunc );
            return poolEntry as GameObjectPoolEntry;
        }
    }
}