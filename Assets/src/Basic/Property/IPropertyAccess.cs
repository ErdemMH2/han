using HAN.Lib.Structure;


namespace HAN.Lib.Basic
{ 
    /**
     * Access interface for using properties
     */
    public interface IPropertyAccess 
    {
        bool HasProperty( Key a_key );

        IProperty Property( Key a_key );

        Property<T> Property<T>( Key a_key );

        bool SetProperty< T >( Key a_key, T a_value );

        bool TrySetProperty( Key a_key, object a_value );

        bool AddProperty( IProperty a_prop );

        bool RemoveProperty( Key a_key );
    }
}