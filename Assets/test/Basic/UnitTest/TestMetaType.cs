using HAN.Lib.Basic;
using HAN.Lib.Structure;

namespace HAN.Lib.Test
{
    class TestMetaType : Test
    {
        
        private void TestConstructor( ref TestResult r_result ) 
        {
            Key key_type = KeyFactory.Create( "a_key_type" );
            Key key_name = KeyFactory.Create( "a_key_name" );
            MetaType metaType = new MetaType( key_type );

            r_result.Compare(metaType.Type == key_type, true, " Key Type couldn't be set! ");

            metaType = new MetaType( key_type, key_name );
            r_result.Compare( ( metaType.Type == key_type )  
                                && ( metaType.ObjectName == key_name ), true, " Key Type and ObjectName couldn't be set! " );
        }


        public override void Cleanup() { }


        public override void CleanupTest() { }


        public override void Init()
        {
            addTest( TestConstructor, "Instantiate a meta type" );
        }


        public override void Prepare()
        {
        }


        public override void PrepareTest() { }
    }
}
