using HAN.Lib.Structure;
using UnityEngine;

namespace HAN.Debug
{
    public class Logger
    {
        public static readonly Key k_Log = KeyFactory.Create( "Log" );
        public static readonly Key k_Warning = KeyFactory.Create( "Warning" );
        public static readonly Key k_Error = KeyFactory.Create( "Error" );
        public static readonly Key k_Assert = KeyFactory.Create( "Assert" );

        private static object s_lock = new object();
        protected static volatile ILoggerImpl s_instance = null; // the implementation can be changed on the fly

        // do not use singleton class, because every other class's log output depends on this
        // we dont want any external dependency for this class
        private static ILoggerImpl instance
        {
            get
            {
                lock( s_lock )
                {
                    if( s_instance == null )
                    {
                        Application.SetStackTraceLogType( LogType.Log, StackTraceLogType.None );

                        s_instance = new LoggerImpl();
                        UnityEngine.Assertions.Assert.IsNotNull<ILoggerImpl>( s_instance, 
                        "[Logger] Something went really wrong  - cannot create Logger singleton!" );
                    }

                    return s_instance;
                }
            }
        }


        public static void Log( Key a_tag, string a_message, params object[] a_params )
        {
            instance.AddMessage( k_Log, a_tag, string.Format(a_message, a_params) );
        }


        public static void Warning( Key a_tag, string a_message, params object[] a_params )
        {
            instance.AddMessage( k_Warning, a_tag, string.Format( a_message, a_params ) );
        }


        public static void Error( Key a_tag, string a_message, params object[] a_params )
        {
            instance.AddMessage( k_Error, a_tag, string.Format( a_message, a_params ) );
        }


        public static void Assert( Key a_tag, string a_message, params object[] a_params )
        {
            instance.AddMessage( k_Assert, a_tag, string.Format( a_message, a_params ) );
        }
    }
}