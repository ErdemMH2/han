
namespace HAN.Lib.Basic
{
    /**
     * IGenericSignal for type safety and common Signal functionality
     */
    public interface IGenericSignal : ISignalDisconnectable
    {
        void Disconnect( IGenericSignalConnection a_connection );
    }
} 