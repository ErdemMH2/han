using HAN.Lib.Basic;
using HAN.Lib.Structure;
using System.Collections;
using UnityEngine;


namespace HAN.Lib.Testing
{
    /**
     * Example implementation of HObjectMonoBehavior for manual tests.
     */
    public class MockHObjectMonoBehavior : HObjectMonoBehavior
    {
        /**
         * Private HObject implementation for using only in this HObjectMonoBehavior.
         * It is also possible to implement a HObject for general purpose and add it here as a behavior.
         */
        private class MockHObject : HObject
        {
            SignalizingProperty<int> m_health;
            SignalizingProperty<int> m_armor;

            SignalizingProperty<Key> m_weapon;

            Property<string> m_name;


            public MockHObject()
            {
                m_health = new SignalizingProperty<int>( KeyFactory.Create( "Health" ), 10, this );
                AddProperty( m_health );

                m_armor = new SignalizingProperty<int>( KeyFactory.Create( "Armor" ), 1, this );
                AddProperty( m_armor );


                m_weapon = new SignalizingProperty<Key>( KeyFactory.Create( "Weapon" ), KeyFactory.Create( "Stone" ), this );
                AddProperty( m_weapon );


                m_name = new Property<string>( KeyFactory.Create( "Name" ), "Dead Meat" );
                AddProperty( m_name );
            }


            public override MetaType Type()
            {
                return new MetaType( KeyFactory.Create( "MockHObject" ) );
            }
        }

        public override IHANObject HObject { get { return m_object; } }

        private IHANObject m_object;

        public MockHObjectMonoBehavior()
        {
            m_object = new MockHObject();
        }
    }
}