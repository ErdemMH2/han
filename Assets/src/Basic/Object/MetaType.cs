using HAN.Lib.Structure;

namespace HAN.Lib.Basic
{
    /**
     * Stores type information via Key for fast comparision and identification without casting
     */
    public class MetaType : System.Object
    {
        /**
         * Unique idendifier for object, can be used to find it
         */
        public Key ObjectName { get; set; }

        /**
         * Type of the object
         */
        public Key Type { get; private set; }
        

        public MetaType( Key a_type ) 
          : this( a_type, null )
        {  
        }


        public MetaType( Key a_type, Key a_objectName )
        {
            if( a_type == null )
            {
                HAN.Debug.Logger.Error( "MetaType", "No type defined for MetaType" );
                throw new System.Exception( "No Type is not allowed!" );
            }

            Type = a_type;

            if( a_objectName == null ) { 
                ObjectName = Type;
            }
            else { 
                ObjectName = a_objectName;
            }
        }


        public override string ToString()
        {
            return Type.Name + "." + ObjectName.Name;
        }
    }
}