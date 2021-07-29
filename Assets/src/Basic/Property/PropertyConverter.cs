using HAN.Lib.Stat;
using HAN.Lib.Structure;

namespace HAN.Lib.Basic
{
    public static class PropertyConverter
    {
        /**
         * Converts a property with Type T to the target type V if possible, else returns null
         */
        public static V ChangeType<V, T>( Property<T> a_property ) where V : class, IProperty
        {
            if( typeof( V ) == typeof( Stat.IStat ) )
            {
                if( a_property.Value is int )
                {
                    return new Stat.StatInt( a_property.Id, (int) (object) a_property.Value, null ) as V;
                }
                else if( a_property.Value is string )
                {
                    return new Stat.StatString( a_property.Id, (string) (object) a_property.Value, null ) as V;
                }
                else if( a_property.Value is Key )
                {
                    return new Stat.StatKey( a_property.Id, (Key) (object) a_property.Value, null ) as V;
                }
                else
                {
                    return new Stat.Stat<T>( a_property.Id, a_property.Value, null ) as V;
                }
            }

            return null;
        }


        /**
         * Creates empty stat with the same type as provided 
         */
        public static IStat CreateNewEmpty( IStat a_type ) 
        {
            if( a_type is StatInt ) {
                return new StatInt( a_type.Id, 0, null );
            }
            else if( a_type is StatString ) {
                return new StatString( a_type.Id, "", null );
            }
            else if( a_type is StatKey ) {
                return new StatKey( a_type.Id, "", null );
            }
            else if( a_type is Stat<bool> )  {
                return new Stat<bool>( a_type.Id, false, null );
            }

            HAN.Debug.Logger.Error( "PropertyConverter",
                                string.Format( "cant create empty for {0}",
                                               a_type.Id ) );

            return null;
        }
    }
}