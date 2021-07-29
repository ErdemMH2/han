using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HAN.Lib.Mvc;
using HAN.Lib.Mvc.Model;
using HAN.Lib.Structure;
using HAN.Lib.Test;

public class TestModelFilter : UnitTest
{
    public class LogListModel : AbstractListModel<string>, IHANWriteableModel<TableId, string>
    {
        public override IEnumerable<string> Values { get { return new List<string>(); } }
        public override IEnumerable<ModelEntry<TableId, string>> Entrys { get { return new List<ModelEntry<TableId, string>>(); } }

        public override int ColumnCount() { return 0; }
        public override ModelEntry<TableId, string> Position(TableId a_id) { return null; }
        public override string Value(TableId a_id) { return null; }

        public delegate void EmitFunction();
        EmitFunction[] functions = new EmitFunction[4];

        public LogListModel(EmitFunction append_func, EmitFunction insert_func, EmitFunction assign_func, EmitFunction remove_func)
        {
            this.functions[0] = append_func;
            this.functions[1] = insert_func;
            this.functions[2] = assign_func;
            this.functions[3] = remove_func;
        }

        public bool Assign(TableId a_id, string a_entry)
        {
            m_onChanged.Emit(new ModelSignalParameter<TableId, string>(new ModelEntry<TableId, string>(new TableId(0), ""), HAN.Lib.Mvc.Keys.Model.Assign, this));
            this.functions[2]();
            return true;
        }

        public bool Insert(TableId a_id, string a_entry)
        {
            m_onChanged.Emit(new ModelSignalParameter<TableId, string>(new ModelEntry<TableId, string>(new TableId(0), ""), HAN.Lib.Mvc.Keys.Model.Insert, this));
            this.functions[1]();
            return true;
        }

        public TableId Append(string a_entry)
        {
            m_onChanged.Emit(new ModelSignalParameter<TableId, string>(new ModelEntry<TableId, string>(new TableId(0), ""), HAN.Lib.Mvc.Keys.Model.Append, this));
            this.functions[0]();
            return new TableId(0);
        }

        public bool Remove(TableId a_id)
        {
            m_onChanged.Emit(new ModelSignalParameter<TableId, string>(new ModelEntry<TableId, string>(new TableId(0), ""), HAN.Lib.Mvc.Keys.Model.Remove, this));
            this.functions[3]();
            return true;
        }
    }

    public class TriggerListModel : AbstractListModel<string>
    {
        public override IEnumerable<string> Values { get { return new List<string>(); } }
        public override IEnumerable<ModelEntry<TableId, string>> Entrys { get { return new List<ModelEntry<TableId, string>>(); } }
        public override int ColumnCount() { return 0; }

        public override ModelEntry<TableId, string> Position(TableId a_id) { return null; }
        public override string Value(TableId a_key) { return null; }

        public TriggerListModel() { }

        public void EmitInsert()
        {
            this.m_onChanged.Emit(new ModelSignalParameter<TableId, string>(new ModelEntry<TableId, string>(new TableId(0), "Test Entry Value"), HAN.Lib.Mvc.Keys.Model.Insert, this));
        }
        public void EmitRemove()
        {
            this.m_onChanged.Emit(new ModelSignalParameter<TableId, string>(new ModelEntry<TableId, string>(new TableId(0), "Test Entry Value"), HAN.Lib.Mvc.Keys.Model.Remove, this));
        }
        public void EmitAppend()
        {
            this.m_onChanged.Emit(new ModelSignalParameter<TableId, string>(new ModelEntry<TableId, string>(new TableId(0), "Test Entry Value"), HAN.Lib.Mvc.Keys.Model.Append, this));
        }
        public void EmitAssign()
        {
            this.m_onChanged.Emit(new ModelSignalParameter<TableId, string>(new ModelEntry<TableId, string>(new TableId(0), "Test Entry Value"), HAN.Lib.Mvc.Keys.Model.Assign, this));
        }
    }

    public void TestModelFilterConstructor(ref TestResult r_result)
    {
        ListModel<string> model_source = new ListModel<string>();
        model_source.Append("element1");
        model_source.Append("_underScoredElement1");
        model_source.Append("_underScoredElement2");
        model_source.Append("element2");
        model_source.Append("element3");
        model_source.Append("_underScoredElement3");
        model_source.Append("_underScoredElement4");
        model_source.Append("element4");

        ListModel<string> model_destination = new ListModel<string>();
        ModelFilter<TableId, string> filter = new ModelFilter<TableId, string>(model_source, model_destination, (string element) => { return !element.StartsWith("_"); });

        r_result.Compare(model_destination.ColumnCount(), 4, "4 element was filtered by ModelFilter constructor");
    }

    public void TestEmitInsert(ref TestResult r_result)
    {
        TriggerListModel trigger_model = new TriggerListModel();
        bool insert_signal_emitted = false;
        bool append_signal_emitted = false;
        bool assign_signal_emitted = false;
        bool remove_signal_emitted = false;
        LogListModel log_model = new LogListModel(() => { append_signal_emitted = true; }, () => { insert_signal_emitted = true; }, () => { assign_signal_emitted = true; }, () => { remove_signal_emitted = true; });
        ModelFilter<TableId, string> mapper = new ModelFilter<TableId, string>(trigger_model, log_model, (string source) => { return true; });
        trigger_model.EmitInsert();
        r_result.Verify(insert_signal_emitted, "Insert signal emitted by ModelFilter");
        r_result.VerifyNot(append_signal_emitted, "Append signal emitted by ModelFilter");
        r_result.VerifyNot(assign_signal_emitted, "Assign signal emitted by ModelFilter");
        r_result.VerifyNot(remove_signal_emitted, "Remove signal emitted by ModelFilter");
    }
    public void TestEmitAppend(ref TestResult r_result)
    {
        TriggerListModel trigger_model = new TriggerListModel();
        bool insert_signal_emitted = false;
        bool append_signal_emitted = false;
        bool assign_signal_emitted = false;
        bool remove_signal_emitted = false;
        LogListModel log_model = new LogListModel(() => { append_signal_emitted = true; }, () => { insert_signal_emitted = true; }, () => { assign_signal_emitted = true; }, () => { remove_signal_emitted = true; });
        ModelFilter<TableId, string> mapper = new ModelFilter<TableId, string>(trigger_model, log_model, (string source) => { return true; });
        trigger_model.EmitAppend();
        r_result.VerifyNot(insert_signal_emitted, "Insert signal emitted by ModelFilter");
        r_result.Verify(append_signal_emitted, "Append signal emitted by ModelFilter");
        r_result.VerifyNot(assign_signal_emitted, "Assign signal emitted by ModelFilter");
        r_result.VerifyNot(remove_signal_emitted, "Remove signal emitted by ModelFilter");
    }
    public void TestEmitAssign(ref TestResult r_result)
    {
        TriggerListModel trigger_model = new TriggerListModel();
        bool insert_signal_emitted = false;
        bool append_signal_emitted = false;
        bool assign_signal_emitted = false;
        bool remove_signal_emitted = false;
        LogListModel log_model = new LogListModel(() => { append_signal_emitted = true; }, () => { insert_signal_emitted = true; }, () => { assign_signal_emitted = true; }, () => { remove_signal_emitted = true; });
        ModelFilter<TableId, string> mapper = new ModelFilter<TableId, string>(trigger_model, log_model, (string source) => { return true; });
        trigger_model.EmitAssign();
        r_result.VerifyNot(insert_signal_emitted, "Insert signal emitted by ModelFilter");
        r_result.VerifyNot(append_signal_emitted, "Append signal emitted by ModelFilter");
        r_result.Verify(assign_signal_emitted, "Assign signal emitted by ModelFilter");
        r_result.VerifyNot(remove_signal_emitted, "Remove signal emitted by ModelFilter");
    }
    public void TestEmitRemove(ref TestResult r_result)
    {
        TriggerListModel trigger_model = new TriggerListModel();
        bool insert_signal_emitted = false;
        bool append_signal_emitted = false;
        bool assign_signal_emitted = false;
        bool remove_signal_emitted = false;
        LogListModel log_model = new LogListModel(() => { append_signal_emitted = true; }, () => { insert_signal_emitted = true; }, () => { assign_signal_emitted = true; }, () => { remove_signal_emitted = true; });
        ModelFilter<TableId, string> mapper = new ModelFilter<TableId, string>(trigger_model, log_model, (string source) => { return true; });
        trigger_model.EmitRemove();
        r_result.VerifyNot(insert_signal_emitted, "Insert signal emitted by ModelFilter");
        r_result.VerifyNot(append_signal_emitted, "Append signal emitted by ModelFilter");
        r_result.VerifyNot(assign_signal_emitted, "Assign signal emitted by ModelFilter");
        r_result.Verify(remove_signal_emitted, "Remove signal emitted by ModelFilter");
    }
    public void TestFilterSource(ref TestResult r_result)
    {
        ListModel<string> model_source = new ListModel<string>();
        ListModel<string> filtered_model = new ListModel<string>();
        ModelFilter<TableId, string> filter = new ModelFilter<TableId, string>(model_source, filtered_model, (string element) => { return !element.StartsWith("_"); });

        model_source.Append("element1");
        model_source.Append("_underScoredElement1");
        model_source.Append("_underScoredElement2");
        model_source.Append("element2");
        model_source.Append("element3");
        model_source.Append("_underScoredElement3");
        model_source.Append("_underScoredElement4");
        model_source.Append("element4");

        r_result.Compare(filtered_model.ColumnCount(), 4, "4 element was filtered by ModelFilter");
    }

    public override void Init()
    {
        this.addTest(this.TestModelFilterConstructor, "Check if all data from source model to destination model in model filter constructor.");
        this.addTest(this.TestEmitAppend, "Check append signal emitted by ModelFilter");
        this.addTest(this.TestEmitInsert, "Check insert signal emitted by ModelFilter");
        this.addTest(this.TestEmitAssign, "Check assign signal emitted by ModelFilter");
        this.addTest(this.TestEmitRemove, "Check remove signal emitted by ModelFilter");
        this.addTest(this.TestFilterSource, "Check underscored elements is filtered by ModelFilter");
    }

    public override void Prepare() { }

    public override void PrepareTest() { }

    public override void CleanupTest() { }

    public override void Cleanup() { }
}
