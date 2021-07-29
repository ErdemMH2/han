using HAN.Lib.Structure;

namespace HAN.Debug
{
    public class FilterLoggerImpl : LoggerImpl
    {
        private ILogFilter m_filter;


        public override void AddMessage( Key a_level, Key a_tag, string a_message )
        {
            if( m_filter != null )
            {
                if( !m_filter.Filter( a_level, a_tag, a_message ) )
                {
                    base.AddMessage( a_level, a_tag, a_message );
                }
            }
            else
            {
                base.AddMessage( a_level, a_tag, a_message );
            }
        }


        public void InstallFilter( ILogFilter a_filter )
        {
            m_filter = a_filter;
        }


        public void RemoveFilter()
        {
            m_filter = null;
        }
    }
}