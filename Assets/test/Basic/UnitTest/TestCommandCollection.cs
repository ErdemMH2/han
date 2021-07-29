using HAN.Lib.Test;
using HAN.Lib.Structure;
using HAN.Lib.Basic;

public class TestCommandCollection : UnitTest
{
    private static readonly Key k_TestCommandCollection = KeyFactory.Create("a_TestCommandCollection");
    private static readonly Key k_FunctorCommand = KeyFactory.Create("a_FunctorCommand");

    public void TestCommandCollectionPush(ref TestResult r_result) 
    {
        CommandCollection collection = new CommandCollection(k_TestCommandCollection);
        int command_count = 0;
        for (int i = 0; i < 10; ++i)
        {
            collection.Push(new FunctorCommand(k_FunctorCommand, () => { ++command_count; return true; }));
        }
        collection.Execute();
        r_result.Compare(command_count, 10, "All of function command is executed by CommandCollection.");
    }

    public void TestCommandCollectionSignal(ref TestResult r_result) 
    {
        CommandCollection collection = new CommandCollection(k_TestCommandCollection);
        int command_count = 0;
        for (int i = 0; i < 10; ++i)
        {
            collection.Push(new FunctorCommand(k_FunctorCommand, () => { ++command_count; return true; }));
        }
        bool completed = false;
        collection.Connect(collection, Keys.Signal.Completed, (BasicSignalParameter signal_parameter) => { completed = true; });
        collection.Execute();
        r_result.Verify(completed, "CommandCollection complete signal was received.");
    }

    public override void Cleanup()
    {
    }

    public override void CleanupTest()
    {
    }

    public override void Init()
    {
        this.addTest(this.TestCommandCollectionPush, "Push 10 Command and check all command if executed.");
        this.addTest(this.TestCommandCollectionSignal, "Push 10 Command and receive signal if all command executed.");
    }

    public override void Prepare()
    {
    }

    public override void PrepareTest()
    {
    }
}
