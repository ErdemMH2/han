using HAN.Lib.Basic;
using HAN.Lib.Structure;
using HAN.Lib.Test;


namespace HAN.Lib.Testing 
{
    public class TestSignalUnitTest : UnitTest
    {
        private class THObject : HObject
        {
            public THObject()
            {
            }


            public override MetaType Type()
            {
                return new MetaType( KeyFactory.Create( "MockHObject" ) );
            }
        }

        public override void Cleanup()
        {
        }

        public override void CleanupTest()
        {
        }

        public override void Init()
        {
            addTest( testSignalSendingReceiving, "Test signal sending and receiving" );
            addTest( testDisconnectEstablished, "Test disconnect all signals" );
            addTest( testGenericConnect, "Test generic signal sending and receiving" );
            addTest( testGenericDisconnect, "Test generic signal sending and disconnect" );
            addTest( testGenericDisconnectEstablishedConnections, "Test generic signal sending and DisconnectEstablishedConnections" );
        }

        public override void Prepare()
        {
        }

        public override void PrepareTest()
        {
        }


        private void testSignalSendingReceiving( ref TestResult r_result ) 
        {
            int signalReceived = 0;
            THObject publisher = new THObject();
            Signal signal = new Signal( publisher );
            THObject receiver = new THObject();

            System.Action<BasicSignalParameter> slot = ( BasicSignalParameter a_param ) 
                                 => { if( a_param.Sender == publisher ) { signalReceived++; } };
            signal.Connect( receiver, "Test", slot );

            signal.Emit();
            r_result.Compare( signalReceived, 1, "Signal received" );

            signal.Disconnect( receiver, "Test", slot );

            signal.Emit();
            r_result.Compare( signalReceived, 1, "Signal should not be received" );
        }


        private void testDisconnectEstablished( ref TestResult r_result )
        {
            int signalReceived = 0;
            THObject publisher = new THObject();
            Signal signal = new Signal( publisher );
            THObject receiver = new THObject();

            System.Action<BasicSignalParameter> slot = ( BasicSignalParameter a_param ) 
                                    => { if( a_param.Sender == publisher ) { signalReceived++; } };
            signal.Connect( receiver, "Test", slot );

            System.Action<BasicSignalParameter> slot2 = ( BasicSignalParameter a_param ) 
                                    => { if( a_param.Sender == publisher ) { signalReceived++; } };
            signal.Connect( receiver, "Test2", slot2 );

            signal.Emit();
            r_result.Compare( signalReceived, 2, "Signal received" );

            signal.DisconnectEstablishedConnections();

            signal.Emit();
            r_result.Compare( signalReceived, 2, "Signal should not be received" );
        }


        private void testGenericConnect( ref TestResult r_result )
        {
            int signalReceived = 0;
            IGenericSignalHandler publisher = new THObject();
            Signal<int> signal = new Signal<int>( publisher );

            IGenericSignalHandler receiver = new THObject();

            System.Action<int> slot = ( int a_param ) => { signalReceived += a_param; };
            signal.Connect( receiver, slot );

            signal.Emit( 5 );
            r_result.Compare( signalReceived, 5, "First emit of Signal should be received" );

            signal.Emit( 3 );
            r_result.Compare( signalReceived, 8, "Second emit of Signal should be received" );
        }


        private void testGenericDisconnect( ref TestResult r_result )
        {
            int signalReceived = 0;
            IGenericSignalHandler publisher = new THObject();
            Signal<int> signal = new Signal<int>( publisher );

            IGenericSignalHandler receiver = new THObject();

            System.Action<int> slot = ( int a_param ) => { signalReceived += a_param; };
            signal.Connect( receiver, slot );

            signal.Emit( 5 );
            r_result.Compare( signalReceived, 5, "First emit of Signal should be received" );

            signal.Disconnect( receiver, slot );
            signal.Emit( 3 );
            r_result.Compare( signalReceived, 5, "Second emit of Signal should NOT be received" );
        }


        private void testGenericDisconnectEstablishedConnections( ref TestResult r_result )
        {
            int signalReceived = 0;
            IGenericSignalHandler publisher = new THObject();
            Signal<int> signal = new Signal<int>( publisher );

            IGenericSignalHandler receiver = new THObject();

            System.Action<int> slot = ( int a_param ) => { signalReceived += a_param; };
            signal.Connect( receiver, slot );

            signal.Emit( 5 );
            r_result.Compare( signalReceived, 5, "First emit of Signal should be received" );

            signal.DisconnectEstablishedConnections();
            signal.Emit( 3 );
            r_result.Compare( signalReceived, 5, "Second emit of Signal should NOT be received" );
        }
    }
}