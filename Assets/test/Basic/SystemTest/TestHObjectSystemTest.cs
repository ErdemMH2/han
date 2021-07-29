using HAN.Lib.Basic;
using HAN.Lib.Structure;
using HAN.Lib.Test;


namespace HAN.Lib.Testing
{
    public class TestHObjectSystemTest : SystemTest
    {
        private class MockHObject : HObject
        {
            public static readonly Key k_Value = KeyFactory.Create( "Value" );
            protected SignalizingProperty<int> m_value;    


            public MockHObject()
            {
            }


            public MockHObject( int a_value )
            {
                m_value = new SignalizingProperty<int>( k_Value, a_value, this );
                AddProperty( m_value );
            }


            public override MetaType Type()
            {
                return new MetaType( KeyFactory.Create( "MockHObject" ) );
            }
        }


        private MockHObjectMonoBehavior m_mock;
        private MockHObjectMonoBehavior_Player m_mock_player;


        public override void Cleanup()
        {
        }


        public override void CleanupTest()
        {
        }


        public override void Init()
        {
            addTest( testHObjectSignals, "Test signal sending and receiving in slot" );
            addTest( testHObjectDisconnect, "Test disconnect from signal" );
            addTest( testHObjectSenderDisconnect, "Test disconnect initiated from sender" );
            addTest( testHObjectProxyDisconnect, "Test disconnect through IHANProxy" );
            addTest( testHOBjectMultipleConnectionsToOnePublisher, "Test connecting multiple subscriber to one publisher" );
            addTest( testHOBjectMultipleConnectionsToOnePublisherWithDisconnection, 
                        "Test connecting multiple subscribers to one publisher and unsubscribe one" );
            addTest( testHOBjectMultipleConnectionsToOnePublisherWithDisconnectAll,
                        "Test connecting multiple subscribers to one publisher and unsubscribe all" );

            addTest( testHObjectOneSubscriberToMultiplePubsliher, "Test connecting multiple pubsliher to one subscriber" );
            addTest( testHObjectOneSubscriberToMultiplePubsliherWithDisconnectOne,
                        "Test connecting multiple pubsliher to one subscriber and unsubscribe one" );
            addTest( testHObjectOneSubscriberToMultiplePubsliherWithDisconnectAll,
                        "Test connecting multiple pubsliher to one subscriber and unsubscribe all" );

            addTest( testHObjectMultipleSubscriberToMultiplePubsliher,
                        "Test connecting multiple subscribers to multiple publishers" );
            addTest( testHObjectMultipleSubscriberToMultiplePubsliherWithDisconnectOne,
                        "Test connecting multiple subscribers to multiple publishers and unsubscribe one" );
            addTest( testHObjectMultipleSubscriberToMultiplePubsliherWithDisconnectAll,
                        "Test connecting multiple subscribers to multiple publishers and unsubscribe all" );

            addTest( testHObjectChainConnection,
                        "Test chain connecting one subscriber to one publisher one subscriber one publisher..." );
            addTest( testHObjectChainConnectionWithDisconnectOne,
                        "Test chain connecting one subscriber to one publisher one subscriber one publisher... and unsubscribe one" );
            addTest( testHObjectChainConnectionWithDisconnectAll,
                        "Test chain connecting one subscriber to one publisher one subscriber one publisher... and unsubscribe all" );

            addTest( testHObjectSubscriberPublisherShowOnInspector, "Test showing the Subscribers and Publishers in Inspector" );

        }


        public override void Prepare()
        {
            m_mock = GetComponentInChildren<MockHObjectMonoBehavior>();
            m_mock_player = GetComponentInChildren<MockHObjectMonoBehavior_Player>();
        }


        public override void PrepareTest()
        {
            if( m_mock ) {
                m_mock.DisconnectEstablishedConnections();
            }

            if( m_mock_player ) {
                m_mock_player.DisconnectEstablishedConnections();
            }
        }


        private void testHObjectSignals( ref TestResult r_result )
        {
            // test will use HObjectMonoBehavior as a interface to HObject
            int health = m_mock.Property<int>( KeyFactory.Create( "Health" ) ).Value; // SignalizingProperty

            bool healthChanged = false;
            System.Action<BasicSignalParameter> slot = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { healthChanged = !healthChanged; } );
            m_mock.Connect( m_mock_player, KeyFactory.Create( "Health" ), slot );
            m_mock.SetProperty<int>( KeyFactory.Create( "Health" ), health + 1 ); // init value is 10; sets property via HObjectMonoBehavior!
            r_result.Verify( healthChanged, "Health changed signal should be arrived!" );

            m_mock.Disconnect( m_mock_player, KeyFactory.Create( "Health" ), slot );
            m_mock.SetProperty<int>( KeyFactory.Create( "Health" ), health - 1 ); // value is 11; sets property via HObjectMonoBehavior!
            r_result.Verify( healthChanged, "healthChanged should not be changed, we disconnected before!" );
        }


        private void testHObjectDisconnect( ref TestResult r_result )
        {
            MockHObject localMock = new MockHObject();
            int health = m_mock.Property<int>( KeyFactory.Create( "Health" ) ).Value; // SignalizingProperty
            bool healthChanged = false;
            System.Action<BasicSignalParameter> slot = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { healthChanged = !healthChanged; } );
            m_mock.Connect( localMock, KeyFactory.Create( "Health" ), slot );
            m_mock.SetProperty<int>( KeyFactory.Create( "Health" ), health + 1 ); // init value is 10; sets property via HObjectMonoBehavior!
            r_result.Verify( healthChanged, "Health changed signal should be arrived!" );

            localMock.DisconnectEstablishedConnections();

            m_mock.SetProperty<int>( KeyFactory.Create( "Health" ), health - 1 );    // will crash if localMock has not been unsubscribed
            r_result.Verify( healthChanged, "healthChanged should not be changed, we disconnected before!" );
        }


        private void testHObjectSenderDisconnect( ref TestResult r_result )
        {
            MockHObject localMock = new MockHObject();
            int health = m_mock.Property<int>( KeyFactory.Create( "Health" ) ).Value; // SignalizingProperty
            bool healthChanged = false;
            System.Action<BasicSignalParameter> slot = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { healthChanged = !healthChanged; } );
            m_mock.Connect( localMock, KeyFactory.Create( "Health" ), slot );
            m_mock.SetProperty<int>( KeyFactory.Create( "Health" ), health + 1 ); // init value is 10; sets property via HObjectMonoBehavior!
            r_result.Verify( healthChanged, "Health changed signal should be arrived!" );

            m_mock.DisconnectEstablishedConnections();

            m_mock.SetProperty<int>( KeyFactory.Create( "Health" ), health - 1 );    // will crash if localMock has not been unsubscribed
            r_result.Verify( healthChanged, "healthChanged should not be changed, we disconnected before!" );
        }


        private void testHObjectProxyDisconnect( ref TestResult r_result )
        {
            // test will use HObjectMonoBehavior as a interface to HObject
            int health = m_mock.Property<int>( KeyFactory.Create( "Health" ) ).Value; // SignalizingProperty

            bool healthChanged = false;
            System.Action<BasicSignalParameter> slot = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { healthChanged = !healthChanged; } );
            m_mock.Connect( m_mock_player, KeyFactory.Create( "Health" ), slot );
            m_mock.SetProperty<int>( KeyFactory.Create( "Health" ), health + 1 ); // init value is 10; sets property via HObjectMonoBehavior!
            r_result.Verify( healthChanged, "Health changed signal should be arrived!" );

            m_mock.DisconnectEstablishedConnections();
            m_mock.SetProperty<int>( KeyFactory.Create( "Health" ), health - 1 ); // value is 11; sets property via HObjectMonoBehavior!
            r_result.Verify( healthChanged, "healthChanged should not be changed, we disconnected before!" );
        }


        // test multiple connection to one publisher
        private void testHOBjectMultipleConnectionsToOnePublisher( ref TestResult r_result )
        {
            int health = m_mock.Property<int>( KeyFactory.Create( "Health" ) ).Value; // SignalizingProperty

            MockHObject localMock = new MockHObject();
            MockHObject localMock2 = new MockHObject();
            bool healthChanged1 = false;
            bool healthChanged2 = false;
            System.Action<BasicSignalParameter> slot1 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { healthChanged1 = !healthChanged1; } );
            System.Action<BasicSignalParameter> slot2 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { healthChanged2 = !healthChanged2; } );

            m_mock.Connect( localMock, KeyFactory.Create( "Health" ), slot1 );
            m_mock.Connect( localMock2, KeyFactory.Create( "Health" ), slot2 );

            m_mock.SetProperty<int>( KeyFactory.Create( "Health" ), health + 1 ); // init value is 10; sets property via HObjectMonoBehavior!
            r_result.Verify( healthChanged1, "Health changed signal should be arrived!" );
            r_result.Verify( healthChanged2, "Health changed signal should be arrived!" );
        }


        // test multiple connection to one publisher then disconnect one and test other 
        private void testHOBjectMultipleConnectionsToOnePublisherWithDisconnection( ref TestResult r_result )
        {
            int health = m_mock.Property<int>( KeyFactory.Create( "Health" ) ).Value; // SignalizingProperty

            MockHObject localMock = new MockHObject();
            MockHObject localMock2 = new MockHObject();
            bool healthChanged1 = false;
            bool healthChanged2 = false;
            System.Action<BasicSignalParameter> slot1 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { healthChanged1 = !healthChanged1; } );
            System.Action<BasicSignalParameter> slot2 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { healthChanged2 = !healthChanged2; } );

            m_mock.Connect( localMock, KeyFactory.Create( "Health" ), slot1 );
            m_mock.Connect( localMock2, KeyFactory.Create( "Health" ), slot2 );

            m_mock.SetProperty<int>( KeyFactory.Create( "Health" ), health + 1 ); // init value is 10; sets property via HObjectMonoBehavior!
            r_result.Verify( healthChanged1, "Health changed signal should be arrived!" );
            r_result.Verify( healthChanged2, "Health changed signal should be arrived!" );

            localMock2.DisconnectEstablishedConnections();

            m_mock.SetProperty<int>( KeyFactory.Create( "Health" ), health + 2 ); // init value is 10; sets property via HObjectMonoBehavior!
            r_result.Verify( !healthChanged1, "Health changed signal should be arrived!" );
            r_result.Verify( healthChanged2, "Health changed signal should NOT be arrived!" );
        }


        // test multiple connection to one publisher then disconnect all
        private void testHOBjectMultipleConnectionsToOnePublisherWithDisconnectAll( ref TestResult r_result )
        {
            int health = m_mock.Property<int>( KeyFactory.Create( "Health" ) ).Value; // SignalizingProperty

            MockHObject localMock = new MockHObject();
            MockHObject localMock2 = new MockHObject();
            bool healthChanged1 = false;
            bool healthChanged2 = false;
            System.Action<BasicSignalParameter> slot1 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { healthChanged1 = !healthChanged1; } );
            System.Action<BasicSignalParameter> slot2 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { healthChanged2 = !healthChanged2; } );

            m_mock.Connect( localMock, KeyFactory.Create( "Health" ), slot1 );
            m_mock.Connect( localMock2, KeyFactory.Create( "Health" ), slot2 );

            m_mock.SetProperty<int>( KeyFactory.Create( "Health" ), health + 1 ); // init value is 10; sets property via HObjectMonoBehavior!
            r_result.Verify( healthChanged1, "Health changed signal should be arrived!" );
            r_result.Verify( healthChanged2, "Health changed signal should be arrived!" );

            localMock2.DisconnectEstablishedConnections();
            m_mock.DisconnectEstablishedConnections();

            m_mock.SetProperty<int>( KeyFactory.Create( "Health" ), health + 2 ); // init value is 10; sets property via HObjectMonoBehavior!
            r_result.Verify( healthChanged1, "Health changed signal should NOT be arrived!" );
            r_result.Verify( healthChanged2, "Health changed signal should NOT be arrived!" );
        }


        // test one subscriber to multiple publisher
        private void testHObjectOneSubscriberToMultiplePubsliher( ref TestResult r_result )
        {
            int localMockValue = 1;

            MockHObject localMockPublisher = new MockHObject( localMockValue );
            MockHObject localMockPublisher2 = new MockHObject( localMockValue );

            bool valueChanged1 = false;
            bool valueChanged2 = false;
            System.Action<BasicSignalParameter> slot1 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { valueChanged1 = !valueChanged1; } );
            System.Action<BasicSignalParameter> slot2 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { valueChanged2 = !valueChanged2; } );

            localMockPublisher.Connect(m_mock, MockHObject.k_Value, slot1 );
            localMockPublisher2.Connect(m_mock, MockHObject.k_Value, slot2 );

            localMockPublisher.SetProperty<int>( MockHObject.k_Value, localMockValue + 1 );
            localMockPublisher2.SetProperty<int>( MockHObject.k_Value, localMockValue + 1 );
            r_result.Verify( valueChanged1, "Value changed signal should be arrived!" );
            r_result.Verify( valueChanged2, "Value changed signal should be arrived!" );
        }



        // test one subscribers to multiple publishers disconnect one of them
        private void testHObjectOneSubscriberToMultiplePubsliherWithDisconnectOne( ref TestResult r_result )
        {
            int localMockValue = 1;

            MockHObject localMockPublisher = new MockHObject( localMockValue );
            MockHObject localMockPublisher2 = new MockHObject( localMockValue );

            bool valueChanged1 = false;
            bool valueChanged2 = false;
            System.Action<BasicSignalParameter> slot1 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { valueChanged1 = !valueChanged1; } );
            System.Action<BasicSignalParameter> slot2 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { valueChanged2 = !valueChanged2; } );

            localMockPublisher.Connect( m_mock, MockHObject.k_Value, slot1 );
            localMockPublisher2.Connect( m_mock, MockHObject.k_Value, slot2 );

            localMockPublisher.SetProperty<int>( MockHObject.k_Value, localMockValue + 1 );
            localMockPublisher2.SetProperty<int>( MockHObject.k_Value, localMockValue + 1 );
            r_result.Verify( valueChanged1, "Value changed signal should be arrived!" );
            r_result.Verify( valueChanged2, "Value changed signal should be arrived!" );

            localMockPublisher.Disconnect( m_mock, MockHObject.k_Value, slot1 );

            localMockPublisher.SetProperty<int>( MockHObject.k_Value, localMockValue + 2 );
            localMockPublisher2.SetProperty<int>( MockHObject.k_Value, localMockValue + 2 );
            r_result.Verify( valueChanged1, "Value changed signal should NOT be arrived!" );
            r_result.Verify( !valueChanged2, "Value changed signal should be arrived!" );
        }


        // test one subscribers to multiple publishers disconnect all of them
        private void testHObjectOneSubscriberToMultiplePubsliherWithDisconnectAll( ref TestResult r_result )
        {
            int localMockValue = 1;

            MockHObject localMockPublisher = new MockHObject( localMockValue );
            MockHObject localMockPublisher2 = new MockHObject( localMockValue );

            bool valueChanged1 = false;
            bool valueChanged2 = false;
            System.Action<BasicSignalParameter> slot1 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { valueChanged1 = !valueChanged1; } );
            System.Action<BasicSignalParameter> slot2 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { valueChanged2 = !valueChanged2; } );

            localMockPublisher.Connect( m_mock, MockHObject.k_Value, slot1 );
            localMockPublisher2.Connect( m_mock, MockHObject.k_Value, slot2 );

            localMockPublisher.SetProperty<int>( MockHObject.k_Value, localMockValue + 1 );
            localMockPublisher2.SetProperty<int>( MockHObject.k_Value, localMockValue + 1 );
            r_result.Verify( valueChanged1, "Value changed signal should be arrived!" );
            r_result.Verify( valueChanged2, "Value changed signal should be arrived!" );

            m_mock.DisconnectEstablishedConnections();

            localMockPublisher.SetProperty<int>( MockHObject.k_Value, localMockValue + 2 );
            localMockPublisher2.SetProperty<int>( MockHObject.k_Value, localMockValue + 2 );
            r_result.Verify( valueChanged1, "Value changed signal should NOT be arrived!" );
            r_result.Verify( valueChanged2, "Value changed signal should NOT be arrived!" );
        }


        // test multiple subscriber to multiple publisher
        private void testHObjectMultipleSubscriberToMultiplePubsliher( ref TestResult r_result )
        {
            int localMockValue = 1;
            MockHObject[] localMockPublishers = new MockHObject[3];

            MockHObject[] localMockSubscribers = new MockHObject[3];

            int counter = 0;
            System.Action<BasicSignalParameter> slot1 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { counter += 1; } );

            for( int i = 0; i < localMockPublishers.Length; i++ )
            {
                localMockPublishers[i] = new MockHObject( localMockValue );

                for( int k = 0; k < localMockSubscribers.Length; k++ )
                {
                    localMockSubscribers[k] = new MockHObject();
                    localMockPublishers[i].Connect( localMockSubscribers[k], MockHObject.k_Value, slot1 );
                }

                localMockValue++;
                localMockPublishers[i].SetProperty<int>( MockHObject.k_Value, localMockValue );
            }
            
            r_result.Compare( true, counter == 9, "Value changed signal should be arrived!" );
        }


        // test multiple subscribers to multiple publishers (cross dependency!) disconnect one of them 
        private void testHObjectMultipleSubscriberToMultiplePubsliherWithDisconnectOne( ref TestResult r_result )
        {
            int localMockValue = 1;
            MockHObject[] localMockPublishers = new MockHObject[3];

            MockHObject[] localMockSubscribers = new MockHObject[3];

            int counter = 0;
            System.Action<BasicSignalParameter> slot1 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { counter += 1; } );

            for( int i = 0; i < localMockPublishers.Length; i++ )
            {
                localMockPublishers[i] = new MockHObject( localMockValue );

                for( int k = 0; k < localMockSubscribers.Length; k++ )
                {
                    localMockSubscribers[k] = new MockHObject();
                    localMockPublishers[i].Connect( localMockSubscribers[k], MockHObject.k_Value, slot1 );
                }
            }

            localMockPublishers[0].DisconnectEstablishedConnections();

            foreach(MockHObject publisherMock in localMockPublishers) 
            {
                localMockValue++;
                publisherMock.SetProperty<int>( MockHObject.k_Value, localMockValue );
            }

            r_result.Compare( true, counter == 6, "Value changed signal should be arrived!" );
        }


        // test multiple subscribers to multiple publishers (cross dependency!) disconnect all of them
        private void testHObjectMultipleSubscriberToMultiplePubsliherWithDisconnectAll( ref TestResult r_result )
        {
            int localMockValue = 1;
            MockHObject[] localMockPublishers = new MockHObject[3];

            MockHObject[] localMockSubscribers = new MockHObject[3];

            int counter = 0;
            System.Action<BasicSignalParameter> slot1 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { counter += 1; } );

            for( int i = 0; i < localMockPublishers.Length; i++ )
            {
                localMockPublishers[i] = new MockHObject( localMockValue );

                for( int k = 0; k < localMockSubscribers.Length; k++ )
                {
                    localMockSubscribers[k] = new MockHObject();
                    localMockPublishers[i].Connect( localMockSubscribers[k], MockHObject.k_Value, slot1 );
                }
            }
            

            foreach( MockHObject publisherMock in localMockPublishers )
            {
                publisherMock.DisconnectEstablishedConnections();
                localMockValue++;
                publisherMock.SetProperty<int>( MockHObject.k_Value, localMockValue );
            }

            r_result.Verify( counter == 0, "Value changed signal should be arrived!" );
        }


        // test chain one subscriber one publisher one subscriber ... dependency to each other 
        private void testHObjectChainConnection( ref TestResult r_result )
        {
            int value = 1;

            MockHObject localMock = new MockHObject( value );
            MockHObject localMock2 = new MockHObject( value + 1 );
            MockHObject localMock3 = new MockHObject( value + 2 );
            MockHObject localMock4 = new MockHObject( value + 3 );

            int counter = 0;

            System.Action<BasicSignalParameter> slot1 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => 
            { 
                localMock2.SetProperty<int>(MockHObject.k_Value, 0);
                counter += 1;
            } );

            System.Action<BasicSignalParameter> slot2 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => 
            { 
                localMock3.SetProperty<int>( MockHObject.k_Value, 0 );
                counter += 1;
            } );

            System.Action<BasicSignalParameter> slot3 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => 
            { 
                localMock4.SetProperty<int>( MockHObject.k_Value, 0 );
                counter += 1;
            } );

            localMock.Connect( localMock2, MockHObject.k_Value, slot1 );
            localMock2.Connect( localMock3, MockHObject.k_Value, slot2 );
            localMock3.Connect( localMock4, MockHObject.k_Value, slot3 );

            localMock.SetProperty<int>( MockHObject.k_Value, 0 );
            r_result.Compare( true, counter == 3, "Value changed signal should be arrived!" );
        }


        // test chain one subscriber one publisher one subscriber ... dependency to each other .. remove one in chain
        private void testHObjectChainConnectionWithDisconnectOne( ref TestResult r_result )
        {
            int value = 1;

            MockHObject localMock = new MockHObject( value );
            MockHObject localMock2 = new MockHObject( value + 1 );
            MockHObject localMock3 = new MockHObject( value + 2 );
            MockHObject localMock4 = new MockHObject( value + 3 );

            int counter = 0;

            System.Action<BasicSignalParameter> slot1 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) =>
            {
                localMock2.SetProperty<int>( MockHObject.k_Value, 0 );
                counter += 1;
            } );

            System.Action<BasicSignalParameter> slot2 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) =>
            {
                localMock3.SetProperty<int>( MockHObject.k_Value, 0 );
                counter += 1;
            } );

            System.Action<BasicSignalParameter> slot3 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) =>
            {
                localMock4.SetProperty<int>( MockHObject.k_Value, 0 );
                counter += 1;
            } );

            localMock.Connect( localMock2, MockHObject.k_Value, slot1 );
            localMock2.Connect( localMock3, MockHObject.k_Value, slot2 );
            localMock3.Connect( localMock4, MockHObject.k_Value, slot3 );

            localMock2.Disconnect( localMock3, MockHObject.k_Value, slot2 );

            localMock.SetProperty<int>( MockHObject.k_Value, 0 );
            r_result.Verify( counter == 1, "Value changed signal should be arrived only once!" );
        }


        // test chain one subscriber one publisher one subscriber ... dependency to each other .. remove all
        private void testHObjectChainConnectionWithDisconnectAll( ref TestResult r_result )
        {
            int value = 1;

            MockHObject localMock = new MockHObject( value );
            MockHObject localMock2 = new MockHObject( value + 1 );
            MockHObject localMock3 = new MockHObject( value + 2 );
            MockHObject localMock4 = new MockHObject( value + 3 );

            int counter = 0;

            System.Action<BasicSignalParameter> slot1 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) =>
            {
                localMock2.SetProperty<int>( MockHObject.k_Value, 0 );
                counter += 1;
            } );

            System.Action<BasicSignalParameter> slot2 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) =>
            {
                localMock3.SetProperty<int>( MockHObject.k_Value, 0 );
                counter += 1;
            } );

            System.Action<BasicSignalParameter> slot3 = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) =>
            {
                localMock4.SetProperty<int>( MockHObject.k_Value, 0 );
                counter += 1;
            } );

            localMock.Connect( localMock2, MockHObject.k_Value, slot1 );
            localMock2.Connect( localMock3, MockHObject.k_Value, slot2 );
            localMock3.Connect( localMock4, MockHObject.k_Value, slot3 );

            localMock.DisconnectEstablishedConnections();
            localMock2.DisconnectEstablishedConnections();
            localMock3.DisconnectEstablishedConnections();
            localMock4.DisconnectEstablishedConnections();

            localMock.SetProperty<int>( MockHObject.k_Value, 0 );
            r_result.Verify( counter == 0, "Value changed signal should NOT be arrived!" );
        }

        private void testHObjectSubscriberPublisherShowOnInspector( ref TestResult r_result )
        {
            MockHObject localMock = new MockHObject( 1 );

            bool healthChanged = false;
            System.Action<BasicSignalParameter> slot = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { healthChanged = !healthChanged; } );
            m_mock.Connect( m_mock_player, KeyFactory.Create( "Health" ), slot );
            m_mock.Connect( localMock, KeyFactory.Create( "Armor" ), slot );

            m_mock_player.Connect( m_mock, KeyFactory.Create( "Health" ), slot );
        }
    }
}