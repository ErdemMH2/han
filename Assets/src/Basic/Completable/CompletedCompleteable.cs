using HAN.Lib.Structure;

namespace HAN.Lib.Basic
{
    /**
     * Constant completed Completable
     * Used for instantly completed operations, will not signalize Complete
     */
    public class CompletedCompleteable : HObject
                                       , ICompleteable
    {
        public override MetaType Type()  { return new MetaType( KeyFactory.Create( "CompletedCompleteable" ) ); }

        public CompletedCompleteable() { }

        public bool Completed => true;
    }
}