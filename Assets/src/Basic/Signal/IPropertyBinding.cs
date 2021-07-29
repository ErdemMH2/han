using HAN.Lib.Structure;

namespace HAN.Lib.Basic
{
    public interface IPropertyBinding : ISignalSubscriber
    {
        /**
         * The Key Id of the Binding. The key is the binded property key.
         */
        Key Id { get; }
    }
}
