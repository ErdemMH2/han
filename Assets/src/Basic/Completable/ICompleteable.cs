
namespace HAN.Lib.Basic
{
    /**
     * Has an complete boolean and should signalize completion
     * but can be complete from start, without sending a signal
     */
    public interface ICompleteable : ISignalPublisher
    {
        bool Completed { get; }
    }
}