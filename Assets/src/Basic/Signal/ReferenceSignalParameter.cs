using HAN.Lib.Structure;

namespace HAN.Lib.Basic
{
    /**
     * Saves a reference for the signal
     */
    public class ReferenceSignalParameter : BasicSignalParameter
    {
        public object Reference { get; private set; }

        public ReferenceSignalParameter( object a_reference, ISignalPublisher a_sender )
          : base( a_sender )
        {
            Reference = a_reference;
        }
    }
}