using HAN.Lib.Structure;
using System;

namespace HAN.Lib.Basic
{
    /**
     * Binding of two properties in one way. If property A changes, property B will also change with the same value. 
     * Property B will be read only and mirror the value of the property A.
     * Implements IPropertyBinding.
     */
    public class OneDirectionalBinding : GenericSignalHandler
                                       , IPropertyBinding
    {
        /**
         * returns true if binding has a valid connecting
         */
        public bool IsConnected { get { return m_connected; } }

        public Key Id { get { return m_id.Value; } }

        private ReadonlyProperty<Key> m_id;

        private IHANObject m_publisher;

        private Key m_bindedPropertyKey;

        private Action<BasicSignalParameter> m_bindSlot;

        private bool m_connected = false;

        /**
         *  Creates a One Directional Binding from Publsiher to Subscriber with same Property Type.
         *  Publsihers Property will change Subscribers Property.
         *  <param name="a_publisher">Publisher of Binding.</param>
         *  <param name="a_publishing">Publisher Property, which will change the subscriber property. Needs to be a SignlazingProperty.</param>
         *  <param name="a_subscriber">Subscriber of Binding.</param>
         *  <param name="a_subscribing">Subscriber Property, mirrors the publishing property. Needs to be a SignlazingProperty</param>
         */
        public OneDirectionalBinding( IHANObject a_publisher, IProperty a_publishing, IHANObject a_subscriber, IProperty a_subscribing )
        {
            sharedConstructor( a_publisher, a_publishing, a_subscriber, a_subscribing );
        }

        /**
         *  Creates a One Directional Binding from Publsiher to Subscriber with same Property Type and Key.
         *  Checks if Publisher and Subscriber have the Property and bind them.
         *  <param name="a_publisher">Publisher of Binding.</param>
         *  <param name="a_subscriber">Subscriber of Binding</param>
         *  <param name="a_propertyKey">Key of property</param>
         */
        public OneDirectionalBinding( IHANObject a_publisher, IHANObject a_subscriber, Key a_propertyKey )
        {
            IProperty propertyPublisher = a_publisher.Property( a_propertyKey );
            IProperty propertySubscriber = a_subscriber.Property( a_propertyKey );

            if( propertyPublisher != null
            && propertySubscriber != null )
            {
                sharedConstructor( a_publisher, propertyPublisher, a_subscriber, propertySubscriber );
            }
            else
            {
                HAN.Debug.Logger.Error( Type().ObjectName,
                string.Format( "HObject {0} or {1} has no {2} Property",
                                a_publisher.Type().ObjectName,
                                a_subscriber.Type().ObjectName,
                                a_propertyKey ) );
            }
        }

        /**
         *  Creates a One Directional Binding between Publisher and Subscriber if property exits in both.
         *  Publisher connects to the Binding and sets value of subscriber on change.
         *  <param name="a_publisher">Publisher of Binding.</param>
         *  <param name="a_publishing">Publisher Property, which will change the subscriber property. Needs to be a SignlazingProperty.</param>
         *  <param name="a_subscriber">Subscriber of Binding.</param>
         *  <param name="a_subscribing">Subscriber Property, mirrors the publishing property. Needs to be a SignlazingProperty</param>
         */
        private void sharedConstructor( IHANObject a_publisher, IProperty a_publishing, IHANObject a_subscriber, IProperty a_subscribing )
        {
            if( a_publishing.GetValueType() == a_subscribing.GetValueType() )
            {
                if( a_publisher.HasProperty( a_publishing.Id )
                && a_subscriber.HasProperty( a_subscribing.Id ) )
                {
                    // Check if Property is a SignalizingProperty
                    if( a_publishing is ISignalPublisher )
                    {
                        m_publisher = a_publisher;

                        m_bindedPropertyKey = a_publishing.Id;

                        m_bindSlot = ( BasicSignalParameter a_param ) => { a_subscribing.TrySetValue( a_publishing.ValueObject ); };

                        m_connected = m_publisher.Connect( this, m_bindedPropertyKey, m_bindSlot );


                        if( m_connected ) {
                            m_id = new ReadonlyProperty<Key>( Mvc.Keys.Component.Id, m_bindedPropertyKey );
                        }
                        else
                        {
                            HAN.Debug.Logger.Error( Type().ObjectName,
                                                string.Format( "OneDirectionalBinding connected: {0}",
                                                m_connected ) );
                        }
                    }
                    else
                    {
                        HAN.Debug.Logger.Error( Type().ObjectName,
                                        string.Format( "Can not bind non-SignalizingProperty {0}",
                                        a_publishing.Id.Name ) );
                    }
                }
                else
                {
                    HAN.Debug.Logger.Error( Type().ObjectName,
                                    string.Format( "HObject {0} or {1} has no {2} Property",
                                    a_publisher.Type().ObjectName.Name,
                                    a_subscriber.Type().ObjectName.Name,
                                    a_publishing.Id.Name ) );
                }
            }
            else
            {
                HAN.Debug.Logger.Error( Type().ObjectName,
                                    string.Format( "Property {0} and Property {1} have different Types",
                                    a_publishing.Type().ObjectName.Name,
                                    a_subscribing.Type().ObjectName.Name ) );
            }
        }

        public override void DisconnectEstablishedConnections()
        {
            base.DisconnectEstablishedConnections();

            if( m_connected )
            {
                m_connected = false;
            }
        }

        public MetaType Type()
        {
            return new MetaType( KeyFactory.Create( "OneDirectionalBinding" ) );
        }
    }
}
