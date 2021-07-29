using HAN.Lib.Basic;
using HAN.Lib.Basic.Collections;
using HAN.Lib.Test;
using System.Linq;

namespace HAN.Lib.Testing
{
    public class TestMutableEnumerable : UnitTest
    {
        public override void Init()
        {
            addTest( testAddRemove, "Test add and remove entries" );
            addTest( testFind, "Test add and find entries" );
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
            MutableEnumerable<string> collection = new MutableEnumerable<string>();

            collection.Add( "1" );
            collection.Add( "2" );
            collection.Add( "3" );

            foreach( var entry in collection )
            {
                if( entry.Contains( "x" ) )
                {
                    break;
                }

                collection.Add( entry + "x" );
            }

            r_result.Verify( collection.Contains( "1" ), "1 should be contained" );
            r_result.Verify( collection.Contains( "2" ), "2 should be contained" );
            r_result.Verify( collection.Contains( "3" ), "3 should be contained" );

            r_result.Verify( collection.Contains( "1x" ), "1x should be contained" );
            r_result.Verify( collection.Contains( "2x" ), "2x should be contained" );
            r_result.Verify( collection.Contains( "3x" ), "3x should be contained" );


            collection.Remove( "1" );
            r_result.Compare( collection.Find( x => x == "1" ), null, "1 should NOT be contained" );
            r_result.Verify( collection.Contains( "2" ), "2 should be contained" );
            r_result.Verify( collection.Contains( "3" ), "3 should be contained" );

            r_result.Verify( collection.Contains( "1x" ), "1x should be contained" );
            r_result.Verify( collection.Contains( "2x" ), "2x should be contained" );
            r_result.Verify( collection.Contains( "3x" ), "3x should be contained" );


            collection.Remove( "1x" );
            r_result.Compare( collection.Find( x => x == "1" ), null, "1 should NOT be contained" );
            r_result.Verify( collection.Contains( "2" ), "2 should be contained" );
            r_result.Verify( collection.Contains( "3" ), "3 should be contained" );

            r_result.Compare( collection.Find( x => x == "1x" ), null, "1 should NOT be contained" );
            r_result.Verify( collection.Contains( "2x" ), "2x should be contained" );
            r_result.Verify( collection.Contains( "3x" ), "3x should be contained" );
        }


        private void testFind( ref TestResult r_result )
        {
            MutableEnumerable<string> collection = new MutableEnumerable<string>();

            collection.Add( "1" );
            collection.Add( "2" );
            collection.Add( "3" );

            r_result.CompareNot( collection.Find( x => x == "1" ), null, "1 should be found" );
            r_result.CompareNot( collection.Find( x => x == "2" ), null, "2 should be found" );
            r_result.CompareNot( collection.Find( x => x == "3" ), null, "3 should be found" );

            r_result.Compare( collection.Find( x => x == "12" ), null, "12 should NOT be contained" );
            r_result.Compare( collection.Find( x => x == "1x" ), null, "1x should NOT be contained" );
            r_result.Compare( collection.Find( x => x == "13" ), null, "13 should NOT be contained" );
        }
    }
}
