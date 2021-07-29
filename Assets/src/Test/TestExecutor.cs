using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HAN.Lib.Structure;

namespace HAN.Lib.Test
{ 
    public class TestExecutor : ITestExecutor
    {
        public static readonly Key k_TestExecutor = KeyFactory.Create( "TestExecutor" );

        public TestExecutor() 
        {
        }


        public IEnumerator Execute( Test a_executed, TestSuiteResult a_suiteResult )
        {
            // Init and prepare test
            try { 
                a_executed.Init(); 
                a_executed.Prepare(); 
            } 
            catch( System.Exception e ) { 
                HAN.Debug.Logger.Error( k_TestExecutor, e.Message ); 
            }

            yield return new WaitForEndOfFrame(); // wait for one frame to prepare everything

            foreach( var test in a_executed.Tests )
            {
                // Prepare test case
                try { 
                    a_executed.PrepareTest(); 
                } 
                catch( System.Exception e ) { 
                    HAN.Debug.Logger.Error( k_TestExecutor, "<b><color=red>PREPARE FAILED</color> {0}</b>\n{1}"
                                                          , test.Description, e.Message ); 
                }

                //***************** START TEST ***************
                var result = new TestResult( test );
                a_executed.NextResult = result;

                Debug.FilterLogger.Install( result.Filter );

                if( test is Test.FuncTestFunction funcTest ) 
                { 
                    try { funcTest.Function( ref result ); } catch( System.Exception e ) { result.Verify( false, e.Message ); }
                }
                else if( test is Test.CoroutineTestFunction coTest )
                {   // WARNING this block can have uncaught exceptions
                    yield return coTest.Function();
                    yield return new WaitForEndOfFrame();
                }
                else if( test is Test.AsyncTestFunction asyncTest ) 
                {
                    try
                    {
                        System.Threading.Tasks.Task task = asyncTest.Function();
                        task.Wait();
                    }
                    catch( System.Exception e ) { result.Verify( false, e.Message ); }
                }

                Debug.FilterLogger.RemoveFilter();
                result.VerifyLog();
                a_suiteResult.AddResult( result );
                //***************** END TEST ***************

                // cleanup test case
                try { 
                    a_executed.CleanupTest(); 
                } 
                catch( System.Exception e )  { 
                    HAN.Debug.Logger.Error( k_TestExecutor
                                          , "<b><color=red>CLEAN FAILED</color> {0}</b>\n{1}"
                                          , test.Description, e.Message ); 
                }

                // output results
                if( result.Success ) {
                    HAN.Debug.Logger.Log( k_TestExecutor, "<color=green>Succeeded</color> {0}", test.Description );
                }
                else {
                    HAN.Debug.Logger.Error( k_TestExecutor, "<b><color=red>FAILED</color> {0}</b>\n---------------------", test.Description );
                }
            }

            // cleanup test
            try { 
                a_executed.Cleanup(); 
            } 
            catch( System.Exception e ) { 
                HAN.Debug.Logger.Error( k_TestExecutor, e.Message ); 
            }

            yield return null;
        }
    }
}