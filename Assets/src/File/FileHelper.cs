using System.IO;
using UnityEngine;

namespace HAN.Lib.Basic.FileManagement
{
    public static class FileHelper
    {
        public static string FileWritablePath( string a_persistantFile, bool a_overwrite = false )
        {
            string filePath = Path.Combine( Application.persistentDataPath, a_persistantFile );
            bool fileExists = System.IO.File.Exists( filePath );
            if( !fileExists )
            {
                Directory.CreateDirectory( Path.GetDirectoryName( filePath ) );
            }

            if( !fileExists
             || a_overwrite ) 
            {
                try
                {
                    string resourceFile = System.IO.Path.ChangeExtension( a_persistantFile, null );
                    TextAsset asset = Resources.Load<TextAsset>( resourceFile );
                    System.IO.File.WriteAllBytes( filePath, asset.bytes );
                }
                catch( System.Exception e)
                {
                    Debug.Logger.Assert( "FileHelper"
                                       , "Cant load or write file {0} received exception {1}"
                                       , filePath, e);
                }
            }

            return filePath;
        }
    }
}