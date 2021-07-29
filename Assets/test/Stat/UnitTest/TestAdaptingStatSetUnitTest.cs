using HAN.Lib.Basic;
using HAN.Lib.Stat;
using HAN.Lib.Structure;
using HAN.Lib.Test;
using System;

namespace HAN.Lib.Testing
{
    public class TestAdaptingStatSetUnitTest : UnitTest
    {
        private class MockAdaptingStatSet : AbstractAdaptingStatSet
        {
            private Action<BasicSignalParameter> m_updateCall;

            public MockAdaptingStatSet( Action<BasicSignalParameter> a_updateCall ) 
              : base( KeyFactory.Create( "StatSet" ) )
            {
                m_updateCall = a_updateCall;
            }

            protected override void updateStats( BasicSignalParameter a_param )
            {
                m_updateCall( a_param );
            }
        }


        public override void Cleanup()
        {
        }


        public override void CleanupTest()
        {
        }


        public override void Init()
        {
            addTest( testChangeSignalStatSet, "Test is updateSignal received of StatSet" );
        }


        public override void Prepare()
        {
        }


        public override void PrepareTest()
        {
        }


        private void testChangeSignalStatSet( ref TestResult r_result )
        {
            StatInt valueStat1 = new StatInt( KeyFactory.Create( "Value" ), 0, null );
            StatSet statSet = new StatSet( KeyFactory.Create( "StatSet" ) );
            statSet.AddStat( valueStat1 );

            int changedReceived = 0;
            Action<BasicSignalParameter> updateDelegate = ( BasicSignalParameter a_param ) => { changedReceived++; };
            MockAdaptingStatSet adaptingStatSet = new MockAdaptingStatSet( updateDelegate );
            adaptingStatSet.Connect( statSet );

            valueStat1.SetValue( 100 );
            r_result.Compare( changedReceived, 1, "Stat valueStat1 was changed but no changed signal received" );
        }
    }
}