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
        private ColorRect _shopInterface;
        private Player _player;

        private Vector2 _mousePos;
        private Rect2 _rect;

        [Signal]
        private delegate void MouseEntered();

        [Signal]
        private delegate void MouseExited();

        public override void _Ready()
        {
            _hBoxContainer = GetNode<HBoxContainer>("ReferenceRect/HBoxContainer");
            _shopInterface = GetNode<ColorRect>("ReferenceRect/ColorRect");

            Connect(nameof(MouseEntered), Scenes.MainScene.GetActiveScene(), nameof(LevelTemplate.OnShopMouseEntered));
            Connect(nameof(MouseExited), Scenes.MainScene.GetActiveScene(), nameof(LevelTemplate.OnShopMouseExited));
        }

        public override void _PhysicsProcess(float delta)
        {
            _mousePos = _shopInterface.GetLocalMousePosition();
            _rect = new Rect2(_shopInterface.RectPosition, _shopInterface.RectSize);
            
            ControlMousePosition();

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

        private void ControlMousePosition()
        {
            if(_rect.HasPoint(_mousePos))
            {
                EmitSignal(nameof(MouseEntered));
            }
            else
            {
                EmitSignal(nameof(MouseExited));
            }
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