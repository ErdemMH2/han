
namespace HAN.Lib.Basic
{
    class PropertyObject : HObject
    {
        public override MetaType Type() { return new MetaType( Structure.KeyFactory.Create("PropertyObject") ); }


        public PropertyObject( PropertyCollection a_collection ) 
        {
            foreach( var property in a_collection )
            {
                AddProperty( property );
            }
        }
    }
}
