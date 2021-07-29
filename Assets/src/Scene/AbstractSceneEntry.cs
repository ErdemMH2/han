using HAN.Lib.Basic;
using HAN.Lib.Structure;

namespace HAN.Lib.Scene
{
    /**
     * Entry point for Scene
     * Can be added to the SceneStack 
     * Has to implement standard behavior for Load, Unload, Show and Hide
     * Every initialization should be done via this class (no Awake, Start!)
     */
    public abstract class AbstractSceneEntry : HObjectMonoBehavior
    {
        public override IHANObject HObject { get { return m_hObject; } }
        protected DefaultHObject m_hObject = new DefaultHObject();

        private Signal m_poppedSignal;

        private void Awake()
        {
            m_poppedSignal = new Signal( this );
            m_hObject.AddSignal( Keys.Scene.Popped, m_poppedSignal );
        }

        /// unique Id, which is also the scene name in Unity
        public abstract Key Id { get; }

        /// can be implemented to initialize everything inside this scene
        /// will only be called one time, when the scene is loaded
        public virtual ICompleteable Load()
        {
            return new CompletedCompleteable();
        }

        /// can be implemented to unload the scene content
        public abstract void Unload();

        /// can be implemented to deactivate scene elements
        public abstract void Hide();

        /// can be implemented to activate scene elements
        public abstract void Show();

        /// will be called when the scene ist removed from the stack
        public void Popped()
        {
            m_poppedSignal.Emit();
        }
    }
}
