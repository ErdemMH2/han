using UnityEngine;

namespace HAN.Lib.Basic.Unity
{
    public class CommandCollectionHBehavior : AbstractCommandHBehavior
    {
        [SerializeField] private AbstractCommandHBehavior[] m_commands;


        protected override ICommand createCommand()
        {
            CommandCollection collection = new CommandCollection( Id );
            foreach( var command in m_commands )
            {
                collection.Push( command );
            }

            return collection;
        }
    }
}