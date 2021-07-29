using HAN.Lib.Basic;
using HAN.Lib.Mvc;
using HAN.Lib.Structure;
using System.Collections.Generic;

namespace HAN.Lib.Scene
{
    /**
     * If one member gets visible all other members will be set to invisible
     */
    public class ExclusiveGroup : Group
    {
        public ExclusiveGroup( Key a_id, IEnumerable<HComponent> a_children )
            : base( a_id, a_children )
        {
        }


        protected override void onGroupEnabledSlot( BasicSignalParameter a_param )
        {
        }


        public override void Add( HComponent a_child )
        {
            base.Add( a_child );
            a_child.Connect( this, Mvc.Keys.Component.ObjectEnabled, (BasicSignalParameter) => { onChildEnableChangedSlot( a_child ); } );
        }


        protected void onChildEnableChangedSlot( HComponent a_child )
        {
            // if we got visible, make the other invisible
            // will also prevent recursion
            if( a_child.ObjectEnabled )
            { 
                foreach( var child in m_children )
                {
                    if( child != a_child )
                    {
                        child.ObjectEnabled = false;
                    }
                }
            }
        }
    }
}