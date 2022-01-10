using Godot;

using TowerDefenseMC.Levels;
using TowerDefenseMC.Singletons;


namespace TowerDefenseMC.UserInterface.Shop
{
    public class ShopItem : Control
    {
        public int Cost { get; private set; }

        private string _onButtonDownBind;

        private readonly Color _buttonDisabled = new Color(1f, 1f, 1f, 0.5f);
        private readonly Color _buttonActive = new Color(1f, 1f, 1f, 1f);

        private Sprite _towerImage;
        private RichTextLabel _costText;
        private TextureButton _towerButton;
        
        [Signal]
        private delegate void ClickEvent(string bind, int cost);

        public override void _Ready()
        {
            _towerButton = GetNode<TextureButton>("Button");
            _towerImage = GetNode<Sprite>("Button/TowerImage");
            _costText = GetNode<RichTextLabel>("Button/Cost/CostText");

            Connect(nameof(ClickEvent), Scenes.MainScene.GetActiveScene(), nameof(LevelTemplate.OnSelectTowerButtonDown));
        }

        public void UpdateData(string towerTexturePath, int cost, string onButtonDownBind)
        {
            Texture towerTexture = ResourceLoader.Load<Texture>($"assets/img/shop/{ towerTexturePath }.png");

            _towerImage.Texture = towerTexture;
            

            Cost = cost;
            _onButtonDownBind = onButtonDownBind;
            
            _costText.Text = Cost.ToString();
        }
        
        public void SetButtonDisabled(bool disabled)
        {
            _towerButton.Disabled = disabled;

            _towerButton.Modulate = _towerButton.Disabled ? _buttonDisabled : _buttonActive;
        }

        public void OnButtonDown()
        {
            EmitSignal(nameof(ClickEvent), _onButtonDownBind, Cost);
        }

    }
}