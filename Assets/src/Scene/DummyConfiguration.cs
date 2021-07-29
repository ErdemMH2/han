using UnityEngine;

namespace HAN.Lib.Scene
{ 
    /**
     * Abstract parent class for all DummyConfigurations, which will be retrieved by 
     * ConfigurationSceneTransfer to initialize configurations for test and development purposes
     */
    public abstract class DummyConfiguration : MonoBehaviour
    {
        public abstract void Init();
    }
}