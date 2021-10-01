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
        public string Tag
        {
            get => _tag;
            set
            {
                _tag = value;
				
                if (Engine.EditorHint)
                {
                    Text = LanguageManager.UI(LanguageManager.DefaultLanguage, _tag);
                    return;
                }
				
                Update();
            }
        }

        public override void _EnterTree()
        {
            if (Engine.EditorHint) return;
            
            _languageManager = GetNode<LanguageManager>("/root/LanguageManager");
            _languageManager.Attach(this);
        }

        public override void _ExitTree()
        {
            if (Engine.EditorHint) return;
            
            _languageManager.Detach(this);
        }

        public new void Update()
        {
            if (_languageManager == null) return;
			
            Text = _languageManager.UI(_tag);
        }
    }
}