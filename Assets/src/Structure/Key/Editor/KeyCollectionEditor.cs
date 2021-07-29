using UnityEngine;
using UnityEditor;
using HAN.Lib.Structure;

#if UNITY_EDITOR
namespace HAN.Editor
{
    /**
     * Editor extension to list all available Keys on KeyCollectionHolder
     */
    [CustomEditor( typeof( KeyCollectionHolder ) )]
    public class KeyCollectionEditor : UnityEditor.Editor
    {
        private KeyCollection m_instance;
        private SerializedObject m_instanceSO;
        

        void OnEnable()
        {
            m_instance   = ((KeyCollectionHolder) target).Keys;
            m_instanceSO = new SerializedObject( target );
        }


        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if( m_instance == null )
                return;

            m_instanceSO.Update();

            EditorGUILayout.BeginVertical();
            GUILayout.Label( "Available Keys:" );

            Key[] keyFields = m_instance.Keys;
            for( int i = 0; i < keyFields.Length; i++ )
            {
                GUILayout.TextField( keyFields[i].Name );
                GUILayout.Space( 2 );
            }

            EditorGUILayout.EndVertical();
        }
    }
}
#endif