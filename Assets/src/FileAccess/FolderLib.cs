using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HAN.Lib.File.Editor
{ 
#if UNITY_EDITOR
    public class FolderLib 
    {
        /**
         * Creates all folders for the path. The path has to be in this form:  
         * Assets/Folder1/Subfolder/filename.assets
         * Assets has to defined as the root.
         */
        public static void CreateFolderHierarchyInAssets( string a_pathWithFileName )
        {
            string[] folders = a_pathWithFileName.Split( '/' );
            string currentFolder = "Assets";    // => folders[0]/

            for( int i = 1; i < folders.Length - 1; i++ )
            {
                if( !AssetDatabase.IsValidFolder( currentFolder + "/" + folders[i] ) )
                {
                    AssetDatabase.CreateFolder( currentFolder, folders[i] );
                }

                currentFolder += "/" + folders[i];
            }
        }
    }
#endif
}