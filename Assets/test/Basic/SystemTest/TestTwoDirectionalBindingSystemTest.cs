using HAN.Lib.Basic;
using HAN.Lib.Structure;
using HAN.Lib.Test;


namespace HAN.Lib.Testing
{
    class TestTwoDirectionalBindingSystemTest : SystemTest
    {
        private static readonly Key k_health = KeyFactory.Create( "Health" );

        private class MockHObject : HObject
        {
            protected SignalizingProperty<int> m_value;

            public MockHObject()
            {
            }


            public MockHObject( int a_value )
            {
                m_value = new SignalizingProperty<int>( k_health, a_value, this );
                AddProperty( m_value );
            }


            public override MetaType Type()
            {
                return new MetaType( KeyFactory.Create( "MockHObject" ) );
            }
        }


        private MockHObjectMonoBehavior m_mockA;
        private MockHObjectMonoBehavior_Player m_mockB;

        private TwoDirectionalBinding<int> m_binding;
        private TwoDirectionalBinding<int> m_binding2;

        public override void Cleanup()
        {

        }

        public override void CleanupTest()
        {

        }

        public override void Init()
        {
            addTest( testTwoDirectionalBinding, "Create a Two Directional Binding and check if property B and A changes" );
            addTest( testTwoDirectionalBindingAndDisconnectPublisher, "Create a Two Directional Binding and disconnect publisher" );
            addTest( testTwoDirectionalBindingAndDisconnectSubscriber, "Create a Two Directional Binding and disconnect Subscriber" );
            addTest( testMultipleTwoDirectionalBinding, "Create multiple Two Directional Binding" );
            addTest( testMultipleTwoDirectionalBindingAndDisconnectPublisher, "Create multiple Two Directional Binding and disconnect publisher" );
            addTest( testMultipleTwoDirectionalBindingAndDisconnectOneSubscriber, "Create multiple Two Directional Binding and disconnect one Subscriber" );
            addTest( testMultipleTwoDirectionalBindingAndDisconnectAllSubscriber, "Create multiple Two Directional Binding and disconnect all Subscriber" );
        }

        public override void Prepare()
        {
            m_mockA = GetComponentInChildren<MockHObjectMonoBehavior>();
            m_mockB = GetComponentInChildren<MockHObjectMonoBehavior_Player>();
        }

        public override void PrepareTest()
        {
            if( m_mockA )
            {
                m_mockA.DisconnectEstablishedConnections();
            }

            if( m_mockB )
            {
                m_mockB.DisconnectEstablishedConnections();
            }
            m_mockA.SetProperty<int>( k_health, 0 );
            m_mockB.SetProperty<int>( k_health, 0 );

            if( m_binding != null )
            {
                m_binding.DisconnectEstablishedConnections();
            }

            if( m_binding2 != null )
            {
                m_binding2.DisconnectEstablishedConnections();
            }
        }


        private void testTwoDirectionalBinding( ref TestResult r_result )
        {
            m_binding = new TwoDirectionalBinding<int>( m_mockA, m_mockB, k_health );

            int newValue = 1;
            int newValue2 = 2;

            m_mockA.SetProperty<int>( k_health, newValue );
            r_result.Compare( m_mockA.Property<int>( k_health ).Value, m_mockB.Property<int>( k_health ).Value, "Binding should change health property of subscriber!" );

            m_mockB.SetProperty<int>( k_health, newValue2 );
            r_result.Compare( m_mockA.Property<int>( k_health ).Value, m_mockB.Property<int>( k_health ).Value, "Binding should change health property of subscriber!" );
        }


        private void testTwoDirectionalBindingAndDisconnectPublisher( ref TestResult r_result )
        {
            m_binding = new TwoDirectionalBinding<int>( m_mockA.HObject, m_mockB.HObject, k_health );

            int newValue = 1;
            int newValue2 = 2;

            m_mockB.Property<int>( k_health ).SetValue( 0 );

            m_mockA.HObject.DisconnectEstablishedConnections();

            m_mockA.SetProperty<int>( k_health, newValue );
            r_result.Compare( 0, m_mockB.Property<int>( k_health ).Value, "Binding should not change health property of subscriber!" );

            m_mockB.SetProperty<int>( k_health, newValue2 );
            r_result.Compare( m_mockA.Property<int>( k_health ).Value, newValue, "Binding should not change health property of subscriber!" );
        }

        private void testTwoDirectionalBindingAndDisconnectSubscriber( ref TestResult r_result )
        {
            m_binding = new TwoDirectionalBinding<int>( m_mockA.HObject, m_mockB.HObject, k_health );

            int newValue = 1;
            int newValue2 = 2;

            m_mockB.Property<int>( k_health ).SetValue( 0 );

            m_mockB.HObject.DisconnectEstablishedConnections();

            m_mockA.SetProperty<int>( k_health, newValue );
            r_result.Compare(0, m_mockB.Property<int>( k_health ).Value, "Binding should not change health property of subscriber!" );

            m_mockB.SetProperty<int>( k_health, newValue2 );
            r_result.Compare( m_mockA.Property<int>( k_health ).Value, newValue, "Binding should not change health property of subscriber!" );
        }


        private void testMultipleTwoDirectionalBinding( ref TestResult r_result )
        {
            MockHObject m_mockC = new MockHObject( 0 );

            int newValue = 1;
            int newValue2 = 2;
            int newValue3 = 3;

            m_mockB.Property<int>( k_health ).SetValue( 0 );
            m_mockA.Property<int>( k_health ).SetValue( 0 );

            m_binding = new TwoDirectionalBinding<int>( m_mockA.HObject, m_mockB.HObject, k_health );
            m_binding2 = new TwoDirectionalBinding<int>( m_mockA.HObject, m_mockC, k_health );

            m_mockA.SetProperty<int>( k_health, newValue );
            r_result.Compare( newValue, m_mockB.Property<int>( k_health ).Value, "A Binding should change health property of subscriber B and A!" );
            r_result.Compare( newValue, m_mockC.Property<int>( k_health ).Value, "A Binding should change health property of subscriber C and A!" );

            m_mockB.SetProperty<int>( k_health, newValue2 );
            r_result.Compare( m_mockA.Property<int>( k_health ).Value, newValue2, "B Binding should change health property of subscriber B and A!" );

            m_mockC.SetProperty<int>( k_health, newValue3 );
            r_result.Compare( m_mockA.Property<int>( k_health ).Value, newValue3, "C Binding should change health property of subscriber C and A!" );
        }


        // Create Binding and disconnect Publisher
        private void testMultipleTwoDirectionalBindingAndDisconnectPublisher( ref TestResult r_result )
        {
            MockHObject m_mockC = new MockHObject( 0 );

            int newValue = 1;
            int newValue2 = 2;
            int newValue3 = 3;

            m_mockB.Property<int>( k_health ).SetValue( 0 );
            m_mockA.Property<int>( k_health ).SetValue( 0 );

            m_binding = new TwoDirectionalBinding<int>( m_mockA.HObject, m_mockB.HObject, k_health );
            m_binding2 = new TwoDirectionalBinding<int>( m_mockA.HObject, m_mockC, k_health );

            m_mockA.HObject.DisconnectEstablishedConnections();

            m_mockA.SetProperty<int>( k_health, newValue );
            r_result.Compare( 0, m_mockB.Property<int>( k_health ).Value, "Binding should not change health property of subscriber!" );

            m_mockB.SetProperty<int>( k_health, newValue2 );
            r_result.Compare( m_mockA.Property<int>( k_health ).Value, newValue, "Binding should not change health property of subscriber!" );

            m_mockC.SetProperty<int>( k_health, newValue3 );
            r_result.Compare( m_mockA.Property<int>( k_health ).Value, newValue, "Binding should not change health property of subscriber!" );
        }


        // A <=> C
        private void testMultipleTwoDirectionalBindingAndDisconnectOneSubscriber( ref TestResult r_result )
        {
            MockHObject m_mockC = new MockHObject( 0 );

            int newValue = 1;
            int newValue2 = 2;
            int newValue3 = 3;

            m_mockB.Property<int>( k_health ).SetValue( 0 );
            m_mockA.Property<int>( k_health ).SetValue( 0 );

            m_binding = new TwoDirectionalBinding<int>( m_mockA.HObject, m_mockB.HObject, k_health );
            m_binding2 = new TwoDirectionalBinding<int>( m_mockA.HObject, m_mockC, k_health );

            m_mockB.HObject.DisconnectEstablishedConnections();

            m_mockA.SetProperty<int>( k_health, newValue );
            r_result.Compare( m_mockB.Property<int>( k_health ).Value, 0, "Binding should not change health property of subscriber!" );

            m_mockB.SetProperty<int>( k_health, newValue2 );
            r_result.Compare( m_mockA.Property<int>( k_health ).Value, newValue, "Binding should not change health property of subscriber!" );

            m_mockC.SetProperty<int>( k_health, newValue3 );
            r_result.Compare( m_mockA.Property<int>( k_health ).Value, newValue3, "Binding should change health property of subscriber!" );
        }


        // Create Binding and disconnect all
        private void testMultipleTwoDirectionalBindingAndDisconnectAllSubscriber( ref TestResult r_result )
        {
            MockHObject m_mockC = new MockHObject( 0 );

            int newValue = 1;
            int newValue2 = 2;
            int newValue3 = 3;

            m_mockB.Property<int>( k_health ).SetValue( 0 );
            m_mockA.Property<int>( k_health ).SetValue( 0 );

            m_binding = new TwoDirectionalBinding<int>( m_mockA.HObject, m_mockB.HObject, k_health );
            m_binding2 = new TwoDirectionalBinding<int>( m_mockA.HObject, m_mockC, k_health );

            m_mockB.HObject.DisconnectEstablishedConnections();
            m_mockC.DisconnectEstablishedConnections();

            m_mockA.SetProperty<int>( k_health, newValue );
            r_result.Compare( 0, m_mockB.Property<int>( k_health ).Value, "Binding should not change health property of subscriber!" );

            m_mockB.SetProperty<int>( k_health, newValue2 );
            r_result.Compare( m_mockA.Property<int>( k_health ).Value, newValue, "Binding should not change health property of subscriber!" );

            m_mockC.SetProperty<int>( k_health, newValue3 );
            r_result.Compare( m_mockA.Property<int>( k_health ).Value, newValue, "Binding should not change health property of subscriber!" );
        }
    }
}
