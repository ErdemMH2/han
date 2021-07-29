using HAN.Debug;
using HAN.Lib.Structure;
using System.Collections.Generic;

namespace HAN.Lib.Test
{
    public class TestLogFilter : ILogFilter
    {
        public IReadOnlyList<string> Uncought { get { return m_uncought; } }
        private List<string> m_uncought = new List<string>();

        public IReadOnlyDictionary<Key, List<string> > Expected { get { return m_expected; } }
        private Dictionary<Key, List<string>> m_expected = new Dictionary<Key, List<string>>();


        public bool Filter( Key a_level, Key a_tag, string a_message )
        {
            if( a_tag == TestResult.k_TestResult ) {
                return false;
            }

            if( m_expected.ContainsKey( a_level )
             && m_expected[a_level].Contains( a_message ) )
            {
                m_expected[a_level].Remove( a_message );
                Debug.Logger.Log( a_tag, "<color=green>Expected message cought:</color> [{0}] : {1}"
                                       , a_level, a_message );
                return true;
            }

            if( a_level != Logger.k_Log ) { 
                m_uncought.Add( a_message );
            }

            return false;
        }


        public uint CountRemainingExpected()
        {
            uint remaining = 0;

            foreach( var level in m_expected.Keys )
            {
                foreach( var message in m_expected[level] )
                {
                    remaining++;
                }
            }

            return remaining;
        }

        public void Expect( Key a_level, string a_message )
        {
            if( !m_expected.ContainsKey( a_level ) )
            {
                m_expected.Add( a_level, new List<string>() );
            }

            m_expected[a_level].Add( a_message );
        }
    }
}