using HAN.Lib.Extension.Collections;
using HAN.Lib.Structure;
using System.Collections.Generic;

namespace HAN.Lib.LinkSystem
{
    /**
     * Tree which will separate  Completed and Available links 
     * according a_completed List and connections betweens links
     */
    public class DoubleLinkTree<T>
    {
        public List<T> Available { get; private set; }

        public ReadOnlySet<T> Completed { get { return m_completed; } }
        private ReadOnlySet<T> m_completed;
        private IEnumerable<DoubleLink<T>> m_tree;


        public DoubleLinkTree( ReadOnlySet<T> a_completed
                             , IEnumerable<DoubleLink<T>> a_tree )
        {
            m_completed = a_completed;
            m_tree = a_tree;
            Available = new List<T>();

            UpdateTree();
        }


        public DoubleLinkTree( IEnumerable<T> a_completed
                             , IEnumerable<DoubleLink<T>> a_tree )
           : this( new ReadOnlySet<T>( new HashSet<T>( a_completed ) ), a_tree )
        {
        }


        public void UpdateTree()
        {
            Available.Clear();
            foreach( var link in m_tree )
            {
                if( !m_completed.Contains( link.Id ) )
                { 
                    bool available = true;

                    // check if all requirements are fulfilled
                    foreach( var predecessor in link.Predecessors )
                    {
                        if( !m_completed.Contains( predecessor ) )
                        {
                            available = false;
                            break;
                        }
                    }

                    if( available ) {
                        Available.Add( link.Id );
                    }
                }
            }
        }
    }
}