using UnityEngine;
using UnityEditor;


namespace HAN.Lib.Structure
{
    /**
     * Will deserilize a KeyCollection and provide them to the outer world
     */
    public abstract class KeyCollectionProvider : MonoBehaviour
    {
        public KeyCollection CollectionAsset;

        public abstract bool LoadPublicKeys();
    }
}