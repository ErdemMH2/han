using HAN.Lib.Basic;
using HAN.Lib.Structure;

namespace HAN.Lib.Test
{
    class TestProperty : Test
    {
        private static readonly Key k_Prop = KeyFactory.Create( "a_prop" );    
    

        private void testProperty( ref TestResult r_result )
        {
            Property<int> prop = new Property<int>( k_Prop, 1);

            r_result.Compare( prop.Value == 1, true, "Value not set" );
        }


        private void testType( ref TestResult r_result )
        {
            Property<int> prop = new Property<int>( k_Prop, 1 );
            MetaType type = prop.Type();

            r_result.Verify( type != null, "Type is null" );
        }


        private void testSetValue( ref TestResult r_result )
        {
            Property<int> prop = new Property<int>( k_Prop, 1 );
            prop.SetValue( 2 );

            r_result.Compare( prop.Value == 2, true, "Value did not change" );
        }


        private void testTrySetValue( ref TestResult r_result )
        {
            Property<int> prop = new Property<int>( k_Prop, 1 );
            object objTemp = new object();
            prop.TrySetValue( objTemp );

            r_result.Verify( prop.Value == 1, "Value must not change" );

            prop.TrySetValue( 2 );
            r_result.Compare( prop.Value == 2, true, "Value should change" );
        }


        private void testCopySerialized( ref TestResult r_result )
        {
            Property<int> prop = new Property<int>( k_Prop, 1 );
            r_result.Compare( prop.Value, 1, "Value must not change" );

            Property<int> prop2 = prop.CopySerialized() as Property<int>;
            r_result.CompareNot( prop2, null, "Property should be copied" );

            prop.SetValue( 2 );
            r_result.Compare( prop2.Value, 1, "Value should not change" );
        }


        public override void Cleanup()
        {

        }


        public override void CleanupTest()
        {

        }


        public override void Init()
        {
            addTest( testProperty, "Instantiate a property" );
            addTest( testType, "Set a meta type for a property" );
            addTest( testSetValue, "Set value" );
            addTest( testTrySetValue, "Set value" );
            addTest( testCopySerialized, "Copies property" );
        }


        public override void Prepare()
        {
        }


        public override void PrepareTest()
        {

        }
    }
}
