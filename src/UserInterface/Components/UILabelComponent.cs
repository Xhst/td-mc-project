using Godot;

using TowerDefenseMC.Singletons;
using TowerDefenseMC.Utils;


namespace TowerDefenseMC.UserInterface.Components
{
    [Tool]
    public class UILabelComponent : Label, IObserver
    {

        private LanguageManager _languageManager;
        
        private string _tag;
        
        [Export]
        protected string Tag
        {
            get => _tag;
            set
            {
                _tag = value;
                Update();
            }
        }

        public override void _Ready()
        {
            if (!Engine.EditorHint)
            {
                _languageManager = GetNode<LanguageManager>("/root/LanguageManager");
            }
			
            Update();
        }

        public new void Update()
        {
            if (Engine.EditorHint)
            {
                Text = LanguageManager.UI(LanguageManager.DefaultLanguage, _tag);
                return;
            }
            Text = _languageManager.UI(_tag);
        }
    }
}