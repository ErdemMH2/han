using HAN.Lib.Basic;
using HAN.Lib.Structure;
using HAN.Lib.Test;


namespace HAN.Lib.Testing
{
    class TestIncrementalIdUnitTest : UnitTest
    {
        public override void Cleanup()
        {

        }

        public override void CleanupTest()
        {

        }

        public override void Init()
        {
            addTest( testIncrementalId, "Create and compare IncrementalId's" );
        }

        public override void Prepare()
        {

        }

        public override void PrepareTest()
        {

        }

        private void testIncrementalId( ref TestResult r_result )
        {
            Key key1 = KeyFactory.Create( "Key" );
            Key key2 = KeyFactory.Create( "Key" );

            IncrementalId incrementalId = new IncrementalId( key1 );
            IncrementalId incrementalId2 = new IncrementalId( key2 );

            r_result.Verify( incrementalId.Id.Equals( key1 ), "Key of incremental Id is right" );
            r_result.Verify( incrementalId.Equals( incrementalId2 ), "IncrementalId's are same" );
        }
    }
}
