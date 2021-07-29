using HAN.Lib.Structure;


namespace HAN.Lib.Basic
{
    /** 
     * Command access
     * Will hold and organize commands via Keys
     */
    public interface ICommandAccess
    {
        /// is the Command available on the ICommandAccess
        bool HasCommand( Key a_key );

        /// returns the Command with a_key or null if not found
        ICommand Command( Key a_key );

        /// executes the Command, if found and returns the Execution of the Command or else false
        bool ExecuteCommand( Key a_key );

        /// Adds a Command to the ICommandAccess
        bool AddCommand( ICommand a_prop );

        /// Removes the Command from the ICommandAccess if possible
        bool RemoveCommand( Key a_key );
    }

}