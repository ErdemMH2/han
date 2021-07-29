

namespace HAN.Lib.Structure
{
    /// <summary>
    /// Singleton patter, which will only allow one instance of the given type.
    /// Instance is only valid until destroy. 
    /// </summary>
    public class Singleton<T> where T : class, new()
    {
        private static object m_lock = new object();
        private static volatile T m_instance = null;


        public static T Instance
        {
            get
            {
                lock( m_lock )
                {
                    if( m_instance == null )
                    {
                        m_instance = System.Activator.CreateInstance< T >();

                        if( m_instance == null )
                        {
                            HAN.Debug.Logger.Error( "Singleton", "[Singleton] Something went really wrong  - cannot create singleton!" );
                        }
                    }

                    return m_instance;
                }
            }
        }


        public void OnDestroy()
        {
            m_instance = null;
        }
    }
}