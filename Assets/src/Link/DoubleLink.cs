using HAN.Lib.Extension.Collections;
using System.Collections.Generic;

namespace HAN.Lib.LinkSystem
{
    /**
     * A generic double linked tree node which connects data (not DoubleLink's) which each other
     * Each Link can have multiple successors and predecessors
     * T has to support GetHash()
     */
    public class DoubleLink<T>
    {
        public T Id { get; private set; }
        public ReadOnlySet<T> Successors { get; private set; }
        public ReadOnlySet<T> Predecessors { get; private set; }

        protected HashSet<T> m_successors;
        protected HashSet<T> m_predecessors;


        public DoubleLink( T a_id
                         , IEnumerable<T> a_successors
                         , IEnumerable<T> a_predecessor )
        {
            Id = a_id;
            m_successors = new HashSet<T>( a_successors );
            Successors = new ReadOnlySet<T>( m_successors );

            m_predecessors = new HashSet<T>( a_predecessor );
            Predecessors = new ReadOnlySet<T>( m_predecessors );
        }
    }
}