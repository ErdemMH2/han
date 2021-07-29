using UnityEngine;
using HAN.Lib.Structure;
using System;
using UnityEditor;


namespace HAN.Lib.Editor
{
    public abstract class KeyCollectionCreatorWindow : EditorWindow
    {
        private string m_subFolder;


        private void OnGUI()
        {
            m_subFolder = EditorGUILayout.TextField( "Subfolder", m_subFolder );
            GUILayout.Space( 10 );
            
            drawGUI();
        }


        protected abstract void drawGUI();


        protected void createKeyCollectionAsset( KeyCollection p_keyCollection )
        {
            if( checkKeyFields( p_keyCollection ) )
            {
                string subfolder = ( m_subFolder != "" ) ? ( m_subFolder + "/" ) : m_subFolder;
                string folder = "Assets/Static/Keys/" + subfolder + p_keyCollection.Type().Type.Name + ".asset";
                HAN.Lib.File.Editor.FolderLib.CreateFolderHierarchyInAssets( folder );

                AssetDatabase.CreateAsset( p_keyCollection, folder );
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                EditorUtility.FocusProjectWindow();
                Selection.activeObject = p_keyCollection;

                UnityEngine.Debug.Log( "Asset Created" );
            }
        }


        /**
         * This function will iterate over all fields of the KeyCollection by using reflection. 
         * For every uninstanciated field a error message ist generated.
         */
        protected bool checkKeyFields( KeyCollection a_keyCollection )
        {
            bool allCorrect = true;

            System.Reflection.FieldInfo[] keyFields = a_keyCollection.GetType().GetFields();
            for( int i = 0; i < keyFields.Length; i++ )
            {
                // Determine whether or not each field is a special name. 
                if( keyFields[i].FieldType == typeof( Key ) )
                {
                    var valObj = keyFields[i].GetValue( a_keyCollection );

                    if( valObj != null )
                    {
                        Key key = (Key) valObj;
                        if( Array.IndexOf( a_keyCollection.Keys, key ) > 0 )
                        {
                            allCorrect &= false;
                            UnityEngine.Debug.LogError( a_keyCollection.Type().ObjectName + " key field was not added to list: " + keyFields[i].Name );
                        }
                    }
                    else
                    {
                        allCorrect &= false;
                        UnityEngine.Debug.LogError( a_keyCollection.Type().ObjectName + " key field is null: " + keyFields[i].Name );
                    }
                }
            }

            return allCorrect;
        }
    }
}
