using HAN.Lib.Basic;
using HAN.Lib.Stat;
using HAN.Lib.Structure;
using HAN.Lib.Test;


namespace HAN.Lib.Testing
{
    public class TestStatUnitTest : UnitTest
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
            addTest( testStatValue, "Test setting and retrieving values" );
            addTest( testAddAndRemoveSubStats, "Test adding and removing stats" );
            addTest( testChangedSignal, "Test changed signal after SetValue and Add- and RemoveSubStat" );
        }


        public override void Prepare()
        {
        }


        public override void PrepareTest()
        {
        }


        private void testStatValue( ref TestResult r_result )
        {
            MockHObject sender = new MockHObject();
            int value = 100;
            Key valueKey = KeyFactory.Create( "Value" );
            StatInt valueStat = new StatInt( valueKey, value, sender );
            r_result.Compare( value, valueStat.Value, "Values are not equal" );
        }


        private void testAddAndRemoveSubStats( ref TestResult r_result )
        {
            MockHObject sender = new MockHObject();
            int value = 100, valueSub1 = 50, valueSub2 = 25;
            Key valueKey = KeyFactory.Create( "Value" );
            StatInt valueStat = new StatInt( valueKey, value, sender );
            StatInt valueSubStat1 = new StatInt( valueKey, valueSub1, sender );
            StatInt valueSubStat2 = new StatInt( valueKey, valueSub2, sender );

            valueStat.AddSubStat( valueSubStat1 );
            r_result.Compare( value + valueSub1, valueStat.Value, "Values of summed (2) stats are not equal" );

            valueStat.AddSubStat( valueSubStat2 );
            r_result.Compare( value + valueSub1 + valueSub2, valueStat.Value, "Values of summed (3) stats are not equal" );

            r_result.Verify( valueStat.HasSubStats, "Has no substats" );

            valueStat.RemoveSubStat( valueSubStat2 );
            r_result.Compare( value + valueSub1, valueStat.Value, "Values of summed (2)[after remove] stats are not equal" );

            valueStat.RemoveAllSubStats();
            r_result.Compare( value, valueStat.Value, "Value of stats is not correct" );
        }


        private void testChangedSignal( ref TestResult r_result )
        {
            MockHObject sender = new MockHObject();
            MockHObject receiver = new MockHObject();

            int value = 100;
            Key valueKey = KeyFactory.Create( "Value" );
            StatInt valueStat = new StatInt( valueKey, value, sender );
            StatInt valueSubStat = new StatInt( valueKey, value, sender );

            int changedReceived = 0;
            System.Action<BasicSignalParameter> eventDelegate = ( BasicSignalParameter a_param ) => { changedReceived++; };
            valueStat.Connect( receiver, valueKey, eventDelegate );

            valueStat.SetValue( value * 2 );
            r_result.Compare( changedReceived, 1, "Changed signal of SetValue not received" );

            valueStat.AddSubStat( valueSubStat );
            r_result.Compare( changedReceived, 2, "Changed signal of AddSubStat not received" );

            valueSubStat.SetValue( value * 2 );
            r_result.Compare( changedReceived, 3, "Changed signal of SetValue of AddSubStat not received" );

            valueStat.Disconnect( receiver, valueKey, eventDelegate );
            valueStat.SetValue( value * 3 );
            valueSubStat.SetValue( value * 3 );
            r_result.Compare( changedReceived, 3, "Changed signal of SetValue of AddSubStat not received" );
        }
    }
}