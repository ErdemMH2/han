using HAN.Lib.Basic;
using static HAN.Lib.Basic.HObjectMonoBehavior;

namespace HAN.Lib.Test
{
    class TestBasicSignalParameter : Test
    {

        private void testConstructor( ref TestResult r_result ) 
        {
            DefaultHObject publisher = new DefaultHObject();
            BasicSignalParameter signal = new BasicSignalParameter( publisher );

            r_result.Compare( signal.Sender == publisher, true, "Sender is not set" );
        }


        public override void Cleanup()
        {

        }


        public override void CleanupTest()
        {

        }


        public override void Init()
        {
            addTest( testConstructor, "Instatiate a BasicSignalParameter" );
        }


        public override void Prepare()
        {
        }


        public override void PrepareTest()
        {

        }
    }
}
