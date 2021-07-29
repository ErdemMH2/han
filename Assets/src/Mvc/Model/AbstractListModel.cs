namespace HAN.Lib.Mvc.Model
{
    /**
     * A ListModel is a TableModel which supports only one column.
     */
    public abstract class AbstractListModel<T> : AbstractTableModel<T>
    {
        public override int RowCount() { return 1; }
    }
}