using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace HAN.Lib.Editor
{
    /**
     * Provides button for starting the serialization and asset creation of KeyCollection via KeyCollectionCreator.
     */
    [CustomEditor( typeof( KeyCollectionCreator ) )]
    public class KeyCollectionCreatorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            KeyCollectionCreator keyCollection = (KeyCollectionCreator) target;
            if( GUILayout.Button( "Build KeyCollection" ) )
            {
                keyCollection.Create();
            }
        }
    }
}
#endif