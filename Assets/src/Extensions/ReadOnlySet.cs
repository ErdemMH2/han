using System.Collections.Generic;
using System.Collections;

namespace HAN.Lib.Extension.Collections
{
    /**
     * Decorator for sets, that should be changed
     * Implements ISet, but writes are not succeeding
     */
    public class ReadOnlySet<T> : IReadOnlyCollection<T>, ISet<T>
    {
        private readonly ISet<T> m_set;


        public ReadOnlySet( ISet<T> set )
        {
            m_set = set;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ( (IEnumerable) m_set ).GetEnumerator();
        }

        void ICollection<T>.Add( T item )
        {
        }

        public void UnionWith( IEnumerable<T> other )
        {
        }

        public void IntersectWith( IEnumerable<T> other )
        {
        }

        public void ExceptWith( IEnumerable<T> other )
        {
        }

        public void SymmetricExceptWith( IEnumerable<T> other )
        {
        }

        public bool IsSubsetOf( IEnumerable<T> other )
        {
            return m_set.IsSubsetOf( other );
        }

        public bool IsSupersetOf( IEnumerable<T> other )
        {
            return m_set.IsSupersetOf( other );
        }

        public bool IsProperSupersetOf( IEnumerable<T> other )
        {
            return m_set.IsProperSupersetOf( other );
        }

        public bool IsProperSubsetOf( IEnumerable<T> other )
        {
            return m_set.IsProperSubsetOf( other );
        }

        public bool Overlaps( IEnumerable<T> other )
        {
            return m_set.Overlaps( other );
        }

        public bool SetEquals( IEnumerable<T> other )
        {
            return m_set.SetEquals( other );
        }

        public bool Add( T item )
        {
            return false;
        }

        public void Clear()
        {
        }

        public bool Contains( T item )
        {
            return m_set.Contains( item );
        }

        public void CopyTo( T[] array, int arrayIndex )
        {
            m_set.CopyTo( array, arrayIndex );
        }

        public bool Remove( T item )
        {
            return false;
        }

        public int Count
        {
            get { return m_set.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }
    }
}