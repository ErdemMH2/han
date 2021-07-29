using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace HAN.Lib.Serialization
{
    //provides serialize / deserialize methods to object.
    //src: http://stackoverflow.com/questions/1446547/how-to-convert-an-object-to-a-byte-array-in-c-sharp
    //NOTE: You need add [Serializable] attribute in your class to enable serialization
    public static class ObjectSerialization
    {

        public static byte[] SerializeToByteArray( object obj )
        {
            if( obj == null )
            {
                return null;
            }
            var bf = new BinaryFormatter();
            using( var ms = new MemoryStream() )
            {
                bf.Serialize( ms, obj );
                return ms.ToArray();
            }
        }

        public static object Deserialize( byte[] byteArray )
        {
            if( byteArray == null )
            {
                return null;
            }
            using( var memStream = new MemoryStream() )
            {
                var binForm = new BinaryFormatter();
                memStream.Write( byteArray, 0, byteArray.Length );
                memStream.Seek( 0, SeekOrigin.Begin );
                var obj = binForm.Deserialize( memStream );
                return obj;
            }
        }


        public static T Deserialize<T>( byte[] a_data ) where T : class
        {
            object obj = Deserialize( a_data );
            if( obj != null
             && obj.GetType() == typeof(T) )
            {
                return (T) obj;
            }

            return null;
        }
    }
}