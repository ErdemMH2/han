using HAN.Lib.Structure;


namespace HAN.Lib.Basic
{
    /** 
     * Command pattern interface.
     */
    public interface ICommand : IHANObject
    {
        Key Id { get; }

        /// Can this Command be executed?
        bool CanExecute { get; }

        /// returns the executions states, descirbed by the particular Command
        IncrementalId State { get; }

        void EvaluateExecute();

        /// Execute Command
        bool Execute();
    }
}