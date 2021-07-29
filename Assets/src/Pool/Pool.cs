using HAN.Lib.Basic;
using System;
using System.Collections.Generic;

namespace HAN.Lib.Structure
{
    /**
     * Pool for organizing IPoolEntry's 
     * Will retrieve and return IcrementalId based.
     */
    public class Pool
    {
        private Dictionary<IncrementalId, Queue<IPoolEntry>> m_pool 
                                = new Dictionary<IncrementalId, Queue<IPoolEntry>>();


        public void Return( IPoolEntry a_item )
        {
            if( !m_pool.TryGetValue( a_item.PoolId, out var queue ) )
            {
                queue = new Queue<IPoolEntry>();
                m_pool.Add( a_item.PoolId, queue );
            }

            if( !queue.Contains( a_item ) ) 
            { 
                a_item.ResetIt();
                queue.Enqueue( a_item );
            }
        }


        public IPoolEntry Retrieve( IncrementalId a_id, Func<IPoolEntry> a_create )
        {
            if( m_pool.TryGetValue( a_id, out var queue ) )
            {
                if( queue.Count > 0 )
                {
                    var poolEntry = queue.Dequeue();
                    poolEntry.Activate();
                    return poolEntry;
                }
            }

            var newEntry = a_create();
            newEntry.InitPoolEntry( a_id, this );
            newEntry.Activate();
            return newEntry;
        }
    }
}