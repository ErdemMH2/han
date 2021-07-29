using System;
using UnityEngine;

namespace HAN.Lib.Structure.Unity
{
    public class GameObjectPool
    {
        protected Pool m_pool;
        private Transform m_defaultParent;


        public GameObjectPool( Pool a_pool
                             , Transform a_defaultParent = null )
        {
            m_pool = a_pool;
            m_defaultParent = a_defaultParent;
        }


        public GameObjectPoolEntry Retrieve( GameObjectId a_id )
        {
            var poolEntry = m_pool.Retrieve( a_id, createFunction( a_id.GameObject ) );
            return poolEntry as GameObjectPoolEntry;
        }


        protected Func<IPoolEntry> createFunction( GameObject a_prefab )
        {
            return () => {
                var go = GameObject.Instantiate( a_prefab, m_defaultParent );
                var prefabEntry = go.GetComponent<GameObjectPoolEntry>();
                if( prefabEntry == null )
                {
                    prefabEntry = go.AddComponent<GameObjectPoolEntry>();
                }

                return prefabEntry;
            };
        }
    }
}