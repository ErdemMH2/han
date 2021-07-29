using HAN.Lib.Basic;
using HAN.Lib.Stat;
using HAN.Lib.Structure;
using HAN.Lib.Test;


namespace HAN.Lib.Testing
{
    public class TestStatSystemTest : SystemTest
    {
        private static Key k_value = KeyFactory.Create( "Value" );
        private static Key k_value2 = KeyFactory.Create( "Value2" );


        private class MockHObject : HObject
        {
            public override MetaType Type()
            {
                return new MetaType( KeyFactory.Create( "MockHObject" ) );
            }
        }


        private class MockAdaptingStatSet : AbstractAdaptingStatSet
        {
            private StatInt m_adaptingValueStat1;
            private StatInt m_adaptingValueStat2;

            public MockAdaptingStatSet() : base( KeyFactory.Create("MockAdaptingStatSet") )
            {
                m_adaptingValueStat1 = new StatInt( k_value, 0, this );
                m_adaptingValueStat2 = new StatInt( k_value2, 0, this );

                AddStat( m_adaptingValueStat1 );
                AddStat( m_adaptingValueStat2 );
            }

            protected override void updateStats( BasicSignalParameter a_param )
            {
                if( a_param.Sender is HObject ) 
                {
                    HObject sender = (HObject) a_param.Sender;

                    Property<int> a = (Property<int>) sender.Property( m_adaptingValueStat1.Id );
                    m_adaptingValueStat1.SetValue( a.Value );

                    Property<int> b = (Property<int>) sender.Property( m_adaptingValueStat2.Id );
                    m_adaptingValueStat2.SetValue( b.Value );
                }
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
            addTest( testSimpleStatConnection, "Test StatSet > StatComponent > AdaptingStatSet" );
            addTest( testAdvancedStatConnection, "Test StatSet > AdaptingStatSet > StatComponent" );
        }


        public override void Prepare()
        {
        }


        public override void PrepareTest()
        {
        }


        private void testSimpleStatConnection( ref TestResult r_result )
        {
            StatSet statSet = new StatSet( KeyFactory.Create( "StatSet" ) );
            StatInt valueStat1 = new StatInt( k_value, 10, statSet );
            StatInt valueStat2 = new StatInt( k_value2, 20, statSet );
            statSet.AddStat( valueStat1 );
            statSet.AddStat( valueStat2 );

            StatComponent statObject = new StatComponent( KeyFactory.Create("") );
            statObject.AddStatSet( statSet );

            MockAdaptingStatSet adaptingStatSet = new MockAdaptingStatSet();
            adaptingStatSet.Connect( statObject );
            adaptingStatSet.Connect( statObject );

            valueStat1.SetValue( 1000 );
            valueStat2.SetValue( 2000 );

            int value = adaptingStatSet.Property<int>( k_value ).Value;
            r_result.Compare( value, 1000, "Stat valueStat1 was changed but no changed signal received" );

            int value2 = adaptingStatSet.Property<int>( k_value2 ).Value;
            r_result.Compare( value2, 2000, "Stat valueStat1 was changed but no changed signal received" );
        }


        private void testAdvancedStatConnection( ref TestResult r_result )
        {
            StatSet publishingStatSet = new StatSet( KeyFactory.Create( "StatSet" ) );
            StatInt publishingValueStat1 = new StatInt( k_value, 10, publishingStatSet );
            StatInt publishingValueStat2 = new StatInt( k_value2, 20, publishingStatSet );
            publishingStatSet.AddStat( publishingValueStat1 );
            publishingStatSet.AddStat( publishingValueStat2 );


            StatSet statSet = new StatSet( KeyFactory.Create( "StatSet" ) );
            StatInt valueStat1 = new StatInt( k_value, 100, statSet );
            StatInt valueStat2 = new StatInt( k_value2, 200, statSet );
            statSet.AddStat( valueStat1 );
            statSet.AddStat( valueStat2 );


            MockAdaptingStatSet adaptingStatSet = new MockAdaptingStatSet();
            adaptingStatSet.Connect( publishingStatSet );


            StatComponent statObject = new StatComponent( KeyFactory.Create( "" ) );
            statObject.AddStatSet( statSet );
            statObject.AddStatSet( adaptingStatSet );


            publishingValueStat1.SetValue( 1000 );
            publishingValueStat2.SetValue( 2000 );

            int value = statObject.Property<int>( k_value ).Value;
            r_result.Compare( value, 1100, "Stat valueStat1 was changed but no changed signal received" );

            int value2 = statObject.Property<int>( k_value2 ).Value;
            r_result.Compare( value2, 2200, "Stat valueStat1 was changed but no changed signal received" );
        }
    }
}