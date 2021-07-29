namespace HAN.Lib.LinkSystem
{
    /**
     * Interface for Link's
     */
    public interface IDoubleLinkTreeEntry<T>
    {
        DoubleLink<T> Link { get; }
    }
}