using UnityEngine;
using System.Collections.Generic;
using HAN.Lib.Basic;


namespace HAN.Lib.Structure
{
    /**
     * Serialization of keys in a specific order, that is retrieved by a KeyCollectionProvider.
     * This object is used to save Keys as a asset and deserialize it. 
     * A KeyCollection have to be derived and Keys have to be added as public members 
     * and initialized in initFields() with createKey. (Do not use KeyFactory.Create!)
     * Example: KeyTestCollection
     */
    [System.Serializable]
    public class KeyCollection : ScriptableObject
                               , Basic.IHANType
    {
        public static readonly Key k_KeyCollection = KeyFactory.Create( "KeyCollection" );

        public Key Id { get; set; }
        public Key[] Keys { get { return m_keys; } }

        [SerializeField] private Key[] m_keys;


        /**
         * Returns Key on Collection. 
         * If key was not found null will be returned!
         */
        public Key ReturnKey( int a_indx )
        {
            if( a_indx < m_keys.Length 
             && a_indx >= 0             )
            {
                return m_keys[a_indx];
            }

            HAN.Debug.Logger.Error( Type().Type,
                        string.Format( "key index {0} does not exist",
                        a_indx ) );

            return null;
        }


        /**
         * Will initialize the KeyCollection and prepare for serialization
         */
        public void Init( Key[] a_keys = null )
        {
            if( Id == null ) {
                Id = k_KeyCollection;
            }

            if( a_keys == null ) { 
                m_keys = initFields();
            }
            else {
                m_keys = a_keys;
            }
        }


        /**
         * Initialize fields and return them in an array. Only fields initialize via this methods are accessible.
         */
        protected virtual Key[] initFields() { return null; }

        public virtual MetaType Type() { return new MetaType( Id ); }
    }
}