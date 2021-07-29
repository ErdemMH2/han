using System;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace HAN.Lib.Extension.Class
{
    public static class HEnum
    {
        /**
         * Returns a I enumerable of an enumeration.
         */
        public static IEnumerable<T> GetValues<T>() where T : struct
        {
            return Enum.GetValues( typeof( T ) ).Cast<T>();
        }


        /**
         * If a DescriptionAttribute in a enumeration is defined. This will return it as a string.
         * <param name="enumerationValue">Value of the enumeration with a DescriptionAttribute</param>
         */
        public static string GetDescription<T>( T enumerationValue )
            where T : struct
        {
            Type type = enumerationValue.GetType();
            if( !type.IsEnum )
            {
                throw new ArgumentException( "EnumerationValue must be of Enum type", "enumerationValue" );
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember( enumerationValue.ToString() );
            if( memberInfo != null && memberInfo.Length > 0 )
            {
                object[] attrs = memberInfo[0].GetCustomAttributes( typeof( DescriptionAttribute ), false );

                if( attrs != null && attrs.Length > 0 )
                {
                    //Pull out the description value
                    return ( (DescriptionAttribute) attrs[0] ).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }


        /**
         * Returns all descriptions avaible as a DescriptionAttribute on a enumeration
         */
        public static List<string> GetAllDescriptions<T>() where T : struct
        {
            List<string> enumStrings = new List<string>();
            IEnumerable<T> values = GetValues<T>();
            foreach( var value in values ) {
                enumStrings.Add( GetDescription<T>( value ) );
            }

            return enumStrings;
        }


        /**
         * Returns the first element of a IEnumerator<T>
         * <param name="a_enumerator">Get first element of this IEnumerator</param>
         */
        public static T GetFirstElement<T>( IEnumerator<T> a_enumerator )
        {
            if( a_enumerator != null )
            {
                if( a_enumerator.MoveNext() )
                {
                    return a_enumerator.Current;
                }

            }

            return default(T);
        }
    }
}