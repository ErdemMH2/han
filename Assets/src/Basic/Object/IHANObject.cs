using HAN.Lib.Structure;

namespace HAN.Lib.Basic {

    /**
     * Object replacement, which has signals and slots, properties and HAN runtime type information
     */
    public interface IHANObject : IHANType, ISignalHandler, ISignalSubscriber, IPropertyAccess
    {
        /* This is a abstract composition of different interfaces */


        /**
         *  Returns true if the signal is supported
         */
        bool HasSignal( Key a_signalKey );
    }


    /**
     * Proxy for HObject should forward all functionality to proxied HObject
     */
    public interface IHANProxy : IHANObject
    {
        IHANObject HObject { get; }
    }
}