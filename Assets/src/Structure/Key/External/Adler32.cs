using UnityEngine;
using System.Collections;

//http://www.yoda.arachsys.com/csharp/miscutil/

namespace HAN.Lib.Structure.External
{
    public class Adler32
    {
        /// <summary>
        /// mc_base for modulo arithmetic
        /// </summary>
        private const int mc_base = 65521;

        /// <summary>
        /// Number of iterations we can safely do before applying the modulo.
        /// </summary>
        private const int mc_nmax = 5552;

        /// <summary>
        /// Initial value or previous result. 1 used for the
        /// first transformation.
        /// </summary>
        static private int s_initial = 1;

        /// <summary>
        /// Encodes string to byte array with Unicode (Net2.0 string standard) 
        /// </summary>
        static private System.Text.Encoding s_textEncoder = System.Text.Encoding.Unicode;

        /// <summary>
        /// Computes the Adler32 checksum for the given p_data.
        /// </summary>
        /// <param name="p_data">The p_data to compute the checksum of</param>
        /// <param name="p_start">Index of first byte to compute checksum for</param>
        /// <param name="p_length">Number of bytes to compute checksum for</param>
        /// <returns>The checksum of the given p_data</returns>
        public static int ComputeChecksum( byte[] p_data, int p_start, int p_length )
        {
            if( p_data == null )
            {
                return 0;
            }

            uint s1 = (uint) ( s_initial & 0xffff );
            uint s2 = (uint) ( ( s_initial >> 16 ) & 0xffff );

            int index = p_start;
            int len = p_length;

            int k;
            while( len > 0 )
            {
                k = ( len < mc_nmax ) ? len : mc_nmax;
                len -= k;

                for( int i = 0; i < k; i++ )
                {
                    s1 += p_data[index++];
                    s2 += s1;
                }

                s1 %= mc_base;
                s2 %= mc_base;
            }

            //s_initial = (int) ( ( s2 << 16 ) | s1 );
            return (int) ( ( s2 << 16 ) | s1 );
        }

        /// <summary>
        /// Computes the Adler32 checksum for the given p_data.
        /// </summary>
        /// <param name="p_data">The p_data to compute the checksum of</param>
        /// <returns>The checksum of the given p_data</returns>
        public static int ComputeChecksum( byte[] p_data )
        {
            return ComputeChecksum( p_data, 0, p_data.Length );
        }

        /// <summary>
        /// Computes the Adler32 checksum for the given p_data.
        /// </summary>
        /// <param name="p_string">The string to compute the checksum of</param>
        /// <returns>The checksum of the given string</returns>
        public static int ComputeChecksum( string p_string )
        {
            return ComputeChecksum( s_textEncoder.GetBytes( p_string.ToCharArray() ) );
        }
    }
}