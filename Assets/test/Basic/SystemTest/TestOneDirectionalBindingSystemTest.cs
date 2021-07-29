using HAN.Lib.Basic;
using HAN.Lib.Structure;
using HAN.Lib.Test;


namespace HAN.Lib.Testing
{
    class TestOneDirectionalBindingSystemTest : SystemTest
    {
        public static readonly Key k_health = KeyFactory.Create( "Health" );
        public static readonly Key k_armor = KeyFactory.Create( "Armor" );

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

        private OneDirectionalBinding m_binding;
        private OneDirectionalBinding m_binding2;

        public override void Cleanup()
        {

        }

        public override void CleanupTest()
        {

        }

        public override void Init()
        {
            addTest( testOneDirectionalBinding, "Create a One Directional Binding and check if property b changes" );
            addTest( testOneDirectionalBindingAndDisconnectPublisher, "Create a One Directional Binding and disconnect publisher" );
            addTest( testMultipleOneDirectionalBinding, "Create multiple One Directional Bindings and check if all property b changes" );
            addTest( testMultipleOneDirectionalBindingAndDisconnectPublisher, "Create multiple One Directional Binding and disconnect publisher" );
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

            if( m_binding != null )
            {
                m_binding.DisconnectEstablishedConnections();
            }

            if( m_binding2 != null )
            {
                m_binding2.DisconnectEstablishedConnections();
            }

            m_mockA.SetProperty<int>( k_health, 0 );
            m_mockB.SetProperty<int>( k_health, 0 );
        }


        private void testOneDirectionalBinding( ref TestResult r_result )
        {
            m_binding = new OneDirectionalBinding( m_mockA, m_mockB, k_health );

            int newValue = 1;
            m_mockA.SetProperty<int>( k_health, newValue );

            r_result.Compare( m_mockA.Property<int>( k_health ).Value, m_mockB.Property<int>( k_health ).Value, "Binding should change health property of subscriber!" );
        }


        private void testOneDirectionalBindingAndDisconnectPublisher( ref TestResult r_result )
        {
            m_binding = new OneDirectionalBinding( m_mockA, m_mockB, k_health );

            m_mockA.DisconnectEstablishedConnections();

            int newValue = 1;
            m_mockA.SetProperty<int>( k_health, newValue );

            r_result.CompareNot( m_mockA.Property<int>( k_health ).Value, m_mockB.Property<int>( k_health ).Value, "Binding should not change health property of subscriber!" );
        }


        private void testMultipleOneDirectionalBinding( ref TestResult r_result )
        {
            MockHObject localMock = new MockHObject( 20 );

            m_binding = new OneDirectionalBinding( m_mockA, m_mockB, k_health );
            m_binding2 = new OneDirectionalBinding( m_mockA, localMock, k_health );

            int newValue = 1;
            m_mockA.SetProperty<int>( k_health, newValue );

            r_result.Compare( m_mockA.Property<int>( k_health ).Value, m_mockB.Property<int>( k_health ).Value, "Binding should change health property of subscriber b!" );
            r_result.Compare( m_mockA.Property<int>( k_health ).Value, localMock.Property<int>( k_health ).Value, "Binding should change health property of subscriber c!" );
        }


        private void testMultipleOneDirectionalBindingAndDisconnectPublisher( ref TestResult r_result )
        {
            MockHObject localMock = new MockHObject( 20 );

            m_binding = new OneDirectionalBinding( m_mockA, m_mockB, k_health );
            m_binding2 = new OneDirectionalBinding( m_mockA, localMock, k_health );

            m_mockA.DisconnectEstablishedConnections();

            int newValue = 1;
            int oldValue = 0;

            m_mockB.SetProperty<int>( k_health, oldValue );
            localMock.SetProperty<int>( k_health, oldValue );


            m_mockA.SetProperty<int>( k_health, newValue );

            r_result.Compare( oldValue, m_mockB.Property<int>( k_health ).Value, "Binding should not change health property of subscriber!" );
            r_result.Compare( oldValue, localMock.Property<int>( k_health ).Value, "Binding should not change health property of subscriber!" );
        }
    }
}
