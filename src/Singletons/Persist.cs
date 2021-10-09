using Godot;
using Godot.Collections;


namespace TowerDefenseMC.Singletons
{

    public class Persist : Node
    {
        private const string SaveDirectory = "user://data/saves/";
        private const string SavePath = SaveDirectory + "game.save";

        private LanguageManager _languageManager;
        private Game _game;

        public override void _Ready()
        {
            Directory dir = new Directory();

            if (!dir.DirExists(SaveDirectory))
            {
                dir.MakeDirRecursive(SaveDirectory);
            }

            _languageManager = GetNode<LanguageManager>("/root/LanguageManager");
            _game = GetNode<Game>("/root/Game");
            
            Load();
        }

        public void Save()
        {
            File file = new File();
            Error error = file.Open(SavePath, File.ModeFlags.Write);

            GD.Print(error);
            if (error != Error.Ok) return;

            file.StoreLine(JSON.Print(GetSaveData()));
            file.Close();
            
            GD.Print(JSON.Print(GetSaveData()));
        }

        private Dictionary<string, object> GetSaveData()
        {
            Dictionary<string, object> saveData = new Dictionary<string, object>()
            {
                { "lang", _languageManager.GetLanguage() },
                { "completed_levels", _game.GetCompletedLevelsToGodotDictionary() }
            };

            return saveData;
        }

        public void Load()
        {
            File file = new File();

            if (!file.FileExists(SavePath)) return;
            
            Error error = file.Open(SavePath, File.ModeFlags.Read);
            
            if (error != Error.Ok) return;
            
            string line = file.GetLine();
            JSONParseResult json = JSON.Parse(line);

            if (json.Result is Dictionary saveData)
            {
                if (saveData["lang"] != null)
                {
                    string lang = saveData["lang"] as string;
                    _languageManager.SetLanguageLoad(lang);
                }
                
                if (saveData["completed_levels"] != null)
                {
                    Dictionary lvlDict = saveData["completed_levels"] as Dictionary;

                    _game.LoadLevelsCompleted(lvlDict);
                }
            }
            
            file.Close();
            
        }
    }
}