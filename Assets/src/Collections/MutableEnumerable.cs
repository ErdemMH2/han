using System.Collections;
using System.Collections.Generic;

namespace HAN.Lib.Basic.Collections
{
    /**
     * The MutableEnumerable will try to reuse its memory and 
     * is able to Add/Remove while enumerating
     */
    public class MutableEnumerable<T> : IEnumerable<T>
                                        where T : class
    {
        private List<T> m_list; // RAII

        public IEnumerator<T> GetEnumerator()
        {
            if( m_list != null )
            {
                for( int i = 0; i < m_list.Count; i++ )
                {
                    var t = m_list[i];
                    if( t == null ) {
                        continue;
                    }
                    yield return t;
                }
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            if( m_list != null )
            {
                for( int i = 0; i < m_list.Count; i++ )
                {
                    var t = m_list[i];
                    if( t == null ) {
                        continue;
                    }

                    yield return t;
                }
            }
        }


        /**
         * Finds T according Func and returns it, else default
         */
        public T Find( System.Func<T, bool> a_predicate )
        {
            foreach( var t in this )
            {
                if( a_predicate( t) )
                {
                    return t;
                }
            }

            return default;
        }


        /**
         * Will add a_t to the list, even when iterated
         */
        public void Add( T a_t )
        {
            if( m_list == null ) {
                m_list = new List<T>();
            }

            int indx = firstEntry();
            if( indx >= 0 )
            {
                m_list[indx] = a_t;
            }
            else 
            {
                m_list.Add( a_t );
            }
        }


        /**
         * Will remove a_t to the list, even when iterated
         */
        public void Remove( T a_t )
        {
            if( m_list != null )
            {
                int indx = firstEntry( a_t );
                if( indx >= 0 )
                {
                    m_list[indx] = null;
                }
            }
        }


        private int firstEntry( T a_t = null )
        {
            return m_list.FindIndex( s => EqualityComparer<T>.Default.Equals( s, a_t ) );
        }
    }
}