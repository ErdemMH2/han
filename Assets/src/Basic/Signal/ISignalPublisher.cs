using HAN.Lib.Structure;
using System;

namespace HAN.Lib.Basic
{
    /**
     * Publishes signals, subscribers can register for signals and 
     * subscribers are automaticaly informed if this objects canceles signal propagation
     */
    public interface ISignalPublisher : IHANType, ISignalDisconnectable
    {
        /**
         * Connects the receivers slot to the singnal of this with the key, 
         * @return true if connection was successful
         * @warning this method should also call ConnectedTo on the receiver on success
         */
        bool Connect( ISignalSubscriber a_subscriber, Key a_signal, Action<BasicSignalParameter> a_slot );

        /**
         * Disconnects the receiver from the singnal of this with the key, 
         * @return true if disconnection was successful
         * @warning this method should also call DisconnectedFrom on the receiver on success
         */
        bool Disconnect( ISignalSubscriber a_subscriber, Key a_signal, Action<BasicSignalParameter> a_slot );
    }
}