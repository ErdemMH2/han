using HAN.Lib.Basic;
using UnityEngine;


namespace HAN.Lib.Structure
{
    /**
     * A key reference in a KeyCollection for usage as MonoBehavior.
     * The KeyHolder will not save or copy the Key, it will point to the KeyCollections 
     * array via a simple index.
     */
    public class KeyCollectionKeyHolder : KeyHolder
    {
        [HideInInspector]
        public KeyCollection Collection;

        [HideInInspector]
        public int SelectedCollectionIndex = -1;

        public override Key Key { get { return ( Collection != null ) ? Collection.ReturnKey( SelectedCollectionIndex ) : null; } protected set { } }
    }
}