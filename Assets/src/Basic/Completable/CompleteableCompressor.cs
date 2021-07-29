using System.Collections.Generic;
using System.Linq;

namespace HAN.Lib.Basic
{
    /**
     * Compresses completables, 
     * as a Completable can be completed without sending a signal 
     * we need this special SignalCompressor
     */
    public class CompleteableCompressor : SignalCompressor
    {
        public override MetaType Type() { return new MetaType( "CompletableCompressor" ); }
        public override bool Completed {
            get
            {
                foreach( var signalOwner in m_signalOwner )
                {
                    if( signalOwner is ICompleteable completable )
                    {
                        if( !completable.Completed )
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }


        public CompleteableCompressor( IEnumerable<ICompleteable> a_signalOwner )
                                           // filter out all completed, as we dont care about them
            : base( Keys.Signal.Completed, a_signalOwner.Where( c => !c.Completed ) ) 
        {
        }


        protected override void completed( ISignalPublisher a_signalOwner )
        {
            if( Completed )
            { 
                m_completedSignal.Emit();
                DisconnectEstablishedConnections();
            }
        }
    }
}