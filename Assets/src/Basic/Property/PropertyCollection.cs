using HAN.Lib.Structure;
using System.Collections;
using System.Collections.Generic;

namespace HAN.Lib.Basic
{ 
    public class PropertyCollection : IPropertyAccess, IHANType, IEnumerable<IProperty>
    {
        public static readonly Key k_PropertyCollection= KeyFactory.Create( "PropertyCollection" );
        public MetaType Type() { return new MetaType( k_PropertyCollection ); }


        /// all properties that are owned by this HObject
        protected Dictionary< Key, IProperty > m_properties = new Dictionary<Key, IProperty>();

        /// Count of all entries
        public int Count { get { return m_properties.Count; } }


        public PropertyCollection()
        {
        }


        public PropertyCollection( IEnumerable<IProperty> a_properties )
        {
            foreach( var property in a_properties )
            {
                AddProperty( property );
            }
        }

        /**
         *  adds a property to our object. Will be owned by the object.
         *  If it is a ISignalPublisher, the object will organize his signals.
         */
        public virtual bool AddProperty( IProperty a_prop )
        {
            if( !m_properties.ContainsKey( a_prop.Id ) )
            {
                m_properties.Add( a_prop.Id, a_prop );
                return true;
            }


            HAN.Debug.Logger.Warning( Type().ObjectName,
                            string.Format( "cant add property {0} to {1}; Property already present",
                            a_prop.Id.Name, Type().ObjectName.Name ) );
            return false;
        }


        /**
         *  removes our signal. The object also do not organize the signals anymore.
         */
        public bool RemoveProperty( Key a_key )
        {
            if( m_properties.ContainsKey( a_key ) )
            {
                m_properties.Remove( a_key );
                return true;
            }


            HAN.Debug.Logger.Warning( Type().ObjectName,
                            string.Format( "cant remove property {0} from {1}; Property not found",
                            a_key.Name, Type().ObjectName.Name ) );
            return false;
        }


        /**
         *  Returns true if object has property, else false
         */
        public bool HasProperty( Key a_key )
        {
            return m_properties.ContainsKey( a_key );
        }


        /**
         *  Returns abstract property for the key, if available. Else it will return null.
         */
        public IProperty Property( Key a_key )
        {
            if( m_properties.ContainsKey( a_key ) )
            {
                return m_properties[a_key];
            }


            HAN.Debug.Logger.Warning( Type().ObjectName,
                        string.Format( "cant find property {0} from {1}; Property not found",
                        a_key.Name, Type().ObjectName.Name ) );

            return null;
        }


        /**
         *  Returns casted property for the key, if available. Else it will return null.
         */
        public Property<T> Property<T>( Key a_key )
        {
            return (Property<T>) Property( a_key );
        }


        public T Value<T>( Key a_key, T a_default = default )
        {
            if( Property( a_key ) is Property<T> prop )
            {
                return prop.Value;
            }
            else
            {
                return a_default;
            }
        }


        /**
         *  Set the properties value. If success it will return true,
         *  else false
         */
        public bool SetProperty<T>( Key a_key, T a_value )
        {
            var property = Property<T>( a_key );
            if( property != null )
            {
                property.SetValue( a_value );
                return true;
            }

            return false;
        }


        /**
         *  Try to set the value of the property with the supplied object.
         *  It will try to cast the value to the right type.
         *  Use this function with care!
         */
        public bool TrySetProperty( Key a_key, object a_value )
        {
            var property = Property( a_key );
            if( property != null )
            {
                return property.TrySetValue( a_value );
            }

            return false;
        }


        public IEnumerator<IProperty> GetEnumerator()
        {
            return m_properties.Values.GetEnumerator();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_properties.Values.GetEnumerator();
        }
    }
}