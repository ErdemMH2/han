using HAN.Lib.Basic;
using UnityEngine;


namespace HAN.Lib.Structure
{
    /**
     * A key reference in a KeyCollection for usage as MonoBehavior.
     * The KeyHolder will copy and save the Key
     */
    public class KeyCollectionEntry: KeyHolder
    {   
        // TODO: make private again
        [HideInInspector] public Key m_key; // only public for the editor; DO NOT USE

        public override Key Key { get { return m_key; } protected set { /* DO NOTHING */ } }
    }
}