using HAN.Lib.Basic;
using HAN.Lib.Structure;
using HAN.Lib.Test;

namespace HAN.Lib.Testing
{
    class TestTwoDirectionalBinding : UnitTest
    {
        private class LocalMockHObject : HObject
        {
            public static readonly Key k_Health = KeyFactory.Create( "Health" );
            public static readonly Key k_Armor = KeyFactory.Create( "Armor" );

            private SignalizingProperty<int> m_health;
            private SignalizingProperty<int> m_armor;

            public LocalMockHObject( int a_health, int a_armor )
            {
                m_health = new SignalizingProperty<int>( k_Health, a_health, this );
                m_armor = new SignalizingProperty<int>( k_Armor, a_armor, this );
                AddProperty( m_health );
                AddProperty( m_armor );
            }

            public LocalMockHObject()
            {

            }

            public override MetaType Type()
            {
                return new MetaType( KeyFactory.Create( "Mock_HObject" ) );
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
            addTest( testSuccesfulBinding, "Test a succesful two directional binding" );
            addTest( testNonSuccesfulBinding, "Test a non-succesful two directional binding" );
            addTest( testNoSignalazingProperty, "Test binding with non-signalazingproperty" );
            addTest( testDisconnectBinding, "Create Two way binding and disconnect all" );
            addTest( testDisconnectedFromBinding, "Create Two way binding and disconnect one and check if other also disconnected" );
        }

        public override void Prepare()
        {

        }

        public override void PrepareTest()
        {

        }

        private void testSuccesfulBinding( ref TestResult r_result )
        {
            LocalMockHObject mockHObject1 = new LocalMockHObject( 1, 1 );
            LocalMockHObject mockHObject2 = new LocalMockHObject( 2, 2 );

            TwoDirectionalBinding<int> successfulBinding = new TwoDirectionalBinding<int>( mockHObject1, mockHObject2, LocalMockHObject.k_Health );

            int newHealthValue = 10;

            mockHObject1.SetProperty<int>( LocalMockHObject.k_Health, newHealthValue );
            r_result.Compare( mockHObject2.Property( LocalMockHObject.k_Health ).ValueObject, newHealthValue, "The Binding should change Property B, because Property A changed!" );

            mockHObject2.SetProperty<int>( LocalMockHObject.k_Health, newHealthValue + 1 );
            r_result.Compare( mockHObject1.Property( LocalMockHObject.k_Health ).ValueObject, newHealthValue + 1, "The Binding should change Property B, because Property A changed!" );

            successfulBinding.DisconnectEstablishedConnections();

            mockHObject2.SetProperty<int>( LocalMockHObject.k_Health, newHealthValue + 100 );
            r_result.Compare( mockHObject1.Property( LocalMockHObject.k_Health ).ValueObject, newHealthValue + 1, "The Binding should value should not change" );

        }

        private void testNonSuccesfulBinding( ref TestResult r_result )
        {
            LocalMockHObject mockHObject1 = new LocalMockHObject( 1, 1 );
            LocalMockHObject mockHObject2 = new LocalMockHObject( 2, 2 );

            Key newPropertyKey = KeyFactory.Create( "Prop" );
            Key newPropertyKey2 = KeyFactory.Create( "Prop2" );
            SignalizingProperty<int> property = new SignalizingProperty<int>( newPropertyKey, 1, mockHObject1 );
            SignalizingProperty<int> property2 = new SignalizingProperty<int>( newPropertyKey2, 1, mockHObject2 );
            mockHObject1.AddProperty( property );
            mockHObject2.AddProperty( property2 );

            // only one of both mockHObjects will have each Property = no match
            r_result.ExpectMessage( Debug.Logger.k_Error, "HObject Mock_HObject or Mock_HObject has no Prop Property" );
            r_result.ExpectMessage( Debug.Logger.k_Error, "Cant find Property Prop of Mock_HObject" );
            TwoDirectionalBinding<int> nonsuccessfulBinding = new TwoDirectionalBinding<int>( mockHObject1, mockHObject2, newPropertyKey );

            mockHObject1.SetProperty<int>( newPropertyKey, 2 );
            r_result.Compare( mockHObject2.Property<int>( newPropertyKey2 ).ValueObject, 1, "Property B should not change!" );

            mockHObject2.SetProperty<int>( newPropertyKey, 3 );
            r_result.Compare( mockHObject1.Property( newPropertyKey ).ValueObject, 2, "Property A should not change!" );

            nonsuccessfulBinding.DisconnectEstablishedConnections();
        }

        private void testNoSignalazingProperty( ref TestResult r_result )
        {
            LocalMockHObject mockHObject1 = new LocalMockHObject( 1, 1 );
            LocalMockHObject mockHObject2 = new LocalMockHObject( 2, 2 );

            Key newPropertyKey = KeyFactory.Create( "Prop" );
            Property<int> property = new Property<int>( newPropertyKey, 1 );
            Property<int> property2 = new Property<int>( newPropertyKey, 1 );
            mockHObject1.AddProperty( property );
            mockHObject2.AddProperty( property2 );

            TwoDirectionalBinding<int> successfulBinding = new TwoDirectionalBinding<int>( mockHObject1, mockHObject2, newPropertyKey );

            mockHObject1.SetProperty<int>( newPropertyKey, 2 );
            r_result.Compare( mockHObject2.Property<int>( newPropertyKey ).ValueObject, 1, "Property B should not change!" );

            mockHObject2.SetProperty<int>( newPropertyKey, 2 );
            r_result.Compare( mockHObject1.Property( newPropertyKey ).ValueObject, 2, "Property A should not change!" );

            successfulBinding.DisconnectEstablishedConnections();
        }

        private void testDisconnectBinding( ref TestResult r_result )
        {
            LocalMockHObject mockHObject1 = new LocalMockHObject( 1, 1 );
            LocalMockHObject mockHObject2 = new LocalMockHObject( 1, 1 );

            TwoDirectionalBinding<int> successfulBinding = new TwoDirectionalBinding<int>( mockHObject1, mockHObject2, LocalMockHObject.k_Health );
            
            successfulBinding.DisconnectEstablishedConnections();

            // SetProperty without a binding
            mockHObject1.SetProperty<int>( LocalMockHObject.k_Health, 2 );
            r_result.Compare( mockHObject2.Property<int>( LocalMockHObject.k_Health ).Value, 1, "Property B should not change, because binding is diconnected!" );

            mockHObject2.SetProperty<int>( LocalMockHObject.k_Health, 3 );
            r_result.Compare( mockHObject1.Property<int>( LocalMockHObject.k_Health ).Value, 2, "Property A should not change, because binding is diconnected!" );
        }


        private void testDisconnectedFromBinding( ref TestResult r_result )
        {
            LocalMockHObject mockHObject1 = new LocalMockHObject( 1, 1 );
            LocalMockHObject mockHObject2 = new LocalMockHObject( 2, 2 );

            TwoDirectionalBinding<int> successfulBinding = new TwoDirectionalBinding<int>( mockHObject1, mockHObject2, LocalMockHObject.k_Health );
            
            mockHObject1.DisconnectEstablishedConnections();

            // SetProperty without a binding
            mockHObject1.SetProperty<int>( LocalMockHObject.k_Health, 3 );
            r_result.Compare( mockHObject2.Property( LocalMockHObject.k_Health ).ValueObject, 2, "Property B should not change, because binding is diconnected!" );

            mockHObject2.SetProperty<int>( LocalMockHObject.k_Health, 4 );
            r_result.Compare( mockHObject1.Property( LocalMockHObject.k_Health ).ValueObject, 3, "Property A should not change, because binding is diconnected!" );
        }
    }
}
