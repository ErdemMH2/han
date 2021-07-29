using HAN.Lib.Structure;

namespace HAN.Lib.Basic
{
    /**
     * Publishes signals, subscribers can register for signals and 
     * subscribers are automaticaly informed if this objects canceles signal propagation
     */
    public interface ISignalDisconnectable
    {
        /**
         * Disconnects all connections of all subscibers
         */
        void DisconnectEstablishedConnections();
    }
}