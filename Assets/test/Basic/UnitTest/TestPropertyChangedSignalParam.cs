using HAN.Lib.Basic;
using HAN.Lib.Structure;
using static HAN.Lib.Basic.HObjectMonoBehavior;

namespace HAN.Lib.Test
{
    class TestPropertyChangedSignalParam : Test
    {
        private void testConstructor( ref TestResult r_result )
        {
            Key key = KeyFactory.Create( "a_key" );
            ISignalPublisher signalPublisher = new DefaultHObject();
            Property<int> property = new Property<int>( key, 10 );

            PropertyChangedSignalParam<int> propertyChangedSignalParam = new PropertyChangedSignalParam<int>( property, signalPublisher );

            r_result.Verify( propertyChangedSignalParam.Value == 10, "Value not set" );
            r_result.Verify( propertyChangedSignalParam.Property.Equals( property ), "Property not set" );
        }


        public override void Cleanup()
        {

        }


        public override void CleanupTest()
        {

        }


        public override void Init()
        {
            addTest( testConstructor, "Instantiate a PropertyChangedSignalParam" );
        }


        public override void Prepare()
        {
        }


        public override void PrepareTest()
        {

        }
    }
}
