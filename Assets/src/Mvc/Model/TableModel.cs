using HAN.Lib.Basic;
using HAN.Lib.Structure;
using System.Collections.Generic;

namespace HAN.Lib.Mvc.Model
{
    /**
     * A table version of the IHANModel. It will have rows and columns.
     * The AbstractTableModel will use TableId to fetch data. 
     * The TableId has a Row, Column and Role member. These will be used to find
     * the desired data. Roles can be used to add another dimension to a cell.
     * 
     * By iterating over the row, column and roles the whole data of the table can be retrieved.
     */
    public abstract class AbstractTableModel<T> : HObject,
                                                  IHANModel<TableId, T>
    {
        public static readonly Key k_AbstractTableModel = KeyFactory.Create( "AbstractTableModel" );
        public override MetaType Type() { return new MetaType( k_AbstractTableModel ); }

        public abstract IEnumerable<T> Values { get; }
        public abstract IEnumerable<ModelEntry<TableId, T>> Entrys { get; }

        protected Signal m_onChanged;


        public AbstractTableModel()
        {
            m_onChanged = new Signal( this );
            addSignal( Basic.Keys.Signal.ChangedSignal, m_onChanged );
        }


        public abstract ModelEntry<TableId, T> Position( TableId a_id );

        /**
         * Returns the row count of the table.
         */
        public abstract int RowCount();


        /**
         * Returns the column count of the table.
         */
        public abstract int ColumnCount();

        public abstract T Value( TableId a_key );
    }
}