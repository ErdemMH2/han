using HAN.Lib.Structure;
using System;

namespace HAN.Lib.Basic 
{
    /**
     * Signal parameter for property changes. It has a reference to the property, which caused this signal. 
     * Additionaly the value is copied to this parameter.
     */
    public class PropertyChangedSignalParam< T > : BasicSignalParameter
    {
        public static readonly Key k_PropertyChangedSignalParam = KeyFactory.Create( "PropertyChangedSignalParam" );

        /**
         * Copy of the value, that was changed.
         */
        public T Value { get; private set; }

        /**
         * Reference to the property, that was changed.
         */
        public Property< T > Property { get; private set; }

        /**
         * Type of the property.
         */
        public override MetaType Type()
        {
            if( m_type == null ) { 
                m_type = new MetaType( k_PropertyChangedSignalParam, Property.Id );
            }

            return m_type;
        }


        public PropertyChangedSignalParam( Property< T > a_property, ISignalPublisher a_publisher )
          : base( a_publisher )
        {
            Property = a_property;
            Value = a_property.Value;   // copies value, it could be changed after processing the signal, so you have the original here
        }
    }


    /**
     * Property with a onChanged signal, that is emitted, when the encapsulated value is changed.
     * The property will not do bookkeeping about subscribers to this signal. This has to be done
     * from the property owner.
     */
    public class SignalizingProperty<T> : Property<T>, ISignalPublisher
    {
        public static readonly Key k_SignalizingProperty = KeyFactory.Create( "SignalizingProperty" );
        public override MetaType Type() { return new MetaType( k_SignalizingProperty ); }

        protected IHANObject m_owner;  // publisher of this

        private Signal m_onChangedSignal; // RAII

        public SignalizingProperty( Key a_id, T a_val, IHANObject a_owner )
          : base( a_id, a_val )
        {
            m_owner = a_owner;
        }


        /**
         * Tries to set the value, if same type
         * if value is changed a changed signal is emitted.
         */
        public override bool TrySetValue( object a_val )
        {
            bool isSet = false;

            if( !object.Equals( Value, a_val ) )
            {
                isSet = rawTrySetValue( a_val );

                if( isSet )
                {
                    emitChangedSignal();
                }
            }

            return isSet;
        }


        /**
         * Set the value, if same type
         * if value is changed a changed signal is emitted.
         */
        public override void SetValue( T a_val )
        {
            if( !System.Collections.Generic.EqualityComparer<T>.Default.Equals( Value, a_val ) )
            {
                rawSetValue( a_val );
                emitChangedSignal();
            }
        }


        /**
         * Connect to this on changed signal. Will be emited when the value has changed.
         */
        public bool Connect( ISignalSubscriber a_subscriber, Key a_signal, Action<BasicSignalParameter> a_slot )
        {
            if( m_onChangedSignal == null ) {
                m_onChangedSignal = new Signal( m_owner );
            }

            return m_onChangedSignal.Connect( a_subscriber, a_signal, a_slot );
        }


        /**
         * Disconnect from this on changed signal.
         */
        public bool Disconnect( ISignalSubscriber a_subscriber, Key a_signal, Action<BasicSignalParameter> a_slot )
        {
            if( m_onChangedSignal == null ) {
                return false;
            }

            return m_onChangedSignal.Disconnect( a_subscriber, a_signal, a_slot );
        }


        /**
         * Disconnet all subscribers.
         */
        public void DisconnectEstablishedConnections()
        {
            if( m_onChangedSignal != null ) {
                m_onChangedSignal.DisconnectEstablishedConnections();
            }
        }


        /**
         * Emits onChangedSignal with a_newValue as new Value
         */
        protected void emitChangedSignal()
        {
            if( m_onChangedSignal != null ) {
                m_onChangedSignal.Emit( new PropertyChangedSignalParam<T>( this, m_owner ) );
            }
        }


        /**
         * TrySet override is slow, but needed SignalizingProperty,
         * so we declare a reference to the not overwritten TrySetValue
         */
        protected bool rawTrySetValue( object a_val ) {
            return base.TrySetValue( a_val );
        }


        /**
         * SetValue override is slow, but needed SignalizingProperty,
         * so we declare a reference to the not overwritten SetValue
         */
        protected void rawSetValue( T a_val ) {
            base.SetValue( a_val );
        }

    }
}