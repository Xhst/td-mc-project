using System.Collections.Generic;

using Godot;

using TowerDefenseMC.Levels;
using TowerDefenseMC.Singletons;


namespace TowerDefenseMC.UserInterface.Shop
{
    public class ShopInterface : Control
    {
        private bool _isTowerBuilt = true;

        private HBoxContainer _hBoxContainer;
        private Player _player;

        public override void _Ready()
        {
            _hBoxContainer = GetNode<HBoxContainer>("ReferenceRect/HBoxContainer");
        }

        public override void _PhysicsProcess(float delta)
        {
            if(!_isTowerBuilt && !_player.CrystalsIncreased) return;
            
            foreach (ShopItem shopItem in _hBoxContainer.GetChildren())
            {
                shopItem.SetButtonDisabled(_player.Crystals < shopItem.Cost);
            }

            _isTowerBuilt = false;
            _player.CrystalsIncreased = false;
        }

        public void SetPlayer(Player player)
        {
            _player = player;
        }

        public void TowerBuilt()
        {
            _isTowerBuilt = true;
        }

        public void LoadButtons(Dictionary<string, TowerData> towers)
        {
            PackedScene shopItemScene = ResourceLoader.Load<PackedScene>("scenes/ui/level/shop/ShopItem.tscn");
            
            foreach (TowerData tower in towers.Values)
            {
                ShopItem shopItem = (ShopItem) shopItemScene.Instance();
                _hBoxContainer.AddChild(shopItem);
                
                shopItem.UpdateData(tower.ButtonImage, tower.Cost, tower.Name);
            }
        }
    }
}