using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HAN.Lib.Mvc.Model;
using HAN.Lib.Test;
using HAN.Lib.Structure;

public class TestListModel : UnitTest
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

    public void TestListModelAppend(ref TestResult r_result)
    {
        ListModel<string> model = new ListModel<string>();
        model.Append("Element 1");
        model.Append("Element 2");
        model.Append("Element 3");
        model.Append("Element 4");
        r_result.Compare<int>(model.ColumnCount(), 4, "All elements are appended.");
    }
    public void TestListModelInsert(ref TestResult r_result)
    {
        ListModel<string> model = new ListModel<string>();
        model.Insert(new TableId(1), "Element 1");
        model.Insert(new TableId(2), "Element 2");

        r_result.Compare<int>(model.ColumnCount(), 2, "All elements are inserted.");
    }
    public void TestListModelRemove(ref TestResult r_result)
    {
        ListModel<string> model = new ListModel<string>();
        model.Append("Element 1");
        TableId id2 = model.Append("Element 2");
        model.Append("Element 3");
        model.Append("Element 4");
        model.Remove(id2);
        r_result.Compare<int>(model.ColumnCount(), 3, "Element is removed.");
    }
    public void TestListModelValue(ref TestResult r_result)
    {
        ListModel<string> model = new ListModel<string>();
        TableId id = model.Append("Element");
        r_result.Compare<string>(model.Value(id), "Element", "Value equals Element.");
    }
    public void TestListModelAssign(ref TestResult r_result)
    {
        ListModel<string> model = new ListModel<string>();
        TableId id = model.Append("old");
        model.Assign(id, "new");
        r_result.Compare<string>(model.Value(id), "new", "New value is assigned.");
    }
    public void TestListModelPosition(ref TestResult r_result)
    {
        ListModel<string> model = new ListModel<string>();
        TableId id = model.Append("value");
        ModelEntry<TableId, string> entry = model.Position(id);
        r_result.Compare<string>(entry.Value, "value", "Value from position is correct.");
    }

    public void TestEmitInsert(ref TestResult r_result)
    {
        ListModel<string> trigger_model = new ListModel<string>();
        bool insert_signal_emitted = false;
        bool append_signal_emitted = false;
        bool assign_signal_emitted = false;
        bool remove_signal_emitted = false;
        LogListModel log_model = new LogListModel(() => { append_signal_emitted = true; }, () => { insert_signal_emitted = true; }, () => { assign_signal_emitted = true; }, () => { remove_signal_emitted = true; });
        ModelMapper<TableId, string, string> mapper = new ModelMapper<TableId, string, string>(trigger_model, log_model, (string source) => { return source; });
        trigger_model.Insert(new TableId(0), "some value");
        r_result.Verify(insert_signal_emitted, "Insert signal emitted by SignalMapper");
        r_result.VerifyNot(append_signal_emitted, "Append signal emitted by SignalMapper");
        r_result.VerifyNot(assign_signal_emitted, "Assign signal emitted by SignalMapper");
        r_result.VerifyNot(remove_signal_emitted, "Remove signal emitted by SignalMapper");
    }
    public void TestEmitAppend(ref TestResult r_result)
    {
        ListModel<string> trigger_model = new ListModel<string>();
        bool insert_signal_emitted = false;
        bool append_signal_emitted = false;
        bool assign_signal_emitted = false;
        bool remove_signal_emitted = false;
        LogListModel log_model = new LogListModel(() => { append_signal_emitted = true; }, () => { insert_signal_emitted = true; }, () => { assign_signal_emitted = true; }, () => { remove_signal_emitted = true; });
        ModelMapper<TableId, string, string> mapper = new ModelMapper<TableId, string, string>(trigger_model, log_model, (string source) => { return source; });
        trigger_model.Append("");
        r_result.VerifyNot(insert_signal_emitted, "Insert signal emitted by SignalMapper");
        r_result.Verify(append_signal_emitted, "Append signal emitted by SignalMapper");
        r_result.VerifyNot(assign_signal_emitted, "Assign signal emitted by SignalMapper");
        r_result.VerifyNot(remove_signal_emitted, "Remove signal emitted by SignalMapper");
    }
    public void TestEmitAssign(ref TestResult r_result)
    {
        ListModel<string> trigger_model = new ListModel<string>();
        bool insert_signal_emitted = false;
        bool append_signal_emitted = false;
        bool assign_signal_emitted = false;
        bool remove_signal_emitted = false;
        LogListModel log_model = new LogListModel(() => { append_signal_emitted = true; }, () => { insert_signal_emitted = true; }, () => { assign_signal_emitted = true; }, () => { remove_signal_emitted = true; });
        ModelMapper<TableId, string, string> mapper = new ModelMapper<TableId, string, string>(trigger_model, log_model, (string source) => { return source; });
        TableId t_id = trigger_model.Append("old value");
        trigger_model.Assign(t_id, "new value");
        r_result.VerifyNot(insert_signal_emitted, "Insert signal emitted by SignalMapper");
        r_result.Verify(append_signal_emitted, "Append signal emitted by SignalMapper");
        r_result.Verify(assign_signal_emitted, "Assign signal emitted by SignalMapper");
        r_result.VerifyNot(remove_signal_emitted, "Remove signal emitted by SignalMapper");
    }
    public void TestEmitRemove(ref TestResult r_result)
    {
        ListModel<string> trigger_model = new ListModel<string>();
        bool insert_signal_emitted = false;
        bool append_signal_emitted = false;
        bool assign_signal_emitted = false;
        bool remove_signal_emitted = false;
        LogListModel log_model = new LogListModel(() => { append_signal_emitted = true; }, () => { insert_signal_emitted = true; }, () => { assign_signal_emitted = true; }, () => { remove_signal_emitted = true; });
        ModelMapper<TableId, string, string> mapper = new ModelMapper<TableId, string, string>(trigger_model, log_model, (string source) => { return source; });
        TableId t_id = trigger_model.Append("value");
        trigger_model.Remove(t_id);
        r_result.VerifyNot(insert_signal_emitted, "Insert signal emitted by SignalMapper");
        r_result.Verify(append_signal_emitted, "Append signal emitted by SignalMapper");
        r_result.VerifyNot(assign_signal_emitted, "Assign signal emitted by SignalMapper");
        r_result.Verify(remove_signal_emitted, "Remove signal emitted by SignalMapper");
    }

    public override void Cleanup()
    {
    }

    public override void CleanupTest()
    {
    }

    public override void Init()
    {
        this.addTest(this.TestListModelAppend, "Check if element is appended.");
        this.addTest(this.TestListModelInsert, "Check if element is inserted.");
        this.addTest(this.TestListModelAssign, "Check if new value is assinged some element.");
        this.addTest(this.TestListModelRemove, "Check if element is removed.");
        this.addTest(this.TestListModelValue, "Check if value matchs appended value.");
        this.addTest(this.TestListModelPosition, "Check getting right value using position is correct");
        this.addTest(this.TestEmitAppend, "Check if append signal emitted when ListModel changed.");
        this.addTest(this.TestEmitAssign, "Check if assign signal emitted when ListModel changed.");
        this.addTest(this.TestEmitInsert, "Check if insert signal emitted when ListModel changed.");
        this.addTest(this.TestEmitRemove, "Check if remove signal emitted when ListModel changed.");
    }

    public override void Prepare()
    {
    }

    public override void PrepareTest()
    {
    }
}
