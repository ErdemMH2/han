using HAN.Lib.Structure;
using System;
using System.IO;
using UnityEngine;

namespace HAN.Debug
{
    public class LoggerImpl : ILoggerImpl
    {
        private FileStream m_fileStream;
        private StreamWriter m_streamWriter;
        protected ILogHandler m_defaultLogHandler = UnityEngine.Debug.unityLogger.logHandler;


        public LoggerImpl()
        {
            string filePath = Application.persistentDataPath + "/logger_output.txt";

            m_fileStream = new FileStream( filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite );
            m_streamWriter = new StreamWriter( m_fileStream );

            // Replace the default debug log handler
            UnityEngine.Debug.Log( "Loggs are located in: " + filePath );
#if !UNITY_EDITOR
            UnityEngine.Debug.unityLogger.logHandler = this;
#endif
        }


        public void LogFormat( UnityEngine.LogType a_logType, UnityEngine.Object a_context, string a_format, params object[] a_args )
        {
            output( "[Unity] " + String.Format( a_format, a_args ) );
            m_defaultLogHandler.LogFormat( a_logType, a_context, a_format, a_args );
        }


        public void LogException( Exception a_exception, UnityEngine.Object a_context )
        {
            output( "[Unity] " + a_exception.Message );
            m_defaultLogHandler.LogException( a_exception, a_context );
        }


        protected UnityEngine.LogType keyToLogType( Key a_type )
        {
            if( Logger.k_Log == a_type ) {
                return UnityEngine.LogType.Log;
            }
            else if( Logger.k_Warning == a_type ) {
                return UnityEngine.LogType.Warning;
            }
            else if( Logger.k_Error == a_type ) {
                return UnityEngine.LogType.Error;
            }
            else if( Logger.k_Assert == a_type ) { 
                return UnityEngine.LogType.Assert;
            }

            return UnityEngine.LogType.Log;
        }


        public virtual void AddMessage( Key a_level, Key a_tag, string a_message )
        {
            output( string.Format( "[{0}] [{1}] {2} : {3}"
                  , a_level
                  , DateTime.UtcNow.ToString( "dd.MM HH:mm:ss.fff"
                                            , System.Globalization.CultureInfo.InvariantCulture )
                  , a_tag, a_message ) );

            m_defaultLogHandler.LogFormat( keyToLogType( a_level ), null, a_message );
        }


        private void output( string a_message )
        {
            m_streamWriter.WriteLine( a_message );
            m_streamWriter.Flush();
        }
    }
}