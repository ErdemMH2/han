using HAN.Lib.Structure;

namespace HAN.Lib.Basic 
{
    /**
     * Subscribes signals by registering to a publisher
     * publishers are automaticaly informed if this objects canceles signal receiving
     */
    public interface ISignalSubscriber : IHANType, ISignalDisconnectable, IGenericSignalHandler
    {
    }
}
