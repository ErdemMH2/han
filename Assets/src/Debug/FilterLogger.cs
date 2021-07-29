using HAN.Lib.Structure;

namespace HAN.Debug
{
    public class FilterLogger : Logger
    {
        public static void Activate()
        {
            if( !(s_instance is FilterLoggerImpl) )
            {
                s_testLoggerInstance = new FilterLoggerImpl();
                s_instance = s_testLoggerInstance; // overwrite singleton
            }
        }

        private static FilterLoggerImpl s_testLoggerInstance;


        public static void Install( ILogFilter a_filter )
        {
            s_testLoggerInstance.InstallFilter( a_filter );
        }


        public static void RemoveFilter()
        {
            s_testLoggerInstance.RemoveFilter();
        }
    }
}
