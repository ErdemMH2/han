using UnityEngine;
using UnityEditor;
using HAN.Lib.Structure;
using System.Collections.Generic;

#if UNITY_EDITOR
namespace HAN.Editor
{
    /**
     * Editor of KeyHolder. Will show selection drop downs 
     * to select KeyCollection in scene and Key on selected KeyCollection
     */
    [CustomEditor( typeof( KeyCollectionKeyHolder ) )]
    public class KeyEditor : UnityEditor.Editor
    {
        private KeyCollectionKeyHolder m_instance;
        private SerializedObject m_instanceSO;
        
        private int m_selectedKeyCollectionIdnx = -1;
        private int m_selectedKeyIndx = -1;


        void OnEnable()
        {
            m_instance = ( (KeyCollectionKeyHolder) target );
            m_instanceSO = new SerializedObject( target );
        }


        public override void OnInspectorGUI()
        {
            if( m_instance == null )
                return;

            if( m_instance.SelectedCollectionIndex >= 0
             && m_instance.Collection != null )
            {
                showCurrentSelection();
            }
            else
            {
                showSelectionForm();
            }
        }

        /**
         * Will show current selection as uneditable text and a button to reset selection.
         */
        private void showCurrentSelection()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField( m_instance.Collection.Type().Type.Name );
            EditorGUILayout.LabelField( m_instance.Key.Name );

            if( GUILayout.Button( "Reset" ) )
            {
                m_instanceSO.Update();

                m_instance.SelectedCollectionIndex = -1;
                m_instance.Collection = null;

                m_selectedKeyCollectionIdnx = -1;
                m_selectedKeyIndx           = -1;

                EditorUtility.SetDirty( m_instance );
            }

            EditorGUILayout.EndVertical();
        }

        /**
         * Will show two dropdown boxes for selection KeyCollection and Key
         */
        private void showSelectionForm()
        {
            m_instanceSO.Update();

            EditorGUILayout.BeginVertical();

            KeyCollectionHolder[] holders = MonoBehaviour.FindObjectsOfType<KeyCollectionHolder>() as KeyCollectionHolder[];
            List<string> keyHolderNames = new List<string>();
            foreach( var holder in holders )
            {
                keyHolderNames.Add( holder.gameObject.name );
            }

            m_selectedKeyCollectionIdnx = EditorGUILayout.Popup( "KeyCollections", m_selectedKeyCollectionIdnx, keyHolderNames.ToArray() );

            if( m_selectedKeyCollectionIdnx >= 0 )
            {
                m_instance.Collection = holders[m_selectedKeyCollectionIdnx].Keys;


                Key[] keyHolderKeys = holders[m_selectedKeyCollectionIdnx].Keys.Keys;
                List<string> keyNames = new List<string>();

                for( int i = 0; i < keyHolderKeys.Length; i++ )
                {
                    keyNames.Add( keyHolderKeys[i].Name );
                }

                m_selectedKeyIndx = EditorGUILayout.Popup( "Key", m_selectedKeyIndx, keyNames.ToArray() );

                if( m_selectedKeyIndx >= 0 )
                {
                    m_instance.SelectedCollectionIndex = m_selectedKeyIndx;
                }
            }

            EditorGUILayout.EndVertical();

            EditorUtility.SetDirty( m_instance );
        }
    }
}
#endif