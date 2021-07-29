using HAN.Lib.Basic;
using UnityEngine;

namespace HAN.Lib.Structure.Unity
{
    /**
     * Interface to reset and return pool entries to the pool
     */
    public class GameObjectPoolEntry : MonoBehaviour, IPoolEntry
    {
        [SerializeField] private bool m_returnToParentTransform = true;

        [SerializeField] private bool m_activateSelf = true;
        [SerializeField] private bool m_deactivateOnReset = true;

        public IncrementalId PoolId { get; private set; }
        private Pool m_parent;
        private Transform m_initialParent;


        public void Activate()
        {
            if( m_activateSelf ) {
                gameObject.SetActive( true );
            }
        }


        public void ResetIt()
        {
            if( m_deactivateOnReset ) { 
                gameObject.SetActive( false );
            }
        }


        public void InitPoolEntry( IncrementalId a_poolId, Pool a_parent )
        {
            m_initialParent = transform.parent;
            PoolId = a_poolId;
            m_parent = a_parent;
        }


        public virtual void ReturnToPool()
        {
            if( m_returnToParentTransform ) { 
                transform.SetParent( m_initialParent, false );
            }
            m_parent.Return( this );
        }
    }
}