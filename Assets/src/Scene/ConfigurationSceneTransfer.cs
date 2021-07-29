using HAN.Lib.Structure;
using System.Collections;
using UnityEngine;

namespace HAN.Lib.Scene
{
    /**
     * Transfers configurations from one scene to another
     * <remarks>Do not overuse Singleton behavior, get it from one central point and delegate it to all users!</remarks>
     */
    public class ConfigurationSceneTransfer : SingletonGlobalMono<ConfigurationSceneTransfer>
    {
        private Hashtable m_configurations = new Hashtable();

#if UNITY_EDITOR
        private void Awake()
        {
            var dummies = GameObject.FindObjectsOfType<DummyConfiguration>();
            foreach( var dummy in dummies ) {
                dummy.Init();
            }
        }
#endif

        public void RemoveConfig( Key a_configType )
        {
            m_configurations.Remove( a_configType );
        }


        /// inserts config into hashtable; only one config per type is allowed!
        public void InsertConfig( Key a_configType, object a_configuration )
        {
            m_configurations[a_configType] = a_configuration;
        }


        /// returns if configuration exists
        public bool ContainsConfig( Key a_configType )
        {
            return m_configurations.ContainsKey( a_configType );
        }


        /// returns configuration with a_configType key, will return null if not found!
        public T RetrieveConfig<T>( Key a_configType )
        {
            if( m_configurations.ContainsKey( a_configType ) )
            {
                if( m_configurations[a_configType] is T ) { 
                    return (T) m_configurations[a_configType];
                }
                else { 
                    HAN.Debug.Logger.Error( "ConfigurationSceneTransfer",
                                            string.Format( "Cant cast type of {0} to {1}",
                                            a_configType.ToString(), typeof(T) ) );
                }
            }
            else { 
                HAN.Debug.Logger.Error( "ConfigurationSceneTransfer",
                                        string.Format( "Cant find key {0}",
                                        a_configType.ToString() ) );
            }

            return default(T);
        }
    }
}
