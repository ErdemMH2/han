using HAN.Lib.Basic;
using HAN.Lib.Stat;
using HAN.Lib.Structure;
using HAN.Lib.Test;
using System.Linq;

namespace HAN.Lib.Testing
{
    public class TestStatObjectUnitTest : UnitTest
    {
        private class MockHObject : HObject
        {
            public override MetaType Type()
            {
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
            addTest( testAddAndRemoveStatSets, "Test adding and removing stats" );
            addTest( testAddAndRemoveStatSetsWithPropertyRemove, "Test adding and removing stats with removing of property" );
            addTest( testChangedSignal, "Test changed signal after adding and removing StatSet" );
        }


        public override void Prepare()
        {
        }


        public override void PrepareTest()
        {
        }


        private void testAddAndRemoveStatSets( ref TestResult r_result )
        {
            StatInt valueStat1 = new StatInt( KeyFactory.Create( "Value" ), 0, null );
            StatInt valueStat2 = new StatInt( KeyFactory.Create( "Value2" ), 0, null );
            StatInt valueStat3 = new StatInt( KeyFactory.Create( "Value3" ), 0, null );

            StatSet statSet = new StatSet( KeyFactory.Create( "StatSet" ) );
            statSet.AddStat( valueStat1 );
            statSet.AddStat( valueStat2 );
            statSet.AddStat( valueStat3 );

            StatComponent statObject = new StatComponent( KeyFactory.Create( "" ) );
            bool added = statObject.AddStatSet( statSet );
            bool secondAdd = statObject.AddStatSet( statSet );
            r_result.Verify( added, "StatSet was not added" );
            r_result.Verify( !secondAdd, "StatSet should not be added twice" );

            r_result.Verify( statObject.HasProperty( valueStat1.Id ), "Stat valueStat1 was not added" );
            r_result.Verify( statObject.HasProperty( valueStat2.Id ), "Stat valueStat2 was not added" );
            r_result.Verify( statObject.HasProperty( valueStat3.Id ), "Stat valueStat2 was not added" );


            bool removed = statObject.RemoveStatSet( statSet );
            r_result.Verify( removed, "StatSet was not removed" );

            r_result.Verify( !statObject.HasProperty( valueStat1.Id ), "Stat valueStat1 was not removed" );
            r_result.Verify( !statObject.HasProperty( valueStat2.Id ), "Stat valueStat2 was not removed" );
            r_result.Verify( !statObject.HasProperty( valueStat3.Id ), "Stat valueStat2 was not removed" );
        }


        private void testAddAndRemoveStatSetsWithPropertyRemove( ref TestResult r_result )
        {
            StatInt valueStat1 = new StatInt( KeyFactory.Create( "Value" ), 0, null );
            StatInt valueStat2 = new StatInt( KeyFactory.Create( "Value2" ), 0, null );
            StatInt valueStat3 = new StatInt( KeyFactory.Create( "Value3" ), 0, null );

            StatSet statSet = new StatSet( KeyFactory.Create( "StatSet" ) );
            statSet.AddStat( valueStat1 );
            statSet.AddStat( valueStat2 );
            statSet.AddStat( valueStat3 );

            StatComponent statObject = new StatComponent( KeyFactory.Create( "" ) );
            bool added = statObject.AddStatSet( statSet );
            r_result.Verify( added, "StatSet was not added" );

            statObject.RemoveProperty( valueStat1.Id );
            r_result.Verify( !statObject.HasProperty( valueStat1.Id ), "Stat valueStat1 was not removed" );

            r_result.ExpectMessage( Debug.Logger.k_Error, "Cant find Property Value of StatComponent" );
            bool removed = statObject.RemoveStatSet( statSet );
            r_result.Verify( removed, "StatSet was not removed" );
            r_result.Compare( statObject.Sets.FirstOrDefault( s => s == statSet ), null, "StatSet was not removed" );
        }


        private void testChangedSignal( ref TestResult r_result )
        {
            Key valueStat1Key = KeyFactory.Create( "Value" );
            StatInt valueStat1 = new StatInt( valueStat1Key, 0, null );
            StatInt valueStat2 = new StatInt( KeyFactory.Create( "Value2" ), 0, null );
            StatInt valueStat3 = new StatInt( KeyFactory.Create( "Value3" ), 0, null );

            StatSet statSet = new StatSet( KeyFactory.Create( "StatSet" ) );
            statSet.AddStat( valueStat1 );
            statSet.AddStat( valueStat2 );
            statSet.AddStat( valueStat3 );

            StatComponent statObject = new StatComponent( KeyFactory.Create( "" ) );
            statObject.AddStatSet( statSet );

            HObject subscriber = new MockHObject();
            int changedReceived = 0;
            System.Action<BasicSignalParameter> eventDelegate = ( BasicSignalParameter a_param ) => { changedReceived++; };
            statObject.Connect( subscriber, valueStat1Key, eventDelegate ); // connect to stat, will use internal proxy

            valueStat1.SetValue( 10 );
            r_result.Compare( changedReceived, 1, "Changed (SetValue) signal of SetValue of valueStat1 not received" );

            // test after removing statset
            statObject.RemoveStatSet( statSet );
            r_result.Compare( changedReceived, 2, "Changed (RemoveSubStat) signal of SetValue of valueStat1 not received" );

            valueStat1.SetValue( 100 );
            r_result.Compare( changedReceived, 2, "Changed signal of SetValue of valueStat1 should not be received" );

            // add again -> autoconnect to proxy
            statObject.AddStatSet( statSet );

            valueStat1.SetValue( 100 );
            r_result.Compare( changedReceived, 2, "Changed signal of SetValue of valueStat1 should not be received" );
        }
    }
}