using HAN.Lib.Basic;
using HAN.Lib.Test;
using System.Linq;

namespace HAN.Lib.Testing
{
    public class TestGenericConnectionBookkeeper : UnitTest
    {
        public override void Init()
        {
            addTest( testAddRemove, "Test add and remove connections" );
        }


        public override void Cleanup()
        {
        }

        public override void CleanupTest()
        {
        }

        public override void Prepare()
        {
        }

        public override void PrepareTest()
        {
        }


        private void testAddRemove( ref TestResult r_result )
        {
            GenericConnectionBookkeeper bookkeeper = new GenericConnectionBookkeeper();

            r_result.Compare( bookkeeper.Connections.Count, 0, "Bookkeeper should be empty" );

            IGenericSignalConnection con1 = new GenericSignalConnection<System.Action<int>>
                                                ( null, null, null, (int a) => { } );
            bookkeeper.AddConnection( con1 );
            r_result.Compare( bookkeeper.Connections.Count, 1, "Bookkeeper should have one entry" );
            r_result.Verify( bookkeeper.Connections.Contains(con1), "Bookkeeper should have one entry" );


            IGenericSignalConnection con2 = new GenericSignalConnection<System.Action<int>>
                                                ( null, null, null, (int a) => { } );
            bookkeeper.AddConnection( con2 );
            r_result.Compare( bookkeeper.Connections.Count, 2, "Bookkeeper should be empty" );
            r_result.Verify( bookkeeper.Connections.Contains( con1 ), "Bookkeeper should have con1" );
            r_result.Verify( bookkeeper.Connections.Contains( con2 ), "Bookkeeper should have con2" );

            bookkeeper.RemoveConnection( con1 );
            r_result.Compare( bookkeeper.Connections.Count, 1, "Bookkeeper should have one entry" );
            r_result.VerifyNot( bookkeeper.Connections.Contains( con1 ), "Bookkeeper should not have con1" );
            r_result.Verify( bookkeeper.Connections.Contains( con2 ), "Bookkeeper should have con2" );

            bookkeeper.RemoveConnection( con2 );
            r_result.Compare( bookkeeper.Connections.Count, 0, "Bookkeeper should be empty" );
            r_result.VerifyNot( bookkeeper.Connections.Contains( con1 ), "Bookkeeper should not have con1" );
            r_result.VerifyNot( bookkeeper.Connections.Contains( con2 ), "Bookkeeper should not have con2" );
        }
    }
}
