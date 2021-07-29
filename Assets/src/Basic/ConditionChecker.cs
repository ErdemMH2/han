namespace HAN.Lib.Basic
{
    public delegate bool ConditionCheck();

    /**
     * Will allow to inject condition check from outside 
     * Has a default mode to conditionally switch it from outside
     */
    public class ConditionChecker
    {
        private ConditionCheck m_check;
        private bool m_default = true;


        public void Set( ConditionCheck a_check ) {
            m_check = a_check;
            m_default = false;
        }

        /// will return default value
        public void Default()
        {
            m_default = true;
        }


        public bool Check()
        {
            if( m_default 
             || m_check == null )
            {
                return true;
            }

            return m_check();
        }
    }
}