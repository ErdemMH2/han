using System;


namespace HAN.Lib.Extension.Collections
{
    public static class HArray
    {
        public static T[] Extend<T>( this T[] a_x, T a_y ) 
        {
            Array.Resize<T>( ref a_x, a_x.Length + 1 );
            a_x[a_x.Length-1] = a_y; // add last element

            return a_x;
        }


        public static T[] Extend<T>( this T[] a_x, T[] a_y )
        {
            int oldLen = a_x.Length;
            Array.Resize<T>( ref a_x, a_x.Length + a_y.Length );
            Array.Copy( a_y, 0, a_x, oldLen, a_y.Length );

            return a_x;
        }
    }
}