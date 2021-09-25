using Godot;

using TowerDefenseMC.Singletons;
using TowerDefenseMC.Transitions;


namespace TowerDefenseMC
{
    public class Main : Node
    {

        private bool _initialFadeActive = true;

        public Transition Transition;
        public Node ActiveSceneContainer;

        public override void _Ready()
        {
            Scenes.MainScene = this;
            
            ActiveSceneContainer = GetNode("ActiveSceneContainer");
            Transition = GetNode<Transition>("Transition");

            Node activeScene = GetActiveScene();
            activeScene.SetProcessInput(false);
            activeScene.SetProcessUnhandledInput(false);

            if (_initialFadeActive)
            {
                InitialTransition();
            }
        }

        public override void _Input(InputEvent @event)
        {
            if (Transition.IsPlaying())
            {
                GetTree().SetInputAsHandled();
            }
        }

        private async void InitialTransition()
        {
            Transition.SetBlack();
            await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
            Transition.FadeOut();
        }

        public Node GetActiveScene()
        {
            return ActiveSceneContainer.GetChild(0);
        }
    }
}