using HAN.Lib.Structure;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HAN.Lib.Basic
{
    /**
     * A property is a key value pair, that can identify variables, which can be set and retrievend
     * with generic methods
     */
    public interface IProperty :  IHANType
    {
        /**
         * The id of property, used to indentify this property
         */
         Key Id { get; }


        /**
          * will return type of value
         */
        System.Type GetValueType();


        /**
         * Returns value as object
         */
        object ValueObject { get; }


        /**
        * Do not use without any good reason.
        * Tries to set the value if same type, bypasses all the overrides of TrySet. 
        */
        bool RawTrySetValue( object p_val );


        /**
         * Tries to set the value, if same type
         */
        bool TrySetValue( object p_val );


        /**
         * Tries to copy the property, if the encapsulated type supports 
         * this by having the attribute [Serializable]. The reference to the copy is returned.
         */
        IProperty CopySerialized();


        /**
         * Will return a converted property of possible, else null. 
         */
        V Convert<V>() where V : class, IProperty;
    }


    /**
     * A property is a key value pair, that can identify variables, which can be set and retrievend
     * with generic methods
     */
    [System.Serializable]
    public class Property<T> : IProperty
    {
        public static readonly Key k_Property = KeyFactory.Create( "Property" );
        public virtual MetaType Type() { return new MetaType( k_Property ); }

        /**
         * The id of property, used to indentify this property
         */
        public Key Id { get; private set; }

        /**
         * Value of poperty
         */
        public virtual T Value { get; private set; }

        public object ValueObject { get { return Value; } }

        public System.Type GetValueType() { return typeof( T ); }


        /**
         * Creates a abstract property
         */
        public Property( Key a_id, T a_val )
        {
            Id = a_id;
            Value = a_val;
        }


        public bool RawTrySetValue( object a_val )
        {
            if( a_val.GetType() == GetValueType() )
            {
                Value = (T) a_val;
                return true;
            }
            
            return false;
        }


        public virtual bool TrySetValue( object a_val )
        {
            if( a_val.GetType() == GetValueType() )
            {
                Value = (T) a_val;
                return true;
            }

            return false;
        }


        public virtual void SetValue( T a_val )
        {
            Value = a_val;
        }


        public virtual IProperty CopySerialized()
        {
            object propertyResult = null;
            using( MemoryStream ms = new MemoryStream() )
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize( ms, this );

                ms.Position = 0;
                propertyResult = bf.Deserialize( ms );
            }

            return propertyResult as IProperty;
        }


        public virtual V Convert<V>() where V : class, IProperty
        {
            return PropertyConverter.ChangeType<V, T>( this );
        }


        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
//* will return type of valuex
