using HAN.Lib.Structure;
using System;

namespace HAN.Lib.Basic
{
    /**
     * Binding of two SignalazingProperties in two way. If property A changes, property B will also change.
     * If property B changes, property A will also change. 
     * Implements IPropertyBinding.
     */
    public class TwoDirectionalBinding<T> : GenericSignalHandler
                                          , IPropertyBinding
    {
        /**
         * returns true if binding has a valid connecting
         */
        public bool IsConnected { get; private set; } = false;

        public Key Id { get { return m_id.Value; } }

        private ReadonlyProperty<Key> m_id;

        private IHANObject m_publisher_A;
        private IHANObject m_publisher_B;

        private Action<BasicSignalParameter> m_slot_A;
        private Action<BasicSignalParameter> m_slot_B;

        private Key m_bindedPropertyKey;

        private bool m_recursiveStop = false;

        /**
         * Binding of two SignalazingProperties with same key in two way. 
         * Both Properties mirros each other and have the same values.
         * <param name="a_publisher_A">Publisher A of Binding. Also the Destination.</param>
         * <param name="a_publisher_B">Publisher B of Binding. Also the Destination.</param>
         * <param name="a_propertyKey">Key of the propeties.</param>
         */
        public TwoDirectionalBinding( IHANObject a_publisher_A, IHANObject a_publisher_B, Key a_propertyKey )
        {
            if( a_publisher_A.HasProperty( a_propertyKey )
             && a_publisher_B.HasProperty( a_propertyKey ) )
            {
                // Check if Property is a SignalizingProperty
                if( a_publisher_A.Property( a_propertyKey ) is SignalizingProperty<T> property1
                 && a_publisher_B.Property( a_propertyKey ) is SignalizingProperty<T> property2 )
                {
                    m_publisher_A = a_publisher_A;
                    m_publisher_B = a_publisher_B;

                    m_bindedPropertyKey = a_propertyKey;

                    // on of each slots will set recursiveStop to true and the other will set it to false
                    // in that way we will block exactly one signal
                    // even when another subscriber will modify the value, we wont block its change signal

                    m_slot_A = ( BasicSignalParameter a_param ) =>
                               {
                                   if( !m_recursiveStop )
                                   {
                                       m_recursiveStop = true;
                                       property2.TrySetValue( property1.ValueObject );
                                   }
                                   else
                                   {
                                       m_recursiveStop = false;
                                   }
                               };

                    m_slot_B = ( BasicSignalParameter a_param ) =>
                               {
                                   if( !m_recursiveStop )
                                   {
                                       m_recursiveStop = true;
                                       property1.TrySetValue( property2.ValueObject );
                                   }
                                   else
                                   {
                                       m_recursiveStop = false;
                                   }
                               };

                    IsConnected = m_publisher_A.Connect( this, m_bindedPropertyKey, m_slot_A );

                    if( IsConnected )
                    {
                        IsConnected = m_publisher_B.Connect( this, m_bindedPropertyKey, m_slot_B );

                        m_id = new ReadonlyProperty<Key>( Mvc.Keys.Component.Id, m_bindedPropertyKey );

                        if( !IsConnected )
                        {
                            HAN.Debug.Logger.Error( Type().ObjectName,
                                            string.Format( "Second Property could not connect" ) );
                        }
                    }
                    else
                    {
                        HAN.Debug.Logger.Error( Type().ObjectName,
                                            string.Format( "First Property could not connect" ) );
                    }
                }
                else
                {
                    HAN.Debug.Logger.Error( Type().ObjectName,
                                        string.Format( "Property {0} and Property {1} have different Types",
                                        a_publisher_A.Property( a_propertyKey ).GetValueType(),
                                        a_publisher_B.Property( a_propertyKey ).GetValueType() ) );
                }
            }
            else
            {
                HAN.Debug.Logger.Error( Type().ObjectName,
                                string.Format( "HObject {0} or {1} has no {2} Property",
                                a_publisher_A.Type().ObjectName.Name,
                                a_publisher_B.Type().ObjectName.Name,
                                a_propertyKey.Name ) );
            }
        }


        public override void RemoveConnection( IGenericSignalConnection a_connection )
        {
            if( IsConnected )
            {
                // assume the disconnection work in first place
                // because in m_publisher.Disconnect() the subscriber (this) calls DisconnectedFrom again
                // and falsified the return value
                IsConnected = false;

                if( a_connection.Sender == m_publisher_A )
                {
                    m_publisher_B.Disconnect( this, m_bindedPropertyKey, m_slot_B );
                }
                else if( a_connection.Sender == m_publisher_B )
                {
                    m_publisher_A.Disconnect( this, m_bindedPropertyKey, m_slot_A );
                }
            }
        }


        public override void DisconnectEstablishedConnections()
        {
            if( IsConnected )
            {
                IsConnected = false;

                bool disconnectA = false;
                bool disconnectB = false;

                disconnectA = m_publisher_A.Disconnect( this, m_bindedPropertyKey, m_slot_A );
                disconnectB = m_publisher_B.Disconnect( this, m_bindedPropertyKey, m_slot_B );

                if( !disconnectA )
                {
                    HAN.Debug.Logger.Error( Type().ObjectName,
                                string.Format( "Publsiher {0} could not disconnect",
                                m_publisher_A.Type().ObjectName.Name ) );
                }

                if( !disconnectB )
                {
                    HAN.Debug.Logger.Error( Type().ObjectName,
                                string.Format( "Publsiher {0} could not disconnect",
                                m_publisher_B.Type().ObjectName.Name ) );
                }
            }
        }

        public MetaType Type()
        {
            return new MetaType( KeyFactory.Create( "TwoDirectionalBinding" ) );
        }
    }
}
