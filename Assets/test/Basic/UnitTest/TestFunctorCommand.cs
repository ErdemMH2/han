using HAN.Lib.Test;
using HAN.Lib.Basic;
using HAN.Lib.Structure;

public class TestFunctorCommand : UnitTest
{
    private static readonly Key k_FunctorCommand = KeyFactory.Create("a_FunctorCommand");

    void TestExecute(ref TestResult r_result) 
    {
        bool command_executed = false;
        FunctorCommand command = new FunctorCommand(k_FunctorCommand, () => 
        {
            command_executed = true; 
            return false; 
        }, () => { return true; });
        command.Execute();
        r_result.Verify(command_executed, "Command was executed. Test OK!");
    }

    void TestEvaluateExecute(ref TestResult r_result) 
    {
        FunctorCommand command = new FunctorCommand(k_FunctorCommand,
        () => { return true; },
        () => { return false; }
        );

        r_result.Compare(command.CanExecute, false, "Command can not executable.");
    }

    public override void Init() 
    {
        this.addTest(this.TestExecute, "Execute Command and check if executed.");
        this.addTest(this.TestEvaluateExecute, "Execute Command and check if not executed when ExecuteFunctor return false.");
    }

    public override void Prepare() { }

    public override void PrepareTest() { }

    public override void CleanupTest() { }

    public override void Cleanup() { }
}
