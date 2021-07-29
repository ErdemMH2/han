using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAN.Lib.Structure
{ 
    public class ManualKeyCollectionHolder : KeyCollectionHolder
    {
        [SerializeField] private string[] m_keys;
        private KeyCollection m_keyCollection = null;

        public override KeyCollection Keys { get { if( m_keyCollection == null ) init(); return m_keyCollection; } set { } }


        private void init()
        {
            m_keyCollection = ScriptableObject.CreateInstance<KeyCollection>();
            List<Key> keys = new List<Key>();
            foreach( var keyString in m_keys ) {
                keys.Add( keyString );
            }

            m_keyCollection.Init( keys.ToArray() );
        }
    }
}