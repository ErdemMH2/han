using HAN.Lib.Basic;
using HAN.Lib.Structure;
using System.Collections.Generic;

namespace HAN.Lib.Mvc.Model
{
    /**
     * List model that can hold items on one row. 
     * It is an AbstractTableModel with one row.
     */
    public class ListModel<T> : AbstractListModel<T>
                              , IHANWriteableModel<TableId, T>
                                where T : class
    {
        public static readonly Key k_LinksListModel = KeyFactory.Create( "EntryLinksListModel" );
        public override MetaType Type() { return new MetaType( k_LinksListModel ); }

        public override IEnumerable<T> Values { get { return m_values; } }
        public override IEnumerable<ModelEntry<TableId, T>> Entrys { get { return m_modelLinks.Values; } }

        protected List<T> m_values = new List<T>();
        protected Dictionary<TableId, WritableModelEntry<TableId, T>> m_modelLinks
                                            = new Dictionary<TableId, WritableModelEntry<TableId, T>>();
        private int m_insertId = 0;


        public ListModel()
        {
        }


        public override int ColumnCount()
        {
            return m_modelLinks.Count;
        }


        public override ModelEntry<TableId, T> Position( TableId a_id )
        {
            if( m_modelLinks.TryGetValue( a_id, out var entry ) ) {
                return entry;
            }

            HAN.Debug.Logger.Error( Type().Type,
                        string.Format( "cant find position for {0} wrong a_id or not found",
                                       a_id ) );
            return null;
        }


        public override T Value( TableId a_id )
        {
            if( m_modelLinks.TryGetValue( a_id, out var modelEntry ) )
            {
                return modelEntry.Value;
            }

            HAN.Debug.Logger.Error( Type().Type,
                        string.Format( "cant update {0} wrong a_id or not found",
                                       a_id ) );

            return null;
        }


        public bool Assign( TableId a_id, T a_entry )
        {
            T entry = a_entry;
            if( entry != null )
            {
                if( m_modelLinks.TryGetValue( a_id, out var oldEntry ) )
                {
                    bool updated = oldEntry.Assign( entry );
                    if( updated ) {
                        m_onChanged.Emit( new ModelSignalParameter<TableId, T>( oldEntry
                                                                              , Mvc.Keys.Model.Assign
                                                                              , this ) );
                    }

                    return updated;
                }
            }

            HAN.Debug.Logger.Error( Type().Type,
                                    string.Format( "cant update {0} wrong a_id or not found",
                                                   a_id ) );

            return false;
        }


        public bool Insert( TableId a_id, T a_entry )
        {
            T entry = a_entry;
            if( entry != null )
            {
                if( !m_modelLinks.TryGetValue( a_id, out var oldEnty ) )
                {
                    var inserted = new WritableModelEntry<TableId, T>( a_id, a_entry );
                    m_modelLinks.Add( a_id, inserted );
                    m_values.Add( entry );

                    m_onChanged.Emit( new ModelSignalParameter<TableId, T>( inserted
                                                                          , Mvc.Keys.Model.Insert
                                                                          , this ) );
                    return true;
                }
            }

            HAN.Debug.Logger.Warning( Type().Type,
                        string.Format( "cant insert {0} wrong a_id or not found",
                                       a_id ) );

            return false;
        }


        public TableId Append( T a_entry )
        {
            TableId tableId = new TableId( m_insertId++ );

            // incase we have used insert, so we will skip all used keys
            while( m_modelLinks.ContainsKey( tableId ) ) { 
                tableId = new TableId( m_insertId++ );
            }

            var appended = new WritableModelEntry<TableId, T>( tableId, a_entry );
            m_modelLinks.Add( tableId, appended );
            m_values.Add( a_entry );

            m_onChanged.Emit( new ModelSignalParameter<TableId, T>( appended
                                                                  , Mvc.Keys.Model.Append
                                                                  , this ) );

            return tableId;
        }


        public bool Remove( TableId a_id )
        {
            TableId tableId = a_id;
            if( tableId != null )
            {
                if( m_modelLinks.TryGetValue( a_id, out var oldEnty ) )
                {
                    m_values.Remove( oldEnty.Value );
                    m_modelLinks.Remove( a_id );

                    m_onChanged.Emit( new ModelSignalParameter<TableId, T>( oldEnty
                                                                          , Mvc.Keys.Model.Remove
                                                                          , this ) );

                    return true;
                }
            }

            HAN.Debug.Logger.Warning( Type().Type,
                        string.Format( "cant remove {0} wrong a_id or not found",
                        a_id.Id.Name ) );

            return false;
        }
    }
}
