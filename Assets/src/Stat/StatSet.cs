using System.Linq;
using System.Collections.Generic;
using HAN.Lib.Basic;
using HAN.Lib.Structure;
using HAN.Lib.Mvc;

namespace HAN.Lib.Stat
{
    /**
     * A StatSet contains a variation of stats with different key. 
     * When a Stat is added it will be handled like a Property and this StatSet will own the Stat.
     * When the Stat is removed, all connections of the Stat will be dropped, as we were owning it!
     * 
     * Multiple stats with the same Key are not allowed.
     * It will signalize changed events for all owned stats.
     */
    public class StatSet : HComponent
    {
        public static readonly Key k_StatSet = KeyFactory.Create( "StatSet" );
        public override MetaType Type() { return new MetaType( k_StatSet ); }

        public ICollection<IStat> Stats { get { return m_stats.AsReadOnly(); } }

        private List< IStat > m_stats = new List< IStat >();

        private Signal m_onStatCountChanged;

        public StatSet( Key a_id )
          : base( a_id )
        {
            m_onStatCountChanged = new Signal( this );
            addSignal( Basic.Keys.Signal.CountChanged, m_onStatCountChanged );
        }


        /**
         * Will add stat to set, if key is not allready present
         */
        public bool AddStat( IStat a_stat )
        {
            a_stat.Own( this ); // we need to own it to be able to connect to it in AddProperty
            if( AddProperty( a_stat ) ) // this make stat available as signals
            {
                m_stats.Add( a_stat );
                m_onStatCountChanged.Emit( 
                        new CountChangedSignalParameter<IStat>( new IStat[] { a_stat }, true, this ) );

                return true;
            }

            a_stat.Own( null ); // revert owning it
            return false;
        }


        /**
         * Will remove stat from set
         */
        public bool RemoveStat( Key a_id )
        {
            var stat = Stats.FirstOrDefault( st => st.Id == a_id );
            if( stat != null )
            {
                return RemoveStat( stat );
            }

            return false;
        }

        /**
         * Will remove stat from set
         */
        public bool RemoveStat( IStat a_stat )
        {
            if( RemoveProperty( a_stat.Id ) )
            {
                m_stats.Remove( a_stat );
                a_stat.Own( null );
                m_onStatCountChanged.Emit( new CountChangedSignalParameter<IStat>( new IStat[] { a_stat }, false, this ) );

                return true;
            }

            return false;
        }
    }
}