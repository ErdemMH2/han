using HAN.Lib.Basic;
using HAN.Lib.Structure;
using System.Collections.Generic;

namespace HAN.Lib.Mvc
{
    /**
     * Main binding for view components. Every view component will have a HAN specific component.
     * By using HComponents in our code we are able to transfer changes to the view. 
     * HComponents are created in the code and can be used as usual HObjects. 
     * HComponentHBehavior are created in the Unity Editor and will provide access to a Unity object.
     * The Id will be used to pair code components and view componens. The Id per pair and parent have to be unique.
     * This is the Unity specific view component. 
     */
    public class HComponent : HObject
    {
        public static readonly Key k_HComponent = KeyFactory.Create( "HComponent" );
        public static Keys.ComponentKeys k_Keys { get { return Keys.Component; } }

        /// Unique Id per parent and pair
        public virtual Key Id { get { return m_id.Value; } }


        /// Is the object enabled
        public bool ObjectEnabled
        {
            get { return ( m_objectEnabled != null ) ? m_objectEnabled.Value : false; }
            set { if( m_objectEnabled != null ) m_objectEnabled.SetValue( value ); }
        }

         
        /// Is the component enabled
        public bool Enabled
        {
            get { return ( m_enabled != null ) ? m_enabled.Value : false; }
            set { if( m_enabled != null ) m_enabled.SetValue( value ); }
        }

        /// unique key of Component
        private ReadonlyProperty<Key> m_id;

        /// Active state of component
        protected SignalizingProperty< bool > m_enabled;

        /// Active state of object
        protected SignalizingProperty< bool > m_objectEnabled;

        /// change signal
        protected Signal m_onChangedSignal;

        /// destroy signal
        protected Signal m_onDestroySignal;


        public HComponent( Key a_id )
        {
            m_id = new ReadonlyProperty<Key>( k_Keys.Id, a_id );
            AddProperty( m_id );
            
            m_onChangedSignal = new Signal( this );
            addSignal( Basic.Keys.Signal.ChangedSignal, m_onChangedSignal );

            m_onDestroySignal = new Signal( this );
            addSignal( k_Keys.ObjectDestroyed, m_onDestroySignal );

            m_enabled = new SignalizingProperty<bool>( k_Keys.Enabled, true, this );
            AddProperty( m_enabled );

            m_objectEnabled = new SignalizingProperty<bool>( k_Keys.ObjectEnabled, true, this );
            AddProperty( m_objectEnabled );
        }


        public override MetaType Type()
        {
            if( m_type == null ) {
                m_type = new MetaType( k_HComponent, Id );
            }

            return m_type;
        }


        public override bool Equals( object a_other )
        {
            HComponent other = a_other as HComponent;

            if( other != null )
            {
                #if UNITY_EDITOR && false
                // here we check the references, as it is easy to forget to generate an unique ID
                // but this has to be removed for productive code
                if( Id == other.Id && !System.Object.ReferenceEquals( this, other )) {
                    HAN.Debug.Logger.Error( Id,
                                            string.Format( "Id is same, but reference is different on {0} and {1}",
                                            Id, other.Id ) );
                }
                #endif

                return Id == other.Id;
            }

            return false;
        }


        public override int GetHashCode()
        {
            return base.GetHashCode() << Id.GetHashCode();
        }


        /**
         * Initializes HComponent.
         * <param name="a_enabled">Is the component enabled.</param>
         */
        public void InitComponent( bool a_enabled, bool a_objectEnabled )
        {
            m_enabled.SetValue( a_enabled );
            m_objectEnabled.SetValue( a_objectEnabled );
        }


        /**
         * Will add all ISignalPublisher based Properties to the change singal of this.
         */
        public override bool AddProperty( IProperty a_property )
        {
            bool added = base.AddProperty( a_property );
            if( added && a_property is ISignalPublisher ) { // TODO: Check if this works: Do we realy cut the connection in RemoveProperty?
                Connect( this, a_property.Id, ( BasicSignalParameter a_param ) => { propertyChangedSlot( a_param ); } );
            }

            return added; 
        }

        /**
         * Will signilize if any property has changed.
         */
        private void propertyChangedSlot( BasicSignalParameter a_param )
        {
            m_onChangedSignal.Emit( a_param );
        }


        /**
         * Emits onChangedSignal with a_newValue as new Value
         */
        protected void emitChangedSignal()
        {
            m_onChangedSignal.Emit( new BasicSignalParameter( this ) );
        }

        /**
         * Emits m_onDestroySignal
         */
        public void Destroy()
        {
            m_onDestroySignal.Emit();
        }
    }
}