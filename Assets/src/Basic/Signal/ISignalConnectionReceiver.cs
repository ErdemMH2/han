using HAN.Lib.Structure;

namespace HAN.Lib.Basic
{
    /**
     * Receives a signal connection and connects to signal
     */
    public interface ISignalConnectionReceiver
    {
        bool SetSignal( ISignalHandler a_publisher, Key a_signalKey );
    }
}