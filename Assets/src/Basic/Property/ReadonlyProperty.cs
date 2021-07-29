using HAN.Lib.Structure;

namespace HAN.Lib.Basic
{
    /**
     * A property is a key value pair, that can identify variables, which can be retrievend
     * with generic methods; setting is not possible with ReadonlyProperty
     * But the used object can be modified!
     */
    public class ReadonlyProperty<T> : Property<T>
    {
        public static readonly Key k_ReadonlyProperty = KeyFactory.Create( "ReadonlyProperty" );
        public override MetaType Type() { return new MetaType( k_ReadonlyProperty ); }


        public ReadonlyProperty( Key a_id, T a_val )
          : base( a_id, a_val )
        {
        }


        public override bool TrySetValue( object a_val )
        {
            return false;
        }


        public override void SetValue( T a_val )
        {
            return;
        }
    }
}