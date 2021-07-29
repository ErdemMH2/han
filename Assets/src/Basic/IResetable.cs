namespace HAN.Lib.Basic
{
    public interface IResetable
    {
        /// Will reset the object to original state 
        /// it is called ResetIt() because Reset() is used by unity
        void ResetIt();
    }
}