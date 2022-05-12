using Godot;

using TowerDefenseMC.Transitions;


namespace TowerDefenseMC.Singletons
{
    public class Scenes : Node
    {
        public static Main MainScene;
        
        private static uint MinLoadTime = 400;
        
        
        /// <summary>
        /// Change current scene with the scene from the given path.
        /// </summary>
        /// <param name="scenePath">Scene path</param>
        /// <param name="args">Scene PreStart method arguments</param>
        public async void ChangeScene(string scenePath, params object[] args)
        {
            if (MainScene == null) return;

            Node currentScene = MainScene.ActiveSceneContainer.GetChild(0);
            Transition transition = MainScene.Transition;

            //Prevent inputs during scene change
            GetTree().Paused = true;

            transition.FadeIn();
            await ToSignal(transition.AnimationPlayer, "animation_finished");
            
            ulong loadingStartTime = OS.GetTicksMsec();
            
            PackedScene newScene = GD.Load<PackedScene>(scenePath);
            MainScene.ActiveSceneContainer.RemoveChild(currentScene);
            currentScene.QueueFree();
            Node newSceneInstance = newScene.Instance();
            
            MainScene.ActiveSceneContainer.AddChild(newSceneInstance);

            ulong loadTime = OS.GetTicksMsec() - loadingStartTime;
            GD.Print($"Scene { scenePath } loaded in { loadTime } ms.");

            if (loadTime < MinLoadTime)
            {
                await ToSignal(GetTree().CreateTimer((MinLoadTime - loadTime) / 1000f), "timeout");
            }
            
            transition.FadeOut();

            if (newSceneInstance.HasMethod("PreStart"))
            {
                newSceneInstance.GetType().GetMethod("PreStart")?.Invoke(newSceneInstance, args);
            }

            GetTree().Paused = false;
            
            if (newSceneInstance.HasMethod("Start"))
            {
                newSceneInstance.GetType().GetMethod("Start")?.Invoke(newSceneInstance, new object[]{ });
            }
        }
    }
}