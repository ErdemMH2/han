using HAN.Lib.Basic;
using HAN.Lib.Structure;

namespace HAN.Lib.Test
{
    class TestHObject : Test
    {
        public override void Cleanup()
        {
        }


        public override void CleanupTest()
        {
        }


        public override void Init()
        {
            addTest( testProperty, "Adding properties and check if added" );
            addTest( testAddProperty, "Add property and check if added" );
            addTest( testRemoveProperty, "Remove property and check if removed" );
            addTest( testSetProperty, "Set added properties and check if changed" );
            addTest( testHasProperty, "Add property and check HasProperty" );
            addTest( testConnect, "Set added properties and check if changed" );
            addTest( testDisconnect, "Set added properties and check if changed" );
            addTest( testConnectSignal, "Connect to signal and receive emitted signal" );
            addTest( testDisconnectSignal, "Connect to signal and receive emitted signal" );
            addTest( testConnectMultiple, "Connect multiple subscribers and check receive emitted signal" );
            addTest( testDisconnectMultiple, "Connect multiple subscribers and Disconnect" );
            addTest( testDisconnectEstablishedConnections, "Disconnect all signals and check if still receiving" );
            addTest( testDisconnectEstablishedConnectionsSubscriber, "Disconnect from subscriber and check if still receiving" );
        }


        public override void Prepare()
        {
        }


        public override void PrepareTest()
        {
        }


        // Implement HObject, because HObject is abstract
        private class THObject : HObject
        {
            public static readonly Key k_Health = KeyFactory.Create( "Health" );
            public static readonly Key k_Armor = KeyFactory.Create( "Armor" );
            public static readonly Key k_Signal = KeyFactory.Create( "Signal" );

            private SignalizingProperty<int> m_health;
            private SignalizingProperty<int> m_armor;

            private Signal m_testSignal;

            public THObject( int a_health, int a_armor )
            {
                m_health = new SignalizingProperty<int>( k_Health, a_health, this );
                m_armor = new SignalizingProperty<int>( k_Armor, a_armor, this );
                AddProperty( m_health );
                AddProperty( m_armor );

                m_testSignal = new Signal( this );
                addSignal( k_Signal, m_testSignal );
            }

            public THObject() : this( 0, 0)
            {
            }

            public void EmitSignal()
            {
                m_testSignal.Emit();
            }

            public override MetaType Type() {
                return new MetaType( KeyFactory.Create( "a_mock_hobject" ) );
            }
        }


        private void testConnect( ref TestResult r_result )
        {
            THObject obj = new THObject( 100, 200 );
            THObject subscriber = new THObject( 1, 1 );

            bool signalReceived = false;

            obj.Connect( subscriber, THObject.k_Health, ( ( BasicSignalParameter a_param ) => { signalReceived = true; } ) );

            obj.SetProperty<int>( THObject.k_Health, 50 );

            r_result.Verify( signalReceived, "Signal was not received" );
        }


        private void testDisconnect( ref TestResult r_result )
        {
            THObject obj = new THObject();
            THObject subscriber = new THObject();

            bool signalReceived = false;
            System.Action<BasicSignalParameter> slot = ( ( BasicSignalParameter a_param ) => { signalReceived = true; } );

            obj.Connect( subscriber, THObject.k_Health, slot );
            obj.Disconnect( subscriber, THObject.k_Health, slot );

            obj.SetProperty<int>( THObject.k_Health, 50 );

            r_result.Compare( signalReceived, false, "Signal should NOT be received" );
        }


        private void testConnectSignal( ref TestResult r_result )
        {
            THObject obj = new THObject();
            THObject subscriber = new THObject();

            bool signalReceived = false;
            System.Action<BasicSignalParameter> slot = ( ( BasicSignalParameter a_param ) => { signalReceived = true; } );

            obj.Connect( subscriber, THObject.k_Signal, slot );

            obj.EmitSignal(); 

            r_result.Compare( signalReceived, true, "Signal should be received" );
        }


        private void testDisconnectSignal( ref TestResult r_result )
        {
            THObject obj = new THObject();
            THObject subscriber = new THObject();

            bool signalReceived = false;
            System.Action<BasicSignalParameter> slot = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) =>
                                                    { signalReceived = true; } );

            obj.Connect( subscriber, THObject.k_Signal, slot );
            obj.Disconnect( subscriber, THObject.k_Signal, slot );

            obj.EmitSignal();
            r_result.Compare( signalReceived, false, "Signal should NOT be received" );
        }


        private void testConnectMultiple( ref TestResult r_result )
        {
            THObject obj = new THObject( 100, 200 );
            THObject subscriber = new THObject( 1, 1 );
            THObject subscriber2 = new THObject( 1, 1 );

            int signalReceived = 0;
            System.Action<BasicSignalParameter> slot = (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) =>
            { signalReceived++; } );

            obj.Connect( subscriber, THObject.k_Signal, slot );
            obj.Connect( subscriber2, THObject.k_Signal, slot );

            obj.EmitSignal();

            r_result.Compare( signalReceived, 2, "Signal should be received 2 times" );
        }


        private void testDisconnectMultiple( ref TestResult r_result )
        {
            THObject obj = new THObject( 100, 200 );
            THObject subscriber = new THObject( 1, 1 );
            THObject subscriber2 = new THObject( 1, 1 );

            int signalReceived = 0;
            System.Action<BasicSignalParameter> slot = ( BasicSignalParameter a_param ) => { signalReceived++; };
            obj.Connect( subscriber, THObject.k_Signal, slot );

            System.Action<BasicSignalParameter> slot2 = ( BasicSignalParameter a_param ) => { signalReceived++; };
            obj.Connect( subscriber2, THObject.k_Signal, slot2 );

            obj.Disconnect( subscriber2, THObject.k_Signal, slot2 );
            obj.EmitSignal();

            r_result.Compare( signalReceived, 1, "Signal should be received 1 time" );

            obj.Disconnect( subscriber, THObject.k_Signal, slot );
            obj.EmitSignal();

            r_result.Compare( signalReceived, 1, "Signal should be received 1 time" );
        }


        private void testDisconnectEstablishedConnections( ref TestResult r_result )
        {
            THObject obj = new THObject( 100, 200 );
            THObject subscriber = new THObject( 1, 1 );
            THObject subscriber2 = new THObject( 1, 1 );

            int signalReceived = 0;
            System.Action<BasicSignalParameter> slot = ( BasicSignalParameter a_param ) => { signalReceived++; };
            obj.Connect( subscriber, THObject.k_Signal, slot );

            System.Action<BasicSignalParameter> slot2 = ( BasicSignalParameter a_param ) => { signalReceived++; };
            obj.Connect( subscriber2, THObject.k_Signal, slot2 );

            obj.Disconnect( subscriber2, THObject.k_Signal, slot2 );
            obj.EmitSignal();

            r_result.Compare( signalReceived, 1, "Signal should be received 1 time" );

            obj.DisconnectEstablishedConnections();
            obj.EmitSignal();

            r_result.Compare( signalReceived, 1, "Signal should be received 1 time" );
        }

        private void testDisconnectEstablishedConnectionsSubscriber( ref TestResult r_result )
        {
            THObject obj = new THObject();
            THObject subscriber = new THObject();
            THObject subscriber2 = new THObject();

            int signalReceived = 0;
            System.Action<BasicSignalParameter> slot = ( BasicSignalParameter a_param ) => { signalReceived++; };
            obj.Connect( subscriber, THObject.k_Signal, slot );

            System.Action<BasicSignalParameter> slot2 = ( BasicSignalParameter a_param ) => { signalReceived++; };
            obj.Connect( subscriber2, THObject.k_Signal, slot2 );

            obj.EmitSignal();

            r_result.Compare( signalReceived, 2, "Signal should be received 1 time" );

            subscriber2.DisconnectEstablishedConnections();
            obj.EmitSignal();

            r_result.Compare( signalReceived, 3, "Signal should be received 1 time" );
        }


        private void testProperty( ref TestResult r_result ) 
        {
            THObject testObj = new THObject( 100, 200 );

            r_result.Verify( testObj.Property( THObject.k_Health ) != null, " Couldn't find health property " );
            r_result.Verify( testObj.Property( THObject.k_Armor ) != null, " Couldn't find armor property " );

            r_result.Compare( testObj.Property<int>( THObject.k_Health ).Value == 100, true, " Couldn't set right health property " );
            r_result.Compare( testObj.Property<int>( THObject.k_Armor ).Value == 200, true, " Couldn't set right armor property " );

            r_result.Verify( testObj.HasProperty( THObject.k_Health ), " Couldn't find health property " );
            r_result.Verify( testObj.HasProperty( THObject.k_Armor ), " Couldn't find armor propertyy " );
        }


        private void testAddProperty( ref TestResult r_result )
        {
            Key propertyKey = KeyFactory.Create("TestProperty");
            THObject testObj = new THObject();

            Property<int> testProperty = new Property<int>( propertyKey, 10 );

            r_result.Verify( testObj.AddProperty( testProperty ), " Could not add health property " );

            r_result.ExpectMessage( Debug.Logger.k_Warning, "cant add property TestProperty to a_mock_hobject; Property already present" );
            r_result.Compare( testObj.AddProperty( testProperty ), false, " Should not add same property " );

            r_result.Compare( testObj.Property<int>( propertyKey ).Value == 10, true, " Couldn't set right health value " );

            r_result.Verify( testObj.HasProperty( propertyKey ), " Couldn't find health property " );
        }


        private void testRemoveProperty( ref TestResult r_result)
        {
            Key propertyKey = KeyFactory.Create("TestProperty");
            THObject testObj = new THObject();

            bool hasProp = testObj.HasProperty( propertyKey );
            r_result.Verify( !hasProp, "Should NOT have property" );

            Property<int> prop = new Property<int>( propertyKey, 10 );
            testObj.AddProperty( prop );

            hasProp = testObj.HasProperty( propertyKey );
            r_result.Verify( hasProp, "Should have property" );

            bool removed = testObj.RemoveProperty( propertyKey );
            r_result.Verify( removed, "Couldn't find TestProperty property" );

            hasProp = testObj.HasProperty( propertyKey );
            r_result.Verify( !hasProp, "Should NOT have property" );
        }


        private void testSetProperty( ref TestResult r_result )
        {
            int value = 1;
            int changedValue = 2;

            THObject testObj = new THObject( value, 10 );

            r_result.Compare( testObj.Property( THObject.k_Health ) != null, true, " Couldn't add property " );
            r_result.Compare( testObj.SetProperty<int>( THObject.k_Health, changedValue ), true, " Couldn't set property ");

            Property<int> changedProperty = testObj.Property<int>( THObject.k_Health );
            r_result.Compare( changedProperty.Value == changedValue, true, " Property is set but didn't changed! " );
        }


        private void testHasProperty( ref TestResult r_result )
        {
            THObject testObj = new THObject( 10, 10 );
            r_result.Verify( testObj.HasProperty( THObject.k_Health ), " Couldn't find health property " );
            r_result.Compare( testObj.HasProperty( KeyFactory.Create( "not_available" ) ), false, " Should not find property " );
        }
    }
}
