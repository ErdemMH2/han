namespace HAN.Lib.Basic
{
    /**
     * Connection object, that will define a connection between a
     * IGenericSignalHandler Sender and Receiver through the IGenericSignal Signal.
     */
    public interface IGenericSignalConnection
    {
        IGenericSignal Signal { get; }
        IGenericSignalHandler Sender { get; }
        IGenericSignalHandler Receiver { get; }

        void Disconnect();
    }
}