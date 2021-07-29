using UnityEngine;


namespace HAN.Lib.Structure
{
    /**
     * The KeyHolder will convert the gameObject name into a useable Key.
     */
    public class ObjectNameKeyHolder : KeyHolder
    {
        public override Key Key { get { if( m_key == null || m_key.IsEmpty() ) m_key = KeyFactory.Create( gameObject.name ); return m_key; } protected set { } }

        private Key m_key = null;
    }
}