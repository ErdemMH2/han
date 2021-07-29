using HAN.Lib.Basic;
using System.Collections.Generic;
using System.Linq;

namespace HAN.Lib.Mvc.Model
{
    /**
     * Filters all entrys which are not conform with filterSource
     */
    public class ModelFilter<I, T> : HObject 
                                     where I : IncrementalId
    {
        public override MetaType Type() { return new MetaType( "ModelFilter" ); }

        public delegate bool FilterSource( T a_sourceObject );
        private FilterSource m_filterMethod;

        private IHANModel<I, T> m_source;
        private IHANWriteableModel<I, T> m_target;


        public ModelFilter( IHANModel<I, T> a_source
                          , IHANWriteableModel<I, T> a_target
                          , FilterSource a_filterSource
                          , bool a_createModel = true )
        {
            m_source = a_source;
            m_target = a_target;

            m_filterMethod = a_filterSource;

            m_source.Connect( this, HAN.Lib.Basic.Keys.Signal.ChangedSignal
                            , onSourceChanged );

            if( a_createModel ) { 
                fillTargetModel();
            }
        }


        /// Updates filter and refilters complete model
        public void Filter( FilterSource a_filterSource = null )
        {
            if( a_filterSource != null ) { 
                m_filterMethod = a_filterSource;
            }


            // remove not in (new) filter, but in target model
            List<I> toRemove = new List<I>();
            foreach( var entry in m_target.Entrys )
            {
                if( !m_filterMethod( entry.Value ) )
                {
                    toRemove.Add( entry.ModelId );
                }
            }

            foreach( var removeId in toRemove )
            {
                m_target.Remove( removeId );
            }


            // insert new ones from source
            foreach( var entry in m_source.Entrys )
            {
                if( m_filterMethod( entry.Value ) )
                {
                    if( !m_target.Values.Contains( entry.Value ) )
                    {
                        m_target.Insert( entry.ModelId, entry.Value );
                    }
                }
            }
        }


        private void fillTargetModel()
        {
            foreach( var entry in m_source.Entrys )
            {
                if( m_filterMethod( entry.Value ) )
                {
                    m_target.Insert( entry.ModelId, entry.Value );
                }
            }
        }


        private void onSourceChanged( BasicSignalParameter a_parameter )
        {
            if( a_parameter is ModelSignalParameter<I, T> modelSignal )
            {
                if( modelSignal.SignalType == Mvc.Keys.Model.Insert )
                {
                    if( m_filterMethod( modelSignal.Entry.Value ) ) { 
                        m_target.Insert( modelSignal.Entry.ModelId, modelSignal.Entry.Value );
                    }
                }
                else if( modelSignal.SignalType == Mvc.Keys.Model.Append )
                {
                    if( m_filterMethod( modelSignal.Entry.Value ) ) {
                        m_target.Append( modelSignal.Entry.Value );
                    }
                }
                else if( modelSignal.SignalType == Mvc.Keys.Model.Assign )
                {
                    if( m_filterMethod( modelSignal.Entry.Value ) ) {
                        m_target.Assign( modelSignal.Entry.ModelId, modelSignal.Entry.Value );
                    }
                }
                else if( modelSignal.SignalType == Mvc.Keys.Model.Remove )
                {
                    if( m_filterMethod( modelSignal.Entry.Value ) ) {
                        m_target.Remove( modelSignal.Entry.ModelId );
                    }
                }
            }
        }
    }
}