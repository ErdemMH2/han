using HAN.Lib.Basic;
using HAN.Lib.Structure;

namespace HAN.Lib.ProgressSystem
{
    /// Controls the Count of one particular Stock
    /// Will signalize changes events when the Count was changed
    public class Stock : HObject
    {
        public override MetaType Type() { return new MetaType( "Acquired" ); }

        public Key Id { get; private set; }
        public Key TypeKey { get; private set; }

        public int Count { get { return m_count.Value; } 
                           protected set { m_count.SetValue( value );  } }
        private Property<int> m_count;


        public Stock( Key a_id, Key a_type, int a_count )
        {
            m_count = new SignalizingProperty<int>( Lib.Basic.Keys.Signal.CountChanged
                                                   , a_count, this );
            AddProperty( m_count );
            Id = a_id;
            TypeKey = a_type;
        }
    }
}