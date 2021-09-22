using Godot;

using TowerDefenseMC.Levels;
using TowerDefenseMC.Singletons;


namespace TowerDefenseMC.Shop
{
    public class ShopItem : Control
    {
        private int _cost;

        private string _onButtonDownBind;

        private Sprite _towerImage;
        private RichTextLabel _costText;
        
        [Signal]
        private delegate void ClickEvent(string bind);
        
        [Signal]
        private delegate void MouseEntered();
        
        [Signal]
        private delegate void MouseExited();

        public override void _Ready()
        {
            _towerImage = GetNode<Sprite>("Button/TowerImage");
            _costText = GetNode<RichTextLabel>("Button/Cost/CostText");

            SceneManager sceneManager = GetNode<SceneManager>("/root/SceneManager");
            
            Connect(nameof(ClickEvent), sceneManager.CurrentScene, nameof(LevelTemplate.OnSelectTowerButtonDown));
            Connect(nameof(MouseEntered), sceneManager.CurrentScene, nameof(LevelTemplate.OnTowerButtonMouseEntered)); 
            Connect(nameof(MouseExited), sceneManager.CurrentScene, nameof(LevelTemplate.OnTowerButtonMouseExited)); 

        }

        public void UpdateData(string towerTexturePath, int cost, string onButtonDownBind)
        {
            Texture towerTexture = ResourceLoader.Load<Texture>($"assets/img/shop/{ towerTexturePath }.png");

            _towerImage.Texture = towerTexture;
            

            _cost = cost;
            _onButtonDownBind = onButtonDownBind;
            
            _costText.Text = _cost.ToString();
        }
        
        public void OnButtonDown()
        {
            EmitSignal(nameof(ClickEvent), _onButtonDownBind);
        }

        public void OnMouseEntered()
        {
            EmitSignal(nameof(MouseEntered));
        }
        
        public void OnMouseExited()
        {
            EmitSignal(nameof(MouseExited));
        }

    }
}