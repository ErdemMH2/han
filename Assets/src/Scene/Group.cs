using HAN.Lib.Basic;
using HAN.Lib.Mvc;
using HAN.Lib.Structure;
using System.Collections.Generic;

namespace HAN.Lib.Scene
{
    /**
     * If group is visible, all children have to be visible
     * Will not prevent changing the visiblity of any child, 
     * but set it when its own visiblity is changed
     */
    public class Group : HComponent
    {
        protected List<HComponent> m_children = new List<HComponent>();


        public Group( Key a_id, IEnumerable<HComponent> a_children ) 
          : base( a_id )
        {
            foreach( var child in a_children )
            {
                Add( child );
            }

            m_objectEnabled.Connect( this, null, onGroupEnabledSlot );
        }


        protected virtual void onGroupEnabledSlot( BasicSignalParameter a_param )
        {
            foreach( var child in m_children )
            {
                child.ObjectEnabled = m_objectEnabled.Value;
            }
        }


        public virtual void Add( HComponent a_child )
        {
            m_children.Add( a_child );
        }
    }
}