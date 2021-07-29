using UnityEngine;


namespace HAN.Lib.Test
{ 
    public class TestSuite : MonoBehaviour
    {
        [HideInInspector]
        public Test[] Tests;

        public string Name { get { return gameObject.name; } }

        private void Awake()
        {
            Tests = gameObject.GetComponentsInChildren< Test >();
        }
    }
}