using UnityEngine;
using UnityEditor;
using HAN.Lib.Structure;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

#if UNITY_EDITOR
namespace HAN.Editor
{
    /**
     * Editor of KeyHolder. Will show selection drop downs 
     * to select KeyCollection in scene and Key on selected KeyCollection
     */
    [CustomEditor( typeof( KeyCollectionEntry ) )]
    public class KeyCollectionEntryEditor : UnityEditor.Editor
    {
        private KeyCollectionEntry m_target;
        private SerializedObject m_targetSO;

        private KeySelector m_targetKeySelection;


        void OnEnable()
        {
            m_target = ( (KeyCollectionEntry) target );
            m_targetSO = new SerializedObject( target );
            
            m_targetKeySelection = new KeySelector();
        }


        public override void OnInspectorGUI()
        {
            Key oldkey = m_target.Key;
            bool save = false;

            if( m_target.Key == null || m_target.Key == Key.Null )
            {
                GUILayout.Label( "Target Key: " );
                m_target.m_key = m_targetKeySelection.SelectKeyFromCollections();

                if( m_target.Key != oldkey )
                {
                    save = true;
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label( "Target: " + m_target.Key.Name );
                if( GUILayout.Button( "X" ) )
                {
                    m_target.m_key = Key.Null;
                    m_targetKeySelection.ResetSelection();
                    save = true;
                }
                EditorGUILayout.EndHorizontal();
            }

            if( save )
            {
                m_targetSO.ApplyModifiedProperties();
                m_targetSO.Update();
                EditorUtility.SetDirty( m_target );
                EditorSceneManager.MarkSceneDirty( m_target.gameObject.scene );
            }
        }
    }
}
#endif