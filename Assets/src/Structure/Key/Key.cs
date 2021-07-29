using UnityEngine;


namespace HAN.Lib.Structure
{
    /**
     * Key for unique and fast hash comperision
     */
    [System.Serializable]
    public class Key : System.Object
                     , System.IEquatable<Key>
    {
        public static readonly Key Null = KeyFactory.Create( "Null" );

        public uint Hash { get { return m_hash; } }
        public string Name { get { return m_name; } }

        [SerializeField]
        private string m_name;

        [SerializeField]
        private uint m_hash;

        private bool m_autoKey;


        public Key( string a_name, uint a_hash )
        {
            m_name = a_name;
            m_hash = a_hash;
        }


        public Key( Key a_autoNamespace, uint a_autoHash )
        {
            m_name = a_autoNamespace.Name + ":" + a_autoHash;
            m_hash = a_autoHash;
            m_autoKey = true;
        }


        /// Dont call this manually; use the factory
        public Key( uint a_hash )
        {
            m_hash = a_hash;
        }


        public static implicit operator Key( string a_key )
        {
            return KeyFactory.Create( a_key );
        }


        public static bool operator ==( Key a_a, Key a_b )
        {
            if( System.Object.ReferenceEquals( a_a, a_b ) )  {
                return true;
            }

            // have to checked later, as we have to support Key != null
            // which is checked by ReferenceEquals
            if( a_a is null
             || a_b is null ) {
                return false;
            }

            if( !a_a.m_autoKey && !a_b.m_autoKey ) { 
                // compare only hashes
                return a_a.m_hash == a_b.m_hash;
            }
            else {
                // compare also text so we avoid collision 
                return a_a.m_hash == a_b.m_hash
                    && a_a.Name == a_b.Name;
            }
        }


        public static bool operator !=( Key a_a, Key a_b )
        {
            return !( a_a == a_b );
        }


        public bool IsEmpty()
        {
            return m_hash == 0;
        }


        public static Key operator +( Key a_a, Key a_b )
        {
            if( a_a == null 
             || a_a.IsEmpty() )
                return a_b;

            if( a_b == null 
             || a_b.IsEmpty() )
                return a_a;

            return KeyFactory.Create( a_a.Name + "." + a_b.Name );
        }


        public override bool Equals( System.Object a_obj )
        {
            if( a_obj is Key other ) {
                return this == other;
            }

            return false;
        }


        public bool Equals( Key a_other )
        {
            return this == a_other;
        }


        public override int GetHashCode()
        {
            return (int) m_hash;
        }


        public override string ToString()
        {
            return m_name;
        }
    }
}