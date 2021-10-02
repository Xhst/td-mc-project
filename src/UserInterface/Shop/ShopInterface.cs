using System.Collections.Generic;

using Godot;

using TowerDefenseMC.Singletons;


namespace TowerDefenseMC.UserInterface.Shop
{
    public class ShopInterface : Control
    {
        private HBoxContainer _hBoxContainer;

        public override void _Ready()
        {
            _hBoxContainer = GetNode<HBoxContainer>("ReferenceRect/HBoxContainer");
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