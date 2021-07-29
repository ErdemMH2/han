using HAN.Lib.Structure;
using System;

namespace HAN.Lib.Basic
{
    /**
     * MonoBehavior replacement, which proxies HObject.
     */
    public abstract class HObjectMonoBehavior : UnityEngine.MonoBehaviour
                                              , IHANProxy
    {
        /**
         * Default implementation for HObject, that can be proxied
         * by HObjectMonoBehavior. It will provide access to signals and slots.
         */
        public class DefaultHObject : HObject
        {
            public GenericConnectionBookkeeper GenericConnectionBookkeeper { get { return m_connectionBookkeeper; } }

            public override MetaType Type()
            {
                if( m_type == null ) {
                    m_type = new MetaType( k_HObjectMonoBehavior );
                }

                return m_type;
            }

            public void AddSignal( Key a_id, Signal a_signal ){
                addSignal( a_id, a_signal );
            }
        }

        public static readonly Key k_HObjectMonoBehavior = KeyFactory.Create( "HObjectMonoBehavior" );

        /**
         * Proxied HObject; have to be overwritten by sub class.
         */
        public abstract IHANObject HObject { get; }


        /**
         *  Signal and Slots for C#. Connect to signal of HObject. In HObject this are signals of properties.
         *  Derivations can also add custom signals.
         *  Only one subscribtion is allowed to each signal with the same slot. 
         *  <param name="a_subscriber">Subscriber of the signal. Will be notfied if signal is not available anymore.</param>
         *  <param name="a_signal">key of the signal</param>
         *  <param name="a_slot">Slot to be connected to signal.</param>
         */
        public bool Connect( ISignalSubscriber a_subscriber, Key a_signal, Action<BasicSignalParameter> a_slot )
        {
            if( HObject != null ) {
                return HObject.Connect( a_subscriber, a_signal, a_slot );
            }

            return false;
        }



        /**
         *  Disconnect from signal.The a_subscriber is not interested in our signal anymore.
         *  <param name="a_subscriber">Subscriber of the signal, that will deregister slot from signal</param>
         *  <param name="a_signal">key of the signal</param>
         *  <param name="a_slot">Slot, that is not connected to us anymore/param>
         */
        public bool Disconnect( ISignalSubscriber a_subscriber, Key a_signal, Action<BasicSignalParameter> a_slot )
        {
            // save reference to proxied HObject not proxy
            ISignalSubscriber subscriber = a_subscriber;
            if( subscriber is IHANProxy ) {
                subscriber = ( (IHANProxy) subscriber ).HObject;
            }

            if( HObject != null ) { 
                return HObject.Disconnect( subscriber, a_signal, a_slot );
            }

            return false;
        }


        /**
         *  We dont want to send signals at all anymore. 
         *  All subscribers are will be notified, 
         *  that we dont want to send any signals anymore. 
         */
        public void DisconnectEstablishedConnections()
        {
            if( HObject != null ) {
                HObject.DisconnectEstablishedConnections();
            }
        }


        /**
         *  adds a property to our object. Will be owned by the object.
         *  If it is a ISignalPublisher, the object will organize his signals.
         */
        public bool AddProperty( IProperty a_prop )
        {
            if( HObject != null )
            {
                return HObject.AddProperty( a_prop );
            }

            return false;
        }


        /**
        *  removes our signal. The object also do not organize the signals anymore.
        */
        public bool RemoveProperty( Key a_key )
        {
            if( HObject != null )
            {
                return HObject.RemoveProperty( a_key );
            }

            return false;
        }

        /**
         *  Returns true if object has property, else false
         */
        public bool HasProperty( Key a_key )
        {
            if( HObject != null ) {
                return HObject.HasProperty( a_key );
            }

            return false;
        }


        /**
         *  Returns abstract property for the key, if available. Else it will return null.
         */
        public IProperty Property( Key a_key )
        {
            if( HObject != null ) {
                return HObject.Property( a_key );
            }

            return null;
        }


        /**
         *  Returns casted property for the key, if available. Else it will return null.
         */
        public Property<T> Property<T>( Key a_key )
        {
            if( HObject != null ) { 
                return HObject.Property<T>( a_key );
            }

            return null;
        }


        /**
         *  Set the properties value. If success it will return true,
         *  else false
         */
        public bool SetProperty<T>( Key a_key, T a_value )
        {
            if( HObject != null ) { 
                return HObject.SetProperty<T>( a_key, a_value );
            }

            return false;
        }


        /**
         *  Try to set the value of the property with the supplied object.
         *  It will try to cast the value to the right type.
         *  Use this function with care!
         */
        public bool TrySetProperty( Key a_key, object a_value )
        {
            if( HObject != null ) { 
                return HObject.TrySetProperty( a_key, a_value );
            }

            return false;
        }


        public bool HasSignal( Key a_signal )
        {
            return HObject.HasSignal( a_signal );
        }


        /**
         *  Meta type of the proxied HObject.
         */
        public virtual MetaType Type()
        {
            if( HObject != null ) {
                return HObject.Type();
            }

            return new MetaType( k_HObjectMonoBehavior );
        }

        public void AddConnection( IGenericSignalConnection a_connection )
        {
            HObject?.AddConnection( a_connection );
        }

        public void RemoveConnection( IGenericSignalConnection a_connection )
        {
            HObject?.RemoveConnection( a_connection );
        }
    }
}