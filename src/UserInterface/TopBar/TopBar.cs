using Godot;

using TowerDefenseMC.Singletons;

namespace TowerDefenseMC.UserInterface.TopBar
{
    public class TopBar : Control
    {
        public Crystals Crystals;
        public HealthBar HealthBar;

        public override void _Ready()
        {
            Crystals = GetNode<Crystals>("ColorRect/HBoxContainer/Crystal");
            HealthBar = GetNode<HealthBar>("ColorRect/HBoxContainer/HealthBar");
        }
    }
}
