using Godot;


namespace TowerDefenseMC.Transitions
{
    public class Transition : CanvasLayer
    {
        public AnimationPlayer AnimationPlayer;
        private ColorRect _colorRect;

        public override void _Ready()
        {
            AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            _colorRect = GetNode<ColorRect>("ColorRect");
        }

        public bool IsPlaying()
        {
            return AnimationPlayer.IsPlaying() || _colorRect.Color.a != 0;
        }

        public void SetBlack()
        {
            AnimationPlayer.Play("black");
        }

        public void FadeIn()
        {
            AnimationPlayer.Play("fade-to-black");
        }

        public void FadeOut()
        {
            AnimationPlayer.Play("fade-from-black");
        }

    }
}