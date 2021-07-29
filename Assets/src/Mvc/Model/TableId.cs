using HAN.Lib.Basic;
using HAN.Lib.Structure;

namespace HAN.Lib.Mvc.Model
{
    /**
     * This Id will provide two extra dimensions for row and column.
     * The Id Key will be used as a role. A role can return different data for the same 
     * row and column.
     * A TableId is only comparable to other TableIds (and derived).
     */
    public class TableId : IncrementalId
    {
        public int Column { get; private set; }
        public int Row { get; private set; }


        /**
         * Creates TableId.
         * <param name="a_role">One of the supported Roles of the used AbstractTableModel.</param>
         */
        public TableId( int a_column )
          : this( a_column, 0 )
        {
        }

        /**
         * Creates TableId.
         * <param name="a_role">One of the supported Roles of the used AbstractTableModel.</param>
         */
        public TableId( int a_column, int a_row )
          : base( KeyFactory.Create( "TableId_" + a_column.ToString() + "." + a_row.ToString() ) )
        {
            Column = a_column;
            Row = a_row;
        }


        public override bool Equals( object a_obj )
        {
            TableId other = a_obj as TableId;
            if( other != null )
            {
                return other.Column == Column
                    && other.Row == Row
                    && base.Equals( a_obj );
            }

            return false;
        }


        public override int GetHashCode()
        {
            return base.GetHashCode() << Row << Column;
        }


        public override string ToString()
        {
            return "Row: " + Row + " Column: " + Column + " Role: " + base.ToString(); 
        }
    }
}