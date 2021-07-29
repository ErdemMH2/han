using HAN.Lib.Basic;
using HAN.Lib.Structure;
using HAN.Lib.Test;

namespace HAN.Lib.Testing
{
    class TestOneDirectionalBindingUnitTest : UnitTest
    {
        private class LocalMockHObject : HObject
        {
            public static readonly Key k_Id = KeyFactory.Create( "Mock_hobject" );
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
                return new MetaType( k_Id );
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
            addTest( testSuccesfulBinding, "Tests succesful One Directional Binding" );
            addTest( testNonSuccesfulBinding, "Tests non-succesful One Directional Binding" );
            addTest( testNoSignalazingProperty, "Tests One Directional Binding with no-signalazingProperty" );
            addTest( testDisconnectBinding, "Tests One Directional Binding Disconnect" );
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

            OneDirectionalBinding successfulBinding = new OneDirectionalBinding( mockHObject1, mockHObject2, LocalMockHObject.k_Health );

            int newHealthValue = 10;
            mockHObject1.SetProperty<int>( LocalMockHObject.k_Health, newHealthValue );

            r_result.Compare( mockHObject2.Property( LocalMockHObject.k_Health ).ValueObject, newHealthValue, "The Binding should change Property B, because Property A changed" );

            successfulBinding.DisconnectEstablishedConnections();

            mockHObject1.SetProperty<int>( LocalMockHObject.k_Health, newHealthValue + 20 );
            r_result.Compare( mockHObject2.Property( LocalMockHObject.k_Health ).ValueObject, newHealthValue, "The Binding should NOT change" );
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

            r_result.ExpectMessage( Debug.Logger.k_Error, "HObject Mock_hobject or Mock_hobject has no Prop Property" );
            r_result.ExpectMessage( Debug.Logger.k_Error, "Cant find Property Prop of Mock_hobject" );
            OneDirectionalBinding noSuccessfulBinding = new OneDirectionalBinding( mockHObject1, mockHObject2, newPropertyKey );
            mockHObject1.SetProperty<int>( newPropertyKey, 2 );

            r_result.Compare( mockHObject2.Property( newPropertyKey2 ).ValueObject, 1, "Property should not change, because there is no binding!" );

            noSuccessfulBinding.DisconnectEstablishedConnections();
        }

        private void testNoSignalazingProperty( ref TestResult r_result )
        {
            LocalMockHObject mockHObject1 = new LocalMockHObject( 1, 1 );
            LocalMockHObject mockHObject2 = new LocalMockHObject( 2, 2 );

            Key noSignalazingKey = KeyFactory.Create( "NoSignalizing" );
            Property<int> noSignalazingProperty = new Property<int>( noSignalazingKey, 1 );
            Property<int> noSignalazingProperty2 = new Property<int>( noSignalazingKey, 1 );
            mockHObject1.AddProperty( noSignalazingProperty );
            mockHObject2.AddProperty( noSignalazingProperty2 );

            r_result.ExpectMessage( Debug.Logger.k_Error, "Can not bind non-SignalizingProperty NoSignalizing" );
            OneDirectionalBinding noSuccessfulBinding = new OneDirectionalBinding( mockHObject1, mockHObject2, noSignalazingKey );
            mockHObject1.SetProperty<int>( noSignalazingKey, 2 );
            
            r_result.VerifyNot( noSuccessfulBinding.IsConnected, "Connection should be false!" );

            r_result.Compare( mockHObject2.Property( noSignalazingKey ).ValueObject, 1, "Property should not change, because Property is not signalazing!" );
        }

        private void testDisconnectBinding( ref TestResult r_result )
        {
            LocalMockHObject mockHObject1 = new LocalMockHObject( 1, 1 );
            LocalMockHObject mockHObject2 = new LocalMockHObject( 2, 2 );

            OneDirectionalBinding successfulBinding = new OneDirectionalBinding( mockHObject1, mockHObject2, LocalMockHObject.k_Health );

            successfulBinding.DisconnectEstablishedConnections();

            int newHealthValue = 10;
            mockHObject1.SetProperty<int>( LocalMockHObject.k_Health, newHealthValue );

            r_result.Compare( mockHObject2.Property( LocalMockHObject.k_Health ).ValueObject, 2, "The Binding should not change Property B, because Binding is disconnected!" );
        }
    }
}
