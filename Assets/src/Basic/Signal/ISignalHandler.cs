
namespace HAN.Lib.Basic
{
    /**
     * Interface to describe a IGenericSignalHandler,
     * that will use ISignalPublisher interface to connect and disconnect
     */
    public interface ISignalHandler : IGenericSignalHandler
                                    , ISignalPublisher
    {

    }
}