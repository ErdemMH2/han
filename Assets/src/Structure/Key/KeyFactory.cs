using System.Collections.Generic;

namespace HAN.Lib.Structure
{
    public class KeyFactory
    {
        private static Dictionary<uint, Key> s_hashTable = new Dictionary<uint, Key>();
        private static Dictionary<string, Key>  s_keyTable  = new Dictionary<string, Key>();


        /// Returns new key, if key was never created before, else pooled one
        public static Key Create( string p_name )
        {
            if( !s_keyTable.TryGetValue( p_name, out Key returnKey ) )
            {
                returnKey = new Key( p_name, External.CityHash.CityHash32( p_name ) );
                storeKey( returnKey );
            }

            return returnKey;
        }


        public static Key Create( string a_name, uint a_hash )
        {
            if( !s_keyTable.TryGetValue( a_name, out Key returnKey ) )
            {
                returnKey = new Key( a_name, a_hash );
                storeKey( returnKey );
            }
            else if( returnKey.Hash != a_hash ) {
                HAN.Debug.Logger.Error( "Keyfactory", "Key collision: " + returnKey.Name );
            }

            return returnKey;
        }


        public static Key Create( Key a_autoNamespace, uint a_autoHash )
        {
            return new Key( a_autoNamespace, a_autoHash );
        }


        public static Key Create( uint p_hash )
        {
            if( s_hashTable.TryGetValue( p_hash, out Key key ) )
            {
                return key;
            }
            else
            {
                HAN.Debug.Logger.Error( "Keyfactory", "Key hash not found " + p_hash );
                return new Key( p_hash );
            }
        }
                

        private static void storeKey( Key a_key )
        {
            s_keyTable[a_key.Name] = a_key;
            s_hashTable[a_key.Hash] = a_key;
        }
    }
}
