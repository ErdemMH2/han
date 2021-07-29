using System.Collections.Generic;
using HAN.Lib.Basic;
using HAN.Lib.Structure;


namespace HAN.Lib.Stat
{
    /**
     *  This StatSet will adapt itself according the overwritten updateStats function.
     *  This function will be called if any of the registered StatSets or Stats changes.
     *  The AdaptinStatSet will depend on the value of the registered Stats.
     */
    public abstract class AbstractAdaptingStatSet : StatSet
    {
        protected List<IHANObject> m_connectedHObjects = new List<IHANObject>();


        public AbstractAdaptingStatSet( Key a_id ) 
          : base( a_id )
        {
        }


        /**
         * Connects this StatSet to a IHANObject and calls updateStats if any change occurs.
         * <param name="a_set">IHANObject with a ChangedSignal, when properties were changed</param>
         */
        public bool Connect( IHANObject a_set )
        {
            if( !m_connectedHObjects.Contains( a_set ) ) 
            {
                if( a_set.Connect( this, Basic.Keys.Signal.ChangedSignal, updateStats ) )  
                {
                    m_connectedHObjects.Add( a_set );
                    return true;
                }
            }

            return false;
        }


        /**
         * Disconnect this StatSet from a IHANObject
         * <param name="a_set">Connected IHANObject</param>
         */
        public bool Disconnect( IHANObject a_set )
        {
            if( m_connectedHObjects.Contains( a_set ) ) 
            {
                if( a_set.Disconnect( this, Basic.Keys.Signal.ChangedSignal, updateStats ) )
                {
                    m_connectedHObjects.Remove( a_set );
                    return true;
                }
            }

            return false;
        }


        protected abstract void updateStats( BasicSignalParameter a_param );
    }
}