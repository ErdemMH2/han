using HAN.Lib.Basic;
using HAN.Lib.Structure;

namespace HAN.Lib.Mvc.Model 
{
    /**
     * Signal parameter for table model based changes
     */
    public class ModelSignalParameter<I, T> : BasicSignalParameter
                                              where I : IncrementalId
    {
        public ModelEntry<I, T> Entry { get; private set; }
        public Key SignalType { get; private set; }

        public IHANModel<I, T> Model { get; private set; }


        public ModelSignalParameter( ModelEntry<I,T> a_entry
                                   , Key a_signalType
                                   , IHANModel<I, T> a_model ) 
          : base( a_model )
        {
            Entry      = a_entry;
            SignalType = a_signalType;
            Model      = a_model;
        }
    }
}