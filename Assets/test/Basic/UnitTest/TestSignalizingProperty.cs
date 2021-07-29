using HAN.Lib.Basic;
using HAN.Lib.Structure;

namespace HAN.Lib.Test
{
    class TestSignalizingProperty : Test
    {
        private static readonly Key k_health = KeyFactory.Create( "a_health" );

        public class DefaultHObject : HObject
        {
            public override MetaType Type()
            {
                return new MetaType( "DefaultHObject" );
            }
        }

        public override void Init()
        {
            addTest( testConnect, "Test if signal is recevied" );
            addTest( testDisconnect, "Test if signal is disconnected" );
            addTest( testDisconnectEstablishedConnections, "Test if signals are disconnected" );
            addTest( testSetValue, "Test if value is changed" );
            addTest( testTrySetValue, "Test if value is changed" );
        }


        public override void Prepare()
        {
        }


        public override void PrepareTest()
        {

        }


        public override void Cleanup()
        {

        }


        public override void CleanupTest()
        {

        }


        private void testConnect( ref TestResult r_result )
        {
            DefaultHObject propertyOwner = new DefaultHObject();
            bool signalReceived = false;
            SignalizingProperty<int> signalizingProperty = new SignalizingProperty<int>( k_health, 10, propertyOwner );
            signalizingProperty.Connect( new DefaultHObject(), k_health, ( ( BasicSignalParameter a_param ) => { signalReceived = true; }) );
            signalizingProperty.SetValue( 5 );
            r_result.Compare( signalReceived, true, "Signal not received" );
        }


        private void testDisconnect( ref TestResult r_result )
        {
            DefaultHObject propertyOwner = new DefaultHObject();
            bool signalNotReceived = true;
            SignalizingProperty<int> signalizingProperty = new SignalizingProperty<int>( k_health, 10, propertyOwner );

            DefaultHObject subscriber = new DefaultHObject();
            System.Action<BasicSignalParameter> slot = ( ( BasicSignalParameter a_param ) => { signalNotReceived = false; } );

            signalizingProperty.Connect( subscriber, k_health, slot );
            signalizingProperty.Disconnect( subscriber, k_health, slot );

            signalizingProperty.SetValue( 5 );
            r_result.Verify( signalNotReceived, "Did not disconnect" );
        }


        private void testDisconnectEstablishedConnections( ref TestResult r_result )
        {
            DefaultHObject propertyOwner = new DefaultHObject();
            bool signalReceived = false;
            SignalizingProperty<int> signalizingProperty = new SignalizingProperty<int>( k_health, 10, propertyOwner );
            signalizingProperty.Connect( new DefaultHObject(), k_health, (System.Action<BasicSignalParameter>) ( ( BasicSignalParameter a_param ) => { signalReceived = true; } ) );
            signalizingProperty.DisconnectEstablishedConnections();

            signalizingProperty.SetValue( 5 );
            r_result.Compare( signalReceived, false, "Did not disconnect" );
        }


        private void testSetValue( ref TestResult r_result )
        {
            DefaultHObject propertyOwner = new DefaultHObject();
            SignalizingProperty<int> signalizingProperty = new SignalizingProperty<int>( k_health, 10, propertyOwner );
            signalizingProperty.SetValue( 5 );
            r_result.Compare( signalizingProperty.Value, 5, "Value did not changed" );
        }


        private void testTrySetValue( ref TestResult r_result )
        {
            DefaultHObject propertyOwner = new DefaultHObject();
            SignalizingProperty<int> signalizingProperty = new SignalizingProperty<int>( k_health, 10, propertyOwner );
            signalizingProperty.TrySetValue( 5 );
            r_result.Compare( signalizingProperty.Value, 5, "Value did not changed" );
            signalizingProperty.TrySetValue( 5.0f );
            r_result.Compare( signalizingProperty.Value, 5, "Value should not changed" );
        }
    }
}
