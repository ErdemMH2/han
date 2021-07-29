using HAN.Lib.Structure;
using System.Collections.Generic;

namespace HAN.Lib.Basic
{
    /**
     * The parameter of System.Action<BasicSignalParameter> propagated on signal emission
     */
    public class CountChangedSignalParameter<T> : BasicSignalParameter
    {
        public bool Added { get { return m_added; } }
        public bool Removed { get { return !m_added; } }

        public IEnumerable<T> Changed { get; private set; }

        private bool m_added = false;


        /**
         * <param name="a_changedIds">Ids of changed entries</param>
         * <param name="a_added">If added true; else removed</param>
         */
        public CountChangedSignalParameter( IEnumerable<T> a_changedIds, bool a_added, ISignalPublisher a_sender ) 
          : base( a_sender )
        {
            Changed = a_changedIds;
            m_added = a_added;
        }
    }
}
