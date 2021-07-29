using HAN.Lib.Basic;

namespace HAN.Lib.Scene
{
    /// Return object to signalize the finalization of the load event
    public class SceneLoadState : Completeable
    {
        public AbstractSceneEntry SceneEntry { get; set; }

        public SceneLoadState()
        {
        }
    }
}