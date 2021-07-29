using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HAN.Lib.Structure;
using HAN.Lib.Basic;
using HAN.Lib.Test;



public class TestSignalCompressor : UnitTest
{
    private static readonly Key k_DummyCompleteableCommand = KeyFactory.Create("a_DummyCompleteableCommand");

    public void TestSignalCompressorSignalReceived(ref TestResult r_result) 
    {
        List<ISignalPublisher> a_signalOwner = new List<ISignalPublisher>();

        for (int i = 0; i < 3; ++i)
        {
            a_signalOwner.Add(new DummyCompleteableCommand(k_DummyCompleteableCommand));
        }
        SignalCompressor compressor = new SignalCompressor(Keys.Signal.Completed, a_signalOwner);
        bool completed = false;
        compressor.Connect(compressor, Keys.Signal.Completed, (BasicSignalParameter parameter)=> { completed = true; });
        foreach (var e in a_signalOwner) 
        {
            ((DummyCompleteableCommand)e).Execute();
        }
        r_result.Verify(completed, "All of completeable command is executed.");
    }

    public override void Cleanup()
    {
    }

    public override void CleanupTest()
    {
    }

    public override void Init()
    {
        this.addTest(this.TestSignalCompressorSignalReceived, "Test compressor signal received if all publishers signals were raised.");
    }

    public override void Prepare()
    {
    }

    public override void PrepareTest()
    {
    }


    private class DummyCompleteableCommand : CompleteableCommand
    {
        public DummyCompleteableCommand(Key a_id) : base(a_id) { }

        public override bool Execute()
        {
            this.complete();
            return true;
        }
    }
}
