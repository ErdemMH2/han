using HAN.Lib.Basic;
using HAN.Lib.Stat;
using HAN.Lib.Structure;
using HAN.Lib.Test;


namespace HAN.Lib.Testing
{
    public class TestStatSetUnitTest : UnitTest
    {
        private class MockHObject : HObject
        {
            public override MetaType Type() {
                return new MetaType( KeyFactory.Create( "MockHObject" ) );
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
            addTest( testAddAndRemoveStats, "Test adding and removing stats" );
            addTest( testChangedSignal, "Test changed signal after SetValue and Add- and RemoveSubStat" );
        }


        public override void Prepare()
        {
        }


        public override void PrepareTest()
        {
        }


        private void testAddAndRemoveStats( ref TestResult r_result )
        {
            StatInt valueStat1 = new StatInt( KeyFactory.Create( "Value" ), 0, null );
            StatInt valueStat2 = new StatInt( KeyFactory.Create( "Value2" ), 0, null );
            StatInt valueStat3 = new StatInt( KeyFactory.Create( "Value3" ), 0, null );
            StatInt duplicateKeyStat = new StatInt( KeyFactory.Create( "Value" ), 0, null );

            StatSet statSet = new StatSet( KeyFactory.Create( "StatSet" ) );
            statSet.AddStat( valueStat1 );
            statSet.AddStat( valueStat2 );
            statSet.AddStat( valueStat3 );

            r_result.ExpectMessage( Debug.Logger.k_Warning, "cant add property Value to StatSet; Property already present" );
            r_result.ExpectMessage( Debug.Logger.k_Warning, "cant add property Value to StatSet; Property already present" );
            bool duplicateAdded = statSet.AddStat( duplicateKeyStat ); // should not be added
            bool secondAdd = statSet.AddStat( valueStat1 ); // should not be added

            r_result.Verify( !duplicateAdded, "Duplicate should not be added" );
            r_result.Verify( !secondAdd, "Second add should not be successful" );

            r_result.Verify( statSet.Stats.Contains( valueStat1 ), "Stat not added" );
            r_result.Compare( statSet.Stats.Count, 3, "Stat count is not correct" );

            statSet.RemoveStat( valueStat1 );
            r_result.Verify( !statSet.Stats.Contains( valueStat1 ), "Stat not removed" );

            r_result.Compare( statSet.Stats.Count, 2, "Stat count is not correct" );
            r_result.Verify( statSet.Stats.Contains( valueStat2 ), "Stat not available anymore" );
        }


        private void testChangedSignal( ref TestResult r_result )
        {
            StatInt valueStat1 = new StatInt( KeyFactory.Create( "Value" ), 0, null );
            StatInt valueStat2 = new StatInt( KeyFactory.Create( "Value2" ), 0, null );
            StatInt duplicateValueStat = new StatInt( KeyFactory.Create( "Value2" ), 0, null );

            StatSet statSet = new StatSet( KeyFactory.Create( "StatSet" ) );

            HObject subscriber = new MockHObject();
            int changedReceived = 0;
            System.Action<BasicSignalParameter> eventDelegate = ( BasicSignalParameter a_param ) => { changedReceived++; };
            statSet.Connect( subscriber, Basic.Keys.Signal.CountChanged, eventDelegate );
            statSet.Connect( subscriber, Basic.Keys.Signal.ChangedSignal, eventDelegate );

            statSet.AddStat( valueStat1 );
            r_result.Compare( changedReceived, 1, "1. Changed signal of AddStat of valueStat1 not received" );

            statSet.AddStat( valueStat2 );
            r_result.Compare( changedReceived, 2, "2. Changed signal of AddStat of valueStat2 not received" );

            r_result.ExpectMessage( Debug.Logger.k_Warning, "cant add property Value2 to StatSet; Property already present" );
            statSet.AddStat( duplicateValueStat );
            r_result.Compare( changedReceived, 2, "3. Changed signal of AddStat of duplicateValueStat should NOT be received" );

            valueStat1.SetValue( 1 );
            r_result.Compare( changedReceived, 3, "4. Changed signal of SetValue not received" );

            valueStat1.SetValue( 2 );
            r_result.Compare( changedReceived, 4, "5. Changed signal of SetValue not received" );

            valueStat2.SetValue( 1 );
            r_result.Compare( changedReceived, 5, "6. Changed signal of SetValue not received" );

            statSet.RemoveStat( valueStat1 );
            r_result.Compare( changedReceived, 6, "7. Changed signal of RemoveStat not received" );

            // test disconnect after remove
            valueStat1.SetValue( 4 );
            r_result.Compare( changedReceived, 6, "Changed signal of RemoveStat should NOT be received" );

            valueStat2.SetValue( 3 );
            r_result.Compare( changedReceived, 7, "8. Changed signal of SetValue not received" );
        }
    }
}