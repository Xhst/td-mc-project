using Godot;

using TowerDefenseMC.Singletons;

namespace TowerDefenseMC.UserInterface.TopBar
{
    public class TopBar : Control
    {
        private Label _availableCrystals;

        public override void _Ready()
        {
            _availableCrystals = GetNode<Label>("ColorRect/HBoxContainer/Crystal");
        }

        public override void _PhysicsProcess(float delta)
        {
            if(!Game.EnemyIsDead) return;

            IncreaseAvailableCrystals();

            Game.EnemyIsDead = false;
        }

        public void DecreaseAvailableCrystals(int towerCost)
        {
            _availableCrystals.Text = (_availableCrystals.Text.ToInt() - towerCost).ToString();
        }

        public void IncreaseAvailableCrystals()
        {
            _availableCrystals.Text = (_availableCrystals.Text.ToInt() + 1).ToString();
        }

        public int GetAvailableCrystals()
        {
            return _availableCrystals.Text.ToInt();
        }
    }
}
