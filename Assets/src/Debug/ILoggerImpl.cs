using HAN.Lib.Structure;
using UnityEngine;

namespace HAN.Debug
{
    public interface ILoggerImpl : ILogHandler
    {
        void AddMessage( Key a_level, Key a_tag, string a_message );
    }
}