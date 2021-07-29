namespace HAN.Lib.Basic
{
    /**
     * Can receive IGenericSignalConnection's. To determine lifetime of a signal connection
     * we connect IGenericSignalHandler with each other. 
     * Each end point can manage and disconnect the connection.
     */
    public interface IGenericSignalHandler
    {
        void AddConnection( IGenericSignalConnection a_connection );
        void RemoveConnection( IGenericSignalConnection a_connection );
    }
}