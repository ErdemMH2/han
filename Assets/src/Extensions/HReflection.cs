using System;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace HAN.Lib.Extension.Class
{
    public static class HReflection
    {
        /**
         * Finds public non public fields of a object.
         * Used to return private members.
         * <param name="a_obj">Object to retrieve member</param>
         * <param name="a_name">name of member</param>
         * <typeparam name="T">Type of field</typeparam>
         */
        public static T GetFieldValue<T>( object a_obj, string a_name )
        {
            // Set the flags so that private and public fields from instances will be found
            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var field = a_obj.GetType().GetField( a_name, bindingFlags );
            return (T) field?.GetValue( a_obj );
        }


        /**
         * Finds non public fields of a object (base class) and tries to set it
         * Used to set private members.
         * <param name="a_obj">Object to retrieve member</param>
         * <param name="a_field">name of member</param>
         * <param name="a_value">value to set</param>
         * <typeparam name="T">Type of field</typeparam>
         */
        public static void SetNonPublicField<T>( object a_obj, string a_field, object a_value )
        {
            var type = a_obj.GetType();
            while( type != typeof(T) )
            {
                type = type.BaseType;
                if( type == typeof( object ) )
                    return;
            }

            type.GetField( a_field, BindingFlags.NonPublic | BindingFlags.Instance )
                .SetValue( a_obj, a_value );
        }
    }
}