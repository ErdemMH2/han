using HAN.Lib.Structure;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HAN.Lib.Test
{ 
    public class TestRunner : MonoBehaviour 
    {
        public static readonly Key k_TestRunner = KeyFactory.Create( "TestRunner" );

        protected ITestExecutor m_executor;
        protected TestSuite[] m_testSuites;

        protected bool m_hasRunned = false;
    
        protected virtual ITestExecutor createExecutor() { return new TestExecutor(); }
        

        protected void Awake()
        {
            Debug.FilterLogger.Activate();

            m_hasRunned = false;

            m_testSuites = gameObject.GetComponentsInChildren< TestSuite >();
            m_executor = createExecutor();
        }

        
        protected void Update()
        {
            if( !m_hasRunned ) { 
                StartCoroutine( Run() );
            }
        }


        protected IEnumerator Run()
        {
            if( !m_hasRunned )
            {
                m_hasRunned = true;

                yield return new WaitForSeconds( 0.1f ); // wait for starting
                //yield return Lib.Test.Emulator.MouseEmulator.Calibrate(); // Calibrate Input

                List<string> failedSuites = new List<string>();

                foreach( var testSuite in m_testSuites )
                {
                    var testSuiteResult = new TestSuiteResult();

                    HAN.Debug.Logger.Log( k_TestRunner, "<b>=== Starting tests for  {0}  =============</b>", testSuite.Name );

                    foreach( var test in testSuite.Tests )
                    {
                        HAN.Debug.Logger.Log( k_TestRunner, "<b>---  {0}  Tests ---------------</b>", test.Name );

                        yield return m_executor.Execute( test, testSuiteResult );
                    }

                    HAN.Debug.Logger.Log( k_TestRunner, "<b><color=red>FAILED: {0}</color>  |   <color=green>Succeeded: {1}</color></b>"
                                                      , testSuiteResult.Failed, testSuiteResult.Succeeded );
                    HAN.Debug.Logger.Log( k_TestRunner, "<b>=== End all tests for  {0}  =============</b>", testSuite.Name );

                    if( testSuiteResult.Failed > 0 ) {
                        failedSuites.Add( testSuite.Name );
                    }
                }


                if( failedSuites.Count > 0 ) {
                    string failedTestSuiteNames = failedSuites.Aggregate( ( i, j ) => i + " " + j );
                    HAN.Debug.Logger.Log( k_TestRunner, "<b><color=red>FAILED SUITES: {0}</color></b>", failedTestSuiteNames );
                }
            }
        }
    }
}