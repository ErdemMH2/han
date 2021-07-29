using HAN.Lib.Basic;
using HAN.Lib.Structure;


namespace HAN.Lib.Testing
{
    /**
     * Example implementation of KeyTestCollection for manual tests.
     */
    public class TestKeyTestCollection : KeyCollection
    {
        public Key TestKey1;
        public Key Stenght;
        public Key NotInitializedKey;   // WRONG!
        public Key WrongInitializedKey; // also WRONG!


        protected override Key[] initFields()
        {
            TestKey1 = KeyFactory.Create( "TestKey1" );
            Stenght = KeyFactory.Create ( "Stenght" );

            WrongInitializedKey = KeyFactory.Create( "WrongInitializedKey" ); // WRONG: not added to Return!

            return new Key[] { TestKey1, Stenght };
        }


        public override MetaType Type()
        {
            return new MetaType( KeyFactory.Create( "TestKeyTestCollection" ), KeyFactory.Create( "TestKeyTestCollection_Instance" ) );
        }
    }
}