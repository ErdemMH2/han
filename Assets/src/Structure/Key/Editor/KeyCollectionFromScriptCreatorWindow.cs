using UnityEngine;
using UnityEditor;
using HAN.Lib.Structure;


namespace HAN.Lib.Editor
{
    public class KeyCollectionFromScriptCreatorWindow : KeyCollectionCreatorWindow
    {
        [MenuItem( "HAN/Basic/Key Collection From Script Creator" )]
        public static void OpenKeyCollectionFromScriptCreator()
        {
            GetWindow<KeyCollectionFromScriptCreatorWindow>( "Key Collection Creator" );
        }


        private MonoScript m_keyCollectionClass;


        protected override void drawGUI()
        {
            GUILayout.Label( "Key Collection Creator", EditorStyles.boldLabel );

            m_keyCollectionClass = EditorGUILayout.ObjectField( "Key Collection Class",
                                                                m_keyCollectionClass, typeof( MonoScript ), true ) as MonoScript;

            if( GUILayout.Button( "Build KeyCollection" ) )
            {
                if( m_keyCollectionClass != null )
                {
                    createFromScript( m_keyCollectionClass );
                }
            }
        }


        protected void createFromScript( MonoScript a_keyCollectionClass )
        {
            KeyCollection keyCollection = ScriptableObject.CreateInstance( a_keyCollectionClass.GetClass() ) as KeyCollection;
            if( keyCollection != null )
            {
                KeyCollection newKeyCollection = (KeyCollection) keyCollection;
                newKeyCollection.Init();
                createKeyCollectionAsset( newKeyCollection );
            }
            else
            {
                UnityEngine.Debug.LogError( "Selected KeyCollectionClass is not of type KeyCollection!" );
            }
        }
    }
}
