using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HAN.Lib.Test
{
    public class TestSuiteResult
    {
        public uint Succeeded { get; private set; } = 0;
        public uint Failed { get; private set; } = 0;

        private List<TestResult> m_testResults = new List<TestResult>();


        public void AddResult( TestResult a_result )
        {
            m_testResults.Add( a_result );

            if( a_result.Success ) {
                Succeeded++;
            }
            else {
                Failed++;
            }
        }
    }
}