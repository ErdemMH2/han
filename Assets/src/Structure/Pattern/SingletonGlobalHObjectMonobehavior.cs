using HAN.Lib.Basic;
using UnityEngine;

namespace HAN.Lib.Structure
{
    /// <summary>
    /// Singleton patter, which will only allow one instance of the given type.
    /// Instance is only valid until destroy. 
    /// </summary>
    public class SingletonGlobalHObjectMonobehavior<T> : HObjectMonoBehavior where T : MonoBehaviour
    {
        private static object m_lock = new object();
        private string        m_singletonName;
        private static T      m_instance;


        public static T Instance
        {
            get
            {
                lock( m_lock )
                {
                    if( m_instance == null )
                    {
                        m_instance = (T) FindObjectOfType( typeof( T ) );

                        if( FindObjectsOfType( typeof( T ) ).Length > 1 )
                        {
                            HAN.Debug.Logger.Error( "Singleton(mono)", "[Singleton] Something went really wrong " +
                                " - there should never be more than 1 singleton!" +
                                " Reopening the scene might fix it." );
                            return m_instance;
                        }

                        if( m_instance == null )
                        {
                            GameObject singleton = new GameObject();
                            m_instance = singleton.AddComponent<T>();
                            singleton.name = "Singleton." + typeof( T ).ToString();

                            DontDestroyOnLoad( singleton );

                            HAN.Debug.Logger.Log( "Singleton(mono)", "[Singleton] An instance of " + typeof( T ) +
                                " is needed in the scene, so '" + singleton +
                                "' was created with DontDestroyOnLoad." );
                        }
                        else
                        {
                            HAN.Debug.Logger.Log( "Singleton(mono)", "[Singleton] Using instance already created: " +
                                m_instance.gameObject.name );
                        }
                    }

                    return m_instance;
                }
            }

            protected set
            {
                if( m_instance != null )
                    HAN.Debug.Logger.Error( "Singleton(mono)", "[Singleton] Something went really wrong  - there should never be more than 1 singleton!" );

                m_instance = value;
            }
        }

        public override IHANObject HObject
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public void OnDestroy()
        {
            m_instance = null;
        }
    }
}