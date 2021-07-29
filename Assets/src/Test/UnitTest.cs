namespace HAN.Lib.Test
{
    /**
     * Parent class for all unit tests
     * To write a unit test this class has to be derived and test methods have to be members of this child class.
     * The test methods have to be added to the test by using the addTest function. This step has to be done by 
     * overriding in Init().
     */
    public abstract class UnitTest : Test
    {
        public UnitTest()
          : base()
        {
        }
    }
}