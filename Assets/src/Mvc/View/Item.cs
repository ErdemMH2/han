using UnityEngine;
using HAN.Lib.Basic;
using HAN.Lib.Structure;


namespace HAN.Lib.Mvc
{
    /** 
     * A Item is a specialized HComponent with transform properties.
     * It is the base renderable UI HComponent.
     */
    public class Item : HComponent
    {
        public static readonly Key k_Item = KeyFactory.Create( "Item" );

        public Vector3 Position { 
            get { return (m_position != null ) ? m_position.Value : Vector3.zero; }
            set { if( m_position != null ) m_position.SetValue(value); }
        }
        
        public Quaternion Rotation { 
            get { return ( m_rotation != null ) ? m_rotation.Value : Quaternion.identity; }
            set { if( m_rotation != null ) m_rotation.SetValue( value ); }
        }

        public Vector3 Scale { 
            get { return ( m_scale != null ) ? m_scale.Value : Vector3.one; }
            set { if( m_scale != null ) m_scale.SetValue( value ); }
        }


        /// Local position
        protected SignalizingProperty< Vector3 > m_position;

        /// Local rotation.
        protected SignalizingProperty< Quaternion > m_rotation;

        /// Local scale
        protected SignalizingProperty< Vector3 > m_scale;


        public Item( Key a_id )
            : base( a_id )
        {
            // transforms
            m_position = new SignalizingProperty<Vector3>( Keys.Transform.Position, Vector3.zero, this );
            AddProperty( m_position );

            m_rotation = new SignalizingProperty<Quaternion>( Keys.Transform.Rotation, Quaternion.identity, this );
            AddProperty( m_rotation );

            m_scale = new SignalizingProperty<Vector3>( Keys.Transform.Scale, Vector3.one, this );
            AddProperty( m_scale );
        }


        public void InitTransforms( Vector3 a_position, Quaternion a_rotation, Vector3 a_scale )
        {
            m_position.SetValue( a_position );
            m_rotation.SetValue( a_rotation );
            m_scale.SetValue( a_scale );
        }


        public void Rotate( float a_deg, bool a_clockwise = true )
        {

        }
    }
}
