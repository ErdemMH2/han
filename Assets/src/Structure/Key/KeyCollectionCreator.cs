using UnityEngine;
using UnityEditor;
using HAN.Lib.Structure;


#if UNITY_EDITOR
namespace HAN.Lib.Editor
{
    /**
     * Creates a KeyCollection Asset using a derived class of KeyCollection defined in MonoScript KeyCollectionClass.
     * KeyCollectionCreator will call Init on KeyCollection to prepare serialization and save the asset on static path. 
     * With Folder a subfolder under Keys can be added.
     */
    public class KeyCollectionCreator : MonoBehaviour
    {
        public static void CreateAsset( KeyCollection p_keyCollection, string p_subfolder = "" )
        {
            p_keyCollection.Init();

            string subfolder = ( p_subfolder != "" ) ? ( p_subfolder + "/" ) : p_subfolder;
            string folder = "Assets/Static/Keys/" + subfolder + p_keyCollection.Type().Type.Name + ".asset";
            HAN.Lib.File.Editor.FolderLib.CreateFolderHierarchyInAssets( folder );

            AssetDatabase.CreateAsset( p_keyCollection, folder );
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = p_keyCollection;
        }


        public MonoScript KeyCollectionClass;

        public string Folder;

        public void Create()
        {
            UnityEngine.Debug.Log( "Creating Asset" );
            ScriptableObject keyCollection = ScriptableObject.CreateInstance( KeyCollectionClass.GetClass() );
            if( keyCollection is KeyCollection )
            { 
                KeyCollection newKeyCollection = (KeyCollection) keyCollection;
                CreateAsset( newKeyCollection, Folder );
                UnityEngine.Debug.Log( "Asset Created" );
            }
            else
            {
                UnityEngine.Debug.LogError( "Selected KeyCollectionClass is not of type KeyCollection!" );
            }
        }
    }
}

#endif