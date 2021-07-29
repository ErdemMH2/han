using System.Collections;

namespace HAN.Lib.Test
{
    public interface ITestExecutor
    {
        IEnumerator Execute( Test a_executed, TestSuiteResult a_suitResult );
    }
}