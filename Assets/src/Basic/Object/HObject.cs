using System;
using System.Collections.Generic;
using HAN.Lib.Structure;

namespace HAN.Lib.Basic
{
    /**
     * Object replacement, which has signals and slots, properties and HAN runtime type information
     */
    public abstract class HObject : GenericSignalHandler
                                  , IHANObject
    {
        /// all signals available on this publisher (with sub publishers)
        protected Dictionary< Key, ISignalPublisher > m_signals;

        /// all properties that are owned by this HObject
        protected Dictionary< Key, IProperty > m_properties;

        /// the meta type object, used for caching the meta type, to prevent recreation on every usage
        protected MetaType m_type;

        ///  Meta type of the object. Has to be implemented!
        public abstract MetaType Type();


        public HObject()
        {
            m_signals = new Dictionary<Key, ISignalPublisher>();
            m_properties = new Dictionary<Key, IProperty>();
        }


        ~HObject()
        {
            DisconnectEstablishedConnections();
        }


        /**
         *  adds a property to our object. Will be owned by the object.
         *  If it is a ISignalPublisher, the object will organize his signals.
         */
        public virtual bool AddProperty( IProperty a_prop )
        {
            if( !m_properties.ContainsKey( a_prop.Id ) )
            {
                if( a_prop is ISignalPublisher signalizingProperty ) {
                    addSignal( a_prop.Id, signalizingProperty );
                }

                m_properties.Add( a_prop.Id, a_prop );
                return true;
            }
            else
            {
                HAN.Debug.Logger.Warning( Type().ObjectName,
                                "cant add property {0} to {1}; Property already present",
                                a_prop.Id.Name, Type().ObjectName.Name );
                return false;
            }
        }

        /**
         *  Removes property. 
         *  If it has a signal it removes our signal. The signal is reseted, so it has no connections anymore.
         *  Even if the connection existed before adding it to the HObject!
         */
        public bool RemoveProperty( Key a_key )
        {
            if( m_properties.ContainsKey( a_key ) )
            {
                var prop = m_properties[a_key];
                if( prop is ISignalPublisher )
                {
                    m_signals[a_key].DisconnectEstablishedConnections();
                    m_signals.Remove( a_key );
                }

                m_properties.Remove( a_key );
                return true;
            }
            else
            {
                HAN.Debug.Logger.Warning( Type().ObjectName,
                                "cant remove property {0} from {1}; Property not found",
                                a_key, Type().ObjectName );
                return false;
            }
        }


        /**
         *  Returns true if object has property, else false
         */
        public bool HasProperty( Key a_key )
        {
            return m_properties.ContainsKey( a_key );
        }


        /**
         *  Returns abstract property for the key, if available. Else it will return null.
         */
        public IProperty Property( Key a_key )
        {
            IProperty property = null;
            if( !m_properties.TryGetValue( a_key, out property ) )
            {
                HAN.Debug.Logger.Error( Type().ObjectName
                                      , "Cant find Property {0} of {1}"
                                      , a_key, Type().ObjectName );
            }

            return property;
        }


        /**
         *  Returns casted property for the key, if available. Else it will return null.
         */
        public Property<T> Property<T>( Key a_key )
        {
            return (Property<T>) Property( a_key );
        }


        /**
         *  Set the properties value. If success it will return true,   
         *  else false
         */
        public bool SetProperty<T>( Key a_key, T a_value )
        {
            var property = Property<T>( a_key );
            if( property != null )
            {
                property.SetValue( a_value );
                return true;
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
            var property = Property( a_key );
            if( property != null )
            {
                return property.TrySetValue( a_value );
            }

            return false;
        }


        /**
         *  Returns true if the HObject has the signal with a_signalKey
         */
        public bool HasSignal( Key a_signalKey ) 
        {
            return m_signals.ContainsKey( a_signalKey );
        }


        /**
         * Adds a signal to this HObject
         */
        protected void addSignal( Key a_signal, ISignalPublisher a_signalPublisher )
        {
            m_signals.Add( a_signal, a_signalPublisher );
        }


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
            if( a_subscriber == null
             || a_signal == null
             || a_slot == null
             || !m_signals.ContainsKey( a_signal ) )
            {
                HAN.Debug.Logger.Error( Type().ObjectName,
                                           @"Connection not possible; one of the paramters is null:
                                             Subscriper {0}
                                             Key {1}
                                             Slot {2}
                                             Has Signal {3}",
                                             a_subscriber, a_signal, a_slot, m_signals.ContainsKey( a_signal ) );

                return false;
            }

            // save reference to proxied HObject not proxy
            ISignalSubscriber subscriber = a_subscriber;
            if( subscriber is IHANProxy proxy )
            {
                subscriber = proxy.HObject;
            }

            // connect actual signal in property
            m_signals[a_signal].Connect( subscriber, a_signal, a_slot );

            return true;
        }


        /**
         *  Disconnect from signal.The a_subscriber is not interested in our signal anymore.
         *  <param name="a_subscriber">Subscriber of the signal, that will deregister slot from signal</param>
         *  <param name="a_signal">key of the signal</param>
         *  <param name="a_slot">Slot, that is not connected to us anymore/param>
         */
        public bool Disconnect( ISignalSubscriber a_subscriber, Key a_signal, Action<BasicSignalParameter> a_slot )
        {
            if( a_subscriber == null
             || a_signal == null
             || a_slot == null )
            {
                HAN.Debug.Logger.Warning( Type().ObjectName,
                            "Disconnect not possible; on of the paramters is null:\nSubscriber {0}\nKey {1}\nSlot {2}",
                            a_subscriber, a_signal, a_slot );

                return false;
            }

            if( !m_signals.ContainsKey( a_signal ) )
            {
                return false; // nothing to disconnect; but also no problem
            }

            // get reference of proxied HObject
            ISignalSubscriber subscriber = a_subscriber;
            if( subscriber is IHANProxy proxy )
            {
                subscriber = proxy.HObject;
            }

            m_signals[a_signal].Disconnect( subscriber, a_signal, a_slot );

            return true;
        }


        public override string ToString()
        {
            return Type().ToString();
        }
    }
}