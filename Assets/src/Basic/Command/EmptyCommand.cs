using HAN.Lib.Structure;

namespace HAN.Lib.Basic
{
    public class EmptyCommand : AbstractCommand
    {
        public EmptyCommand( Key a_id ) 
          : base( a_id )
        {
            m_canExecute.SetValue( true );
        }


        public override bool Execute()
        {
            return true;
        }


        public override string ToString()
        {
            return "";
        }
    }
}