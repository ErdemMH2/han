using HAN.Lib.Basic;
using UnityEngine;

namespace HAN.Lib.Structure.Unity
{
    public class GameObjectId : IncrementalId
    {
        public GameObject GameObject { get; private set; }


        public GameObjectId( GameObject a_go ) 
          : base( a_go.name )
        {
            GameObject = a_go;
        }


        public override bool Equals( System.Object a_obj )
        {
            GameObjectId other = a_obj as GameObjectId;
            if( other != null ) {
                return GameObject == other.GameObject;
            }

            return false;
        }


        public override int GetHashCode() {
            return GameObject.GetHashCode();
        }
    }
}