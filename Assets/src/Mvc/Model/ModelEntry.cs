using HAN.Lib.Basic;
using HAN.Lib.Structure;

namespace HAN.Lib.Mvc.Model
{
    /**
     * The entry link is owned by the model 
     * and will be used to singalize events or update the data. 
     * The consumer has to fetch the ModelEntryLink and an use it while lifetime 
     * to proccess the data. 
     * If The model decides that this entry is not used anymore, this ModelEntryLink will
     * be invalidated.
     */
    public class ModelEntry<I, T> : HObject
                                    where I : IncrementalId
    {
        public override MetaType Type() { return new MetaType( KeyFactory.Create( "ModelEntry" ) ); }

        /// Id of the entry in the model
        public I ModelId { get; private set; }

        public T Value { get; protected set; }

        /// OnChanged Singnal will be emitted after any change on the linked model data happens.
        protected Signal m_onChanged;


        public ModelEntry( I a_modelId, T a_value )
        {
            ModelId = a_modelId;
            Value = a_value;

            m_onChanged = new Signal( this );
            addSignal( Basic.Keys.Signal.ChangedSignal, m_onChanged );
        }
    }
}