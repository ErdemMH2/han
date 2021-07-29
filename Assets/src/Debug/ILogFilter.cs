using HAN.Lib.Structure;


namespace HAN.Debug
{
    public interface ILogFilter
    {
        bool Filter( Key a_level, Key a_tag, string a_message );
    }
}