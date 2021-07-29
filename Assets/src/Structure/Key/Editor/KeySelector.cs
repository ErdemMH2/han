using UnityEngine;
using UnityEditor;
using HAN.Lib.Structure;
using System.Collections.Generic;


namespace HAN.Editor
{
    public class KeySelector
    {
        private int m_selectedKeyCollectionIdnx = -1;
        private int m_selectedKeyIndx = -1;
        private KeyCollection m_collection;
        private Key m_key;


        public Key SelectKeyFromCollections()
        {
            KeyCollectionHolder[] holders = MonoBehaviour.FindObjectsOfType<KeyCollectionHolder>() as KeyCollectionHolder[];
            List<string> keyHolderNames = new List<string>();
            foreach( var holder in holders )
            {
                keyHolderNames.Add( holder.gameObject.name );
            }

            m_selectedKeyCollectionIdnx = EditorGUILayout.Popup( "KeyCollections", m_selectedKeyCollectionIdnx, keyHolderNames.ToArray() );

            if( m_selectedKeyCollectionIdnx >= 0 )
            {
                m_collection = holders[m_selectedKeyCollectionIdnx].Keys;


                Key[] keyHolderKeys = holders[m_selectedKeyCollectionIdnx].Keys.Keys;
                List<string> keyNames = new List<string>();

                for( int i = 0; i < keyHolderKeys.Length; i++ )
                {
                    keyNames.Add( keyHolderKeys[i].Name );
                }

                m_selectedKeyIndx = EditorGUILayout.Popup( "Key", m_selectedKeyIndx, keyNames.ToArray() );

                if( m_selectedKeyIndx >= 0 ) {
                    m_key = keyHolderKeys[m_selectedKeyIndx];
                    return m_key;
                }
            }

            return Key.Null;
        }


        public void ResetSelection() 
        {
            m_selectedKeyCollectionIdnx = -1;
            m_selectedKeyIndx = -1;
        }


        public void ShowCurrentSelection()
        {
            if( m_selectedKeyIndx >= 0 )
            {
                EditorGUILayout.LabelField( m_collection.name + "/" + m_key.Name );

                if( GUILayout.Button( "X" ) )
                {
                    ResetSelection();
                }
            }
        }
    }
}
