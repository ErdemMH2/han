using HAN.Lib.Basic;

namespace HAN.Lib.Mvc.Model
{
    /**
     * Maps all write operations from the source model to the target model
     */
    public class ModelMapper<I, ST, TT> : HObject // ST - Source Type <> TT - Target Type
                                          where I : IncrementalId
    {
        public override MetaType Type() { return new MetaType( "ModelBridge" ); }

        private IHANModel<I, ST> m_source;
        private IHANWriteableModel<I, TT> m_target;
        
        public delegate TT CreateTargetFromSource( ST a_sourceObject );
        private CreateTargetFromSource m_factoryMethod;


        public ModelMapper( IHANModel<I, ST> a_source
                          , IHANWriteableModel<I, TT> a_target
                          , CreateTargetFromSource a_factoryMethod )
        {
            m_source = a_source;
            m_target = a_target;
            m_factoryMethod = a_factoryMethod;

            m_source.Connect( this, HAN.Lib.Basic.Keys.Signal.ChangedSignal, onSourceChanged );

            fillTargetModel();
        }


        private void fillTargetModel()
        {
            foreach( var entry in m_source.Entrys )
            {
                var targetObject = m_factoryMethod( entry.Value );
                m_target.Insert( entry.ModelId, targetObject );
            }
        }


        private void onSourceChanged( BasicSignalParameter a_parameter )
        {
            if( a_parameter is ModelSignalParameter<I, ST> modelSignal )
            {
                if( modelSignal.SignalType == Mvc.Keys.Model.Insert )
                {
                    var targetObject = m_factoryMethod( modelSignal.Entry.Value );
                    m_target.Insert( modelSignal.Entry.ModelId, targetObject );
                }
                else if( modelSignal.SignalType == Mvc.Keys.Model.Append )
                {
                    var targetObject = m_factoryMethod( modelSignal.Entry.Value );
                    m_target.Append( targetObject );
                }
                else if( modelSignal.SignalType == Mvc.Keys.Model.Assign )
                {
                    var targetObject = m_factoryMethod( modelSignal.Entry.Value );
                    m_target.Assign( modelSignal.Entry.ModelId, targetObject );
                }
                else if( modelSignal.SignalType == Mvc.Keys.Model.Remove )
                {
                    m_target.Remove( modelSignal.Entry.ModelId );
                }
            }
        }
    }
}