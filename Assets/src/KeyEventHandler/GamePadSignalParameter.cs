using HAN.Lib.Basic;
using HAN.Lib.Structure;

namespace HAN.Lib.Event
{
    /**
     * SignalParameter with the Input event key and signal (Clicked, Pressed...)
     * */
    public class GamePadSignalParameter : BasicSignalParameter
    {
        public Key Input { get; private set; }
        public Key Signal { get; private set; }

        public GamePadSignalParameter( ISignalPublisher a_sender, Key a_input, Key a_signal )
        : base( a_sender )
        {
            Signal = a_signal;
            Input = a_input;
        }
    }
}
