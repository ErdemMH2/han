using HAN.Lib.Structure;


namespace HAN.Lib.Basic
{
    public static class Keys
    {
        /**
         * Often used Signals (static) Keys
         */
        public static SignalKeys Signal { get { return SignalKeys.Instance; } }
        public class SignalKeys : Singleton<SignalKeys>
        {
            public readonly Key ChangedSignal = KeyFactory.Create( "ChangedSignal" );
            public readonly Key CountChanged = KeyFactory.Create("CountChanged");

            public readonly Key Loaded = KeyFactory.Create("Loaded");
            public readonly Key Unloaded = KeyFactory.Create("Unloaded");

            public readonly Key Selected = KeyFactory.Create("Selected");
            public readonly Key Deselected = KeyFactory.Create( "Deselected" );

            public readonly Key Confirmed = KeyFactory.Create("Confirmed");
            public readonly Key Completed = KeyFactory.Create( "Completed" );

            public readonly Key Activated = KeyFactory.Create("Activated");
            public readonly Key Deactivated = KeyFactory.Create( "Deactivated" );

            public readonly Key Reset = KeyFactory.Create( "Reset" );

            public readonly Key Current = KeyFactory.Create( "Current" );
        }

        public static CommandKeys Command { get { return CommandKeys.Instance; } }
        public class CommandKeys : Singleton<CommandKeys>
        {
            public readonly Key State = KeyFactory.Create( "State" );
            public readonly Key CanExecute = KeyFactory.Create( "CanExecute" );
            public readonly Key ExcutionState = KeyFactory.Create( "ExcutionState" );

        }

        
        public static CompleteableStateKeys CompleteableState { get { return CompleteableStateKeys.Instance; } }
        public class CompleteableStateKeys : Singleton<CompleteableStateKeys>
        {
            public readonly Key Unknown = KeyFactory.Create( "Unknown" ); // dont use; reserved for internal use!
            public readonly Key Succeeded = KeyFactory.Create( "Succeeded" );
            public readonly Key Failed = KeyFactory.Create( "Failed" );
        }
    }
}