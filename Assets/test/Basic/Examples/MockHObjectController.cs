using HAN.Lib.Basic;
using HAN.Lib.Structure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HAN.Lib.Testing
{
    public class MockHObjectController : HObjectMonoBehavior
    {
        private class HObjectMockHObjectController : HObject
        {
            private static Key k_health = KeyFactory.Create( "Health" );
            private static Key k_name = KeyFactory.Create( "Name" );


            private List< IHANObject > m_members;


            public HObjectMockHObjectController()
            {
            }


            public void RefreshMembers( List<IHANObject> a_newMembers )
            {
                m_members = a_newMembers;
                registerFighter();
            }


            public void FightOneOnOne()
            {
                if( m_members.Count > 1 )
                {
                    IHANObject player1 = m_members[0];
                    IHANObject player2 = m_members[1];

                    int player1Health = player1.Property<int>( k_health ).Value;
                    int player2Health = player2.Property<int>( k_health ).Value;

                    int round = 0;
                    while( (player1Health > 0) && (player2Health > 0) )
                    {
                        HAN.Debug.Logger.Log( Type().Type, "Round " + (++round) );

                        int player1Damage = Random.Range( 0, 3);
                        int player2Damage = Random.Range( 0, 5 );

                        // apply damage to both
                        player2Health = player2Health - player1Damage;
                        player1Health = player1Health - player2Damage;

                        player1.SetProperty<int>( k_health, player1Health );
                        player2.SetProperty<int>( k_health, player2Health );

                        HAN.Debug.Logger.Log( Type().Type, "Round NEXT " + round );
                    }

                    HAN.Debug.Logger.Log( Type().Type, "Round END" );
                }
            }


            public override MetaType Type()
            {
                return new MetaType( KeyFactory.Create( "HObjectMockHObjectController" ) );
            }


            private void registerFighter()
            {
                if( m_members.Count > 1 )
                {
                    IHANObject player1 = m_members[0];
                    IHANObject player2 = m_members[1];

                    player1.Connect( this, k_health, onPlayerDamage );
                    player2.Connect( this, k_health, onPlayerDamage );
                }
            }


            private void onPlayerDamage( BasicSignalParameter a_param )
            {
                PropertyChangedSignalParam<int> health = (PropertyChangedSignalParam<int>) a_param;

                if( health.Value <= 0 )
                {
                    string name = "";
                    if( health.Sender == m_members[0] ) // player 1
                    {
                        name = m_members[0].Property<string>( k_name ).Value;
                    }
                    else // player 2
                    {
                        name = m_members[1].Property<string>( k_name ).Value;
                    }

                    // we died
                    HAN.Debug.Logger.Log( Type().Type, "Player " + name + " died" );
                }
            }
        }

        private HObjectMockHObjectController m_controller;

        public override IHANObject HObject { get {   return m_controller;    } }


        public MockHObjectController()
        {
            m_controller = new HObjectMockHObjectController();
        }


        public void RefreshMembers()
        {
            List<IHANObject> children = new List<IHANObject>();
            var components = GetComponentsInChildren<HObjectMonoBehavior>();
            for( int i = 0; i < components.Length; i++ )
            {
                if( components[i] != this )
                    children.Add( components[i] );
            }

            m_controller.RefreshMembers( children );
        }


        public void FightOneOnOne()
        {
            m_controller.FightOneOnOne();
        }
    }
}