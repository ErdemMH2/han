using HAN.Lib.Basic;
using HAN.Lib.Structure;
using System.Collections.Generic;


namespace HAN.Lib.Stat
{
    /**
     * A StatComponent is a HObject that is specialized to hold and organize StatSets.
     * Stats can be added by adding StatSets, which are encapsulating Stat groups. 
     * By adding a StatSet all Stats will be transfered as Properties to the StatComponent as Substats,
     * even when it had no corresponding Stat before. A new main stat will be created and exclusivly owned
     * by this StatComponent. This guarentees that we are not touching and changing the added StatSet. 
     * All incoming connections will be redirected to the new main stat.
     * 
     * The generic PropertyAccess can be used to read, modify or register to signals of Stats.
     * Stats will behave similar to Properties, with the difference, that they could hold sub stats, 
     * which are changing the returned value. 
     * StatSets with duplicated Stats (same Key) will result in adding the Stat as a sub Stat of the already existing Stat.
     */
    public class StatComponent : StatSet
    {
        public static readonly Key k_StatObject = KeyFactory.Create( "StatComponent" );
        public override MetaType Type() { if( m_type == null ) m_type = new MetaType( k_StatObject ); return m_type; }

        public IList<StatSet> Sets { get { return m_sets.AsReadOnly(); } }

        protected List< StatSet > m_sets = new List< StatSet >();


        public StatComponent( Key a_id )
          : base( a_id )
        {
        }


        /**
         * Will add a StatSet. Every duplicate Key will be added as a sub stat.
         * If a property with the stat key exists its value will be used instead of stat concatenating
         */
        public bool AddStatSet( StatSet a_set )
        {
            if( !m_sets.Contains( a_set ) )
            {
                m_sets.Add( a_set );
                a_set.Connect( this, Basic.Keys.Signal.CountChanged, onStatCountChangedSlot );

                foreach( var stat in a_set.Stats )  // add all stats as properties
                {
                    addStatAndSubstat( stat );
                }

                return true;
            }

            return false;
        }


        /**
         * Will remove a StatSet. If a Stat has sub stats, the first sub stat will be 
         * the new main Stat. All signal connection will be transfered to the new main stat.
         */
        public bool RemoveStatSet( StatSet a_set )
        {
            if( m_sets.Contains( a_set ) )
            {
                a_set.Disconnect( this, Basic.Keys.Signal.CountChanged, onStatCountChangedSlot );
                m_sets.Remove( a_set );
                bool removed = true;

                foreach( var stat in a_set.Stats )  // remove all stats from properties
                {
                    removed &= removeStatAndSubstat( stat );
                }

                return removed;
            }

            return false;
        }


        private void addStatAndSubstat( IStat a_stat )
        {
            if( !HasProperty( a_stat.Id ) )  // first appearance of stat
            {
                var newStat = PropertyConverter.CreateNewEmpty( a_stat );
                newStat.Own( this );
                newStat.AddSubStat( a_stat );
                AddStat( newStat );
            }
            else
            {
                IProperty property = Property( a_stat.Id );
                if( property is IStat mainStat )
                { // add stat as substat
                    mainStat.AddSubStat( a_stat );
                }
                else
                { // if it is not stat set the property value
                    property.TrySetValue( a_stat.ValueObject );
                }
            }
        }


        private bool removeStatAndSubstat( IStat a_stat )
        {
            bool removed = true;

            var mainStat = Property( a_stat.Id ) as IStat;    // remove stat or substat
            if( mainStat != null )
            {
                // substat
                removed = mainStat.RemoveSubStat( a_stat );  // remove substat from main stat

                if( !mainStat.HasSubStats ) // main stat is empty
                {
                    removed &= RemoveStat( mainStat ); // remove stat
                }
            }

            return removed;
        }


        private void onStatCountChangedSlot( BasicSignalParameter a_param )
        {
            var param = (CountChangedSignalParameter<IStat>) a_param;
            StatSet changedStatSet = a_param.Sender as StatSet;
            if( m_sets.Contains( changedStatSet ) )
            {
                foreach( IStat stat in param.Changed )
                {
                    if( param.Added )
                    {
                        addStatAndSubstat( stat );
                    }
                    else if( param.Removed )
                    {
                        removeStatAndSubstat( stat );
                    }
                }
            }
        }
    }
}
