using HAN.Lib.Structure;


namespace HAN.Lib.Basic 
{
    /**
     * A IncrementalId is an Id that can be extended to make querries more complex and restrictive.
     * The deriving classes can add more restrictions. This will make the matching more complex. 
     * The matching is done in the base class via Key comparision over the Equal and GetHashCode
     */
    public class IncrementalId : IHANType
    {
        public static readonly Key k_IncrementalId= KeyFactory.Create( "IncrementalId" );
        public virtual MetaType Type() { return new MetaType( k_IncrementalId ); }

        public Key Id { get; private set; }

        public IncrementalId( Key a_id )
        {
            Id = a_id;
        }


        public static implicit operator IncrementalId( Key a_id )
        {
            return new IncrementalId( a_id );
        }


        public static bool operator ==( IncrementalId a_a, IncrementalId a_b )
        {
            if( System.Object.ReferenceEquals( a_a, a_b ) ) {
                return true;
            }

            if( ( (object) a_a == null ) || ( (object) a_b == null ) ) { 
                return false;
            }

            return a_a.Equals( a_b );
        }


        public static bool operator !=( IncrementalId a_a, IncrementalId a_b )
        {
            return !( a_a == a_b );
        }


        public override bool Equals( System.Object a_obj )
        {
            IncrementalId other = a_obj as IncrementalId;
            if( other != null ) {
                return Id == other.Id;
            }

            return false;
        }


        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }


        public override string ToString()
        {
            return Id.Name;
        }
    }
}