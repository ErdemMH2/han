using HAN.Lib.Basic;
using HAN.Lib.Structure;

namespace HAN.Lib.Mvc.Model
{
    public class WritableModelEntry<I, T> : ModelEntry<I, T>
                                            where I : IncrementalId
                                            where T : class
    {
        public WritableModelEntry( I a_modelId, T a_payload ) 
          : base( a_modelId, a_payload )
        {
        }


        /// update entry with other entry values
        public bool Assign( T a_payload )
        {
            if( Value != a_payload )
            {
                Value = a_payload;
                m_onChanged.Emit();

                return true;
            }

            return false;
        }
    }
}