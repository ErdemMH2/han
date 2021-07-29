using HAN.Debug;
using HAN.Lib.Basic;
using HAN.Lib.Extension.Math;
using HAN.Lib.Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HAN.Lib.Test
{
    public delegate void TestFunctionDelegate( ref TestResult r_result );
    public delegate IEnumerator TestFunctionDelegateCoroutine();

    public class TestResult
    {
        public static readonly Key k_TestResult = KeyFactory.Create( "TestResult" );

        public bool Success { get; private set; }

        public List<string> ErrorMesssages { get; private set; }

        public Test.TestFunction Test { get; private set; }

        public delegate bool Evaluate();

        private const float s_invalidWaitTime = -100000f;
        private float m_currentWaitTime;

        public ILogFilter Filter { get { return m_testLogFilter; } }
        private TestLogFilter m_testLogFilter = new TestLogFilter();


        public TestResult( Test.TestFunction a_target )
        {
            Success = true;
            Test = a_target;
            ErrorMesssages = new List<string>();

            resetCurrentWaitTime();
        }


        /**
         * Waits for the delegate to return true. Will wait until a_maxWaitTime is reached and fail then.
         * If the expression in a_del is true before the timeout the test will succeed. 
         */
        public IEnumerator WaitFor( Evaluate a_del, string a_message, float a_maxWaitTime )
        {
            resetCurrentWaitTime();

            while( hasWaitTime( a_maxWaitTime ) )
            {
                if( !a_del() )
                {
                    yield return new WaitForFixedUpdate();
                }
                else
                {
                    break;
                }
            }

            // not breaked = timeout
            if( !hasWaitTime( a_maxWaitTime )
             && Success )
            {
                failed( a_message );
            }
        }


        /**
         * Compares a_a with a_b. Both values have to be almost equal, within a_error.
         * If Compare fails the test will fail result will set to false. 
         */
        public void AlmostEqual( float a_a, float a_b, float a_error, string a_message )
        {
            bool equal = HMath.IsAlmostEqual( a_a, a_b, a_error );

            bool success = compare( equal, true, a_message );
            if( !success )
            {
                outputValues( a_a, a_b );
            }
        }


        /**
         * Compares a_a with a_b. Both values have to be equal.
         * If Compare fails the test will fail result will set to false. 
         * Both types have to be IEquatable.
         */
        public void Compare<T>( T a_a, T a_b, string a_message )
        {
            compare<T>( a_a, a_b, a_message );
        }


        /**
         * Compares a_a with a_b. Both values have to be NOT equal.
         * If Compare fails the test will fail result will set to false. 
         * Both tyes have to be IEquatable.
         */
        public void CompareNot<T>( T a_a, T a_b, string a_message )
        {
            compare<T>( a_a, a_b, a_message, false );
        }


        /**
         * Verifies the boolean. Fails if the boolean is false.
         * If Verify fails the test will fail result will set to false. 
         */
        public void Verify( bool a_value, string a_message )
        {
            compare<bool>( a_value, true, a_message, true );
        }


        /**
         * Verifies the boolean. Fails if the boolean is true.
         * If Verify fails the test will fail result will set to false. 
         */
        public void VerifyNot( bool a_value, string a_message )
        {
            compare<bool>( a_value, true, a_message, false );
        }


        /**
         * Verifies the Property on a_han. Fails if property is not on a_han or 
         * the Value is unequal to a_value.
         * If VerifyProperty fails the test will fail result will set to false. 
         */
        public void VerifyProperty<T>( IHANObject a_han, Key a_key, T a_value, string a_message )
        {
            if( a_han.HasProperty( a_key )
             && a_han.Property( a_key ) is Property<T> property ) {
                compare<T>( property.Value, a_value, a_message );
            }
            else { 
                failed( a_message );
            }
        }


        /**
         * Will expect incomming Log message
         */
        public void ExpectMessage( Key a_level, string a_message )
        {
            m_testLogFilter.Expect( a_level, a_message );
        }


        public void VerifyLog()
        {
            if( m_testLogFilter.Uncought.Count > 0 )
            {
                failed( "Uncought log messages found" );
                foreach( var message in m_testLogFilter.Uncought )
                {
                    Debug.Logger.Error( k_TestResult, "<color=red>UNCOUGHT:</color> {0}", message );
                }
            }

            if( m_testLogFilter.CountRemainingExpected() > 0 )
            {
                failed( "Not all expected messages have arrived: " );
                foreach( var level in m_testLogFilter.Expected )
                {
                    foreach( var message in level.Value )
                    {
                        Debug.Logger.Error( k_TestResult, "<color=red>EXPECTED:</color> {0}", message );
                    }
                }
            }
        }


        private bool compare( bool a_a, bool a_b, string a_message )
        {
            if( a_a != a_b )
            {
                failed( a_message );
                return false;
            }

            return true;
        }


        /// Compares two values if equal or not equal
        /// <param name="a_equal">Should it be equal or not equal</param>
        private void compare<T>( T a_a, T a_b, string a_message, bool a_equal = true )
        {
            bool succeded = compare( EqualityComparer<T>.Default.Equals( a_a, a_b ), a_equal, a_message );
            if( !succeded )
            {
                outputValues( a_a, a_b, a_equal );
            }
        }


        private void outputValues<T>( T a_a, T a_b, bool a_equal = true )
        {
            string a = a_a != null ? a_a.ToString() : "null";
            string b = a_b != null ? a_b.ToString() : "null";

            string outputMessage = a_equal 
                ? "\tValue is {0} but expected {1}" : "\tValue is {0} but NOT expected {1}";

            Debug.Logger.Error( k_TestResult
                              , outputMessage
                              , a, b );
        }

        /**
         * Test will fail with a_message
         */
        private void failed( string a_message )
        {
            Success = false;
            ErrorMesssages.Add( a_message );
            HAN.Debug.Logger.Error( k_TestResult, a_message );
        }


        /**
         * will calculate if the a_maxWaitTime is reached.
         * Only one external instance can use this API at once.
         * Every new use it has to be reseted by resetCurrentWaitTime().
         */
        private bool hasWaitTime( float a_maxWaitTime )
        {
            if( m_currentWaitTime < s_invalidWaitTime )
            {
                m_currentWaitTime = a_maxWaitTime;
            }
            else if( m_currentWaitTime >= 0f )
            {
                m_currentWaitTime -= Time.fixedDeltaTime;
            }
            else
            {
                return false;
            }

            return true;
        }


        /**
         * Will reset the current wait time.
         */
        private void resetCurrentWaitTime()
        {
            m_currentWaitTime = s_invalidWaitTime - 1f;
        }
    }

}