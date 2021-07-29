using HAN.Lib.Basic;
using System.Collections.Generic;

namespace HAN.Lib.Mvc.Model
{
    /**
     * Merges all entrys together by ignoring mapping Ids
     * WARNING: Do not change the target model after merging, as the Ids have to be valid
     * for the ModelMerger
     */
    public class ModelMerger<I, T> : HObject
                                     where I : IncrementalId
    {
        public override MetaType Type() { return new MetaType( "ModelMerger" ); }

        private IEnumerable<IHANModel<I, T>> m_sources;
        private IHANWriteableModel<I, T> m_target;
        private Dictionary<IHANModel<I, T>, Dictionary<I, I>> m_map 
                    = new Dictionary<IHANModel<I, T>, Dictionary<I, I>>();


        public ModelMerger( IEnumerable<IHANModel<I, T>> a_source
                          , IHANWriteableModel<I, T> a_target )
        {
            m_sources = a_source;
            m_target = a_target;

            foreach( var source in m_sources )
            {
                addToTargetModel( source );
                source.Connect( this, HAN.Lib.Basic.Keys.Signal.ChangedSignal
                              , onSourceChanged );
            }
        }

        
        private void addToTargetModel( IHANModel<I, T> a_source )
        {
            m_map.Add( a_source, new Dictionary<I, I>() );
            foreach( var entry in a_source.Entrys )
            {
                var targetModelId = m_target.Append( entry.Value );
                m_map[a_source].Add( entry.ModelId, targetModelId );
            }
        }


        private void onSourceChanged( BasicSignalParameter a_parameter )
        {
            if( a_parameter is ModelSignalParameter<I, T> modelSignal )
            {
                var source = modelSignal.Model;
                var entry = modelSignal.Entry;

                if( modelSignal.SignalType == Mvc.Keys.Model.Insert
                 || modelSignal.SignalType == Mvc.Keys.Model.Append )
                {
                    var targetModelId = m_target.Append( modelSignal.Entry.Value );
                    m_map[source].Add( entry.ModelId, targetModelId );
                }
                else if( modelSignal.SignalType == Mvc.Keys.Model.Assign )
                {
                    if( m_map[source].TryGetValue( entry.ModelId, out var targetModelId ) )
                    {
                        m_target.Assign( targetModelId, modelSignal.Entry.Value );
                    }
                    else
                    {
                        HAN.Debug.Logger.Error( "ModelMerger"
                                              , string.Format( "Cant assign target model id of {0} {1}"
                                                              , source, entry.ModelId ) );
                    }
                }
                else if( modelSignal.SignalType == Mvc.Keys.Model.Remove )
                {
                    if( m_map[source].TryGetValue( entry.ModelId, out var targetModelId ) )
                    {
                        m_target.Remove( targetModelId );
                        m_map[source].Remove( entry.ModelId );
                    }
                    else
                    {
                        HAN.Debug.Logger.Error( "ModelMerger"
                                              , string.Format( "Cant remove target model id of {0} {1}"
                                                              , source, entry.ModelId ) );
                    }
                }
            }
        }
    }
}