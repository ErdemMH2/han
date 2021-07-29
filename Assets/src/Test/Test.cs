using System.Collections.Generic;
using UnityEngine;


namespace HAN.Lib.Test
{
    /**
     * Parent class for all tests
     * To write a test this class has to be derived and test methods have to be members of this child class.
     * The test methods have to be added to the test by using the addTest function. This step has to be done by 
     * overriding in Init().
     */
    public abstract class Test : MonoBehaviour
    {
        public class TestFunction
        {
            public string Description;
        }

        public class FuncTestFunction : TestFunction
        {
            public TestFunctionDelegate Function;
        }

        public class CoroutineTestFunction : TestFunction
        {
            public TestFunctionDelegateCoroutine Function;
        }

        public class AsyncTestFunction : TestFunction
        {
            public System.Func<System.Threading.Tasks.Task> Function;
        }

        public string Name { get { return gameObject.name; } }
        public List< TestFunction > Tests { get; private set; }

        public TestResult NextResult; // For async and coroutine test


        public Test()
          : base()
        {
            Tests = new List< TestFunction >();
        }


        /**
         * Adds a test funciton to the Test.
         */
        protected void addTest( TestFunctionDelegate a_testFunc, string a_desc )
        {
            var testFunc = new FuncTestFunction
            {
                Function = a_testFunc,
                Description = a_desc
            };
            Tests.Add( testFunc );
        }


        /**
         * Adds a test funciton to the Test.
         */
        protected void addTestCoroutine( TestFunctionDelegateCoroutine a_testFunc, string a_desc )
        {
            var testFunc = new CoroutineTestFunction
            {
                Function = a_testFunc,
                Description = a_desc
            };
            Tests.Add( testFunc );
        }


        /**
         * Adds a test funciton to the Test.
         */
        protected void addTestAsync( System.Func<System.Threading.Tasks.Task> a_testFunc, string a_desc )
        {
            var testFunc = new AsyncTestFunction
            {
                Function = a_testFunc,
                Description = a_desc
            };
            Tests.Add( testFunc );
        }


        /**
         * Init test. In this method the test cases (functions) has to be addded. 
         * This will be called once per lifetime.
         */
        public abstract void Init();


        /**
         * This is used to prepare a test. Override if necessary. This is called only once per execution. 
         */
        public abstract void Prepare();


        /**
         * This is used to prepare a test case. Override if necessary. 
         * This is called every time before a test case is started.
         */
        public abstract void PrepareTest();


        /**
         * Will cleanup the test case. Will be called every time after ONE test case.
         */
        public abstract void CleanupTest();


        /**
         * Will cleanup the test. Will be called after ALL tests has been finished.
         */
        public abstract void Cleanup();
    }
}