using HAN.Lib.Basic;

namespace HAN.Lib.Structure
{
    /**
     * Interface to reset and return pool entries to the pool
     */
    public interface IPoolEntry : IResetable
    {
        IncrementalId PoolId { get; }

        void InitPoolEntry( IncrementalId a_poolId, Pool a_parent );
        void ReturnToPool();

        void Activate();
    }
}