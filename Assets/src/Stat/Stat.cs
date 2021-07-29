using System;
using System.Collections.Generic;
using System.Linq;
using HAN.Lib.Basic;
using HAN.Lib.Structure;

namespace HAN.Lib.Stat
{
    /**
     * This Interface is used for generic access to all specialized Stats<T>
     */
    public interface IStat : IProperty, ISignalPublisher
    {
        IEnumerable<IStat> SubStats { get; }

        void Own( IHANObject a_owner );

        bool HasSubStats { get; }

        bool AddSubStat( IStat a_stat );

        bool RemoveSubStat( IStat a_stat );

        void RemoveAllSubStats();
    }


    /**
     * A Stat is a specialized Property. The difference is the possibility to merge Stats toghether.
     * Same properties with the same key can be merged and the values are summed up.
     * It can signalize changed events. 
     */
    public class Stat<T> : SignalizingProperty<T>, IStat
    {
        public static readonly Key k_Stat= KeyFactory.Create( "Stat" );
        public override MetaType Type() { return new MetaType( k_Stat ); }


        public IEnumerable<IStat> SubStats { get { return HasSubStats ? m_subStats.Keys : null; } }

        public bool HasSubStats { get { return m_subStats != null && m_subStats.Count > 0; } }

        protected Dictionary< IStat, Action<BasicSignalParameter> > m_subStats; //RAII


        public Stat( Key a_id, T a_val, IHANObject a_owner )
          : base( a_id, a_val, a_owner )
        {
        }


        /**
         * Tries to set the value, if same type
         * if value is changed a changed signal is emitted.
         */
        public override bool TrySetValue( object a_val )
        {
            bool isSet = false;

            // we want to compare without sub stats
            if( !object.Equals( base.Value, a_val ) )
            {
                isSet = rawTrySetValue( a_val );

                if( isSet )
                {
                    emitChangedSignal();
                }
            }

            return isSet;
        }


        public override void SetValue( T a_val )
        {
            // we want to compare without sub stats
            if( !System.Collections.Generic.EqualityComparer<T>.Default.Equals( base.Value, a_val ) )
            {
                rawSetValue( a_val );
                emitChangedSignal();
            }
        }


        public void Own( IHANObject a_owner )
        {
            m_owner = a_owner;
        }

        public T RawValue { get { return base.Value; } }

        /**
         * Adds sub stat to this stat, if it was not already added
         */
        public bool AddSubStat( IStat a_stat )
        {
            if( a_stat != null )
            {
                if( m_subStats == null )
                {
                    m_subStats = new Dictionary<IStat, Action<BasicSignalParameter>>();
                }

                if( m_subStats.ContainsKey( a_stat ) )
                {
                    HAN.Debug.Logger.Warning( Type().Type,
                                string.Format( "cant add to {0} sub stat {1} is allready existing",
                                Type().Type.Name, a_stat.Type().Type.Name ) );
                    return false;
                }

                Action<BasicSignalParameter> eventDelegate = ( BasicSignalParameter a_param ) => subStatChangedSlot( a_param );
                a_stat.Connect( m_owner, Id, eventDelegate );   // direct connection to SignilizingProperty

                m_subStats.Add( a_stat, eventDelegate );
                emitChangedSignal();
            }

            return true;
        }


        /**
         * Removes sub stat from stat
         */
        public bool RemoveSubStat( IStat a_stat )
        {
            if( m_subStats == null
             || !m_subStats.ContainsKey( a_stat ) )
            {
                HAN.Debug.Logger.Warning( Type().Type,
                                string.Format( "cant remove from {0} sub stat {1} was never added",
                                Type().Type.Name, a_stat.Type().Type.Name ) );
                return false;
            }

            a_stat.Disconnect( m_owner, Id, m_subStats[a_stat] );  // direct disconnection from SignilizingProperty
            m_subStats.Remove( a_stat );
            emitChangedSignal();

            return true;
        }


        /**
         * Removes all sub stat from stat
         */
        public void RemoveAllSubStats()
        {
            foreach( var statEntry in m_subStats )
            {
                var stat = statEntry.Key;
                stat.Disconnect( m_owner, Id, statEntry.Value );
            }

            m_subStats.Clear();
            emitChangedSignal();
        }


        /**
         * Emits is called if any sub stat of this set was changed
         */
        protected void subStatChangedSlot( BasicSignalParameter a_statChangedParam )
        {
            emitChangedSignal();
        }
    }


    /**
     * A Stat specialized for int, Value will summ all sub stats
     */
    public class StatInt : Stat<int>
    {
        public static readonly Key k_StatInt = KeyFactory.Create( "StatInt" );
        public override MetaType Type() { return new MetaType( k_StatInt ); }


        public StatInt( Key a_id, int a_val, IHANObject a_owner ) 
          : base( a_id, a_val, a_owner )
        {
        }


        /**
         * Returns value of this stat containing all sub stat values
         */
        public override int Value 
        { 
            get 
            {
                if( m_subStats != null ) 
                {
                    int value = base.Value;
                    foreach( var stat in m_subStats ) {
                        var statInt = stat.Key as StatInt;
                        value += statInt.Value;
                    }

                    return value;
                }

                return base.Value; // seperate return
            } 
        }
    }


    /**
     * A Stat specialized for int, Value will retun highest value
     */
    public class StatIntMax : StatInt
    {
        public StatIntMax( Key a_id, int a_val, IHANObject a_owner )
          : base( a_id, a_val, a_owner )
        {
        }


        /**
         * Returns the highest value of this stat containing all sub stat values
         */
        public override int Value
        {
            get
            {
                if( m_subStats != null )
                {
                    int value = 0;
                    foreach( var stat in m_subStats )
                    {
                        var statInt = stat.Key as StatInt;
                        if( value < statInt.Value )
                        {
                            value = statInt.Value;
                        }
                    }

                    return value;
                }

                return base.Value; // seperate return
            }
        }
    }


    /**
     * A Stat specialized for int, Value will return lowest stat 
     */
    public class StatIntMin : StatInt
    {
        public StatIntMin( Key a_id, int a_val, IHANObject a_owner )
          : base( a_id, a_val, a_owner )
        {
        }


        /**
         * Returns the lowest value of this stat containing all sub stat values
         */
        public override int Value
        {
            get
            {
                if( m_subStats != null )
                {
                    int value = int.MaxValue;
                    foreach( var stat in m_subStats )
                    {
                        var statInt = stat.Key as StatInt;
                        if( statInt.Value < value
                         && statInt.Value > 0 )
                        {
                            value = statInt.Value;
                        }
                    }

                    return value;
                }

                return base.Value; // seperate return
            }
        }
    }

    /**
     * A Stat specialized for string, Value concetenate all sub stats
     */
    public class StatString : Stat<string>
    {
        public static readonly Key k_StatString= KeyFactory.Create( "StatString" );
        public override MetaType Type() { return new MetaType( k_StatString ); }

        public string ConcatenationSpacer { get; set; }


        public StatString( Key a_id, string a_val, IHANObject a_owner )
          : base( a_id, a_val, a_owner )
        {
            ConcatenationSpacer = "";
        }


        /**
         * Returns value of this stat containing all sub stat values
         */
        public override string Value
        {
            get
            {
                if( m_subStats != null )
                {
                    string value = base.Value;
                    foreach( var stat in m_subStats )
                    {
                        var statInt = stat.Key as StatString;
                        if( value.Length > 0 ) { 
                            value += ConcatenationSpacer;
                        }
                        value += statInt.Value;
                    }

                    return value;
                }

                return base.Value; // seperate return
            }
        }
    }


    /**
    * A Stat specialized for string, Value concetenate all sub stats
    */
    public class StatKey : Stat<Key>
    {
        public static readonly Key k_StatKey = KeyFactory.Create( "StatKey" );
        public override MetaType Type() { return new MetaType( k_StatKey ); }


        public StatKey( Key a_id, Key a_val, IHANObject a_owner )
          : base( a_id, a_val, a_owner )
        {
        }


        /**
         * Returns value of this stat containing all sub stat values
         */
        public override Key Value
        {
            get
            {
                if( m_subStats != null )
                {
                    Key value = base.Value;
                    foreach( var stat in m_subStats )
                    {
                        var statKey = stat.Key as StatKey;
                        value += statKey.Value;
                    }

                    return value;
                }

                return base.Value; // seperate return
            }
        }
    }
}