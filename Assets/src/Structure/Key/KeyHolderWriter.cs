using UnityEngine;
using System.Collections;


namespace HAN.Lib.Structure
{
    public class KeyHolderWriter : KeyHolder
    {
        public override Key Key { get; protected set; }
        public void SetKey( Key a_id ) { Key = a_id; }
    }
}