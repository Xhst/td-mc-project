using System;
using System.Collections.Generic;
using System.Linq;

using Godot;

using Newtonsoft.Json.Linq;

using TowerDefenseMC.Utils;


namespace TowerDefenseMC.Singletons
{
    public class LanguageManager : Node, ISubject
    {

        public static string DefaultLanguage = "en";

        private readonly Dictionary<string, string> _languageToFileName;
        private readonly HashSet<IObserver> _observers;
        private Persist _persist;

        private static string oit = "{\"lang\":\"Italiano\",\"main_menu\":{\"levels\":\"Livelli\",\"settings\":\"Impostazioni\",\"share\":\"Condividi\",\"back\":\"Torna Al Menu\"},\"top_bar\":{\"wave_timer\":\"Prossima Ondata\"},\"pause_menu\":{\"continue\":\"Continua\",\"back_to_menu\":\"Torna Al Menu\"},\"end_level\":{\"level_completed\":\"Livello Completato\",\"game_over\":\"Hai Perso\",\"back_to_menu\":\"Torna Al Menu\",\"next_level\":\"Prossimo Livello\",\"retry\":\"Ricomincia Il Livello\",\"share\":\"Condividi\"},\"statistics\":{\"damage\":\"Danno\",\"attack_speed\":\"Velocità Di Attacco\",\"attack_range\":\"Raggio Di Attacco\",\"projectile_speed\":\"Velocità Proiettili\",\"aura_damage\":\"EA: Danno\",\"aura_attack_speed\":\"EA: Velocità Di Attacco\"}}";
        private static string oen = "{\"lang\":\"English\",\"main_menu\":{\"levels\":\"Levels\",\"settings\":\"Settings\",\"share\":\"Share\",\"back\":\"Main Menu\"},\"top_bar\":{\"wave_timer\":\"Next Wave\"},\"pause_menu\":{\"continue\":\"Continue\",\"back_to_menu\":\"Back To Menu\"},\"end_level\":{\"level_completed\":\"Level Completed\",\"game_over\":\"You Lost\",\"back_to_menu\":\"Back To Menu\",\"next_level\":\"Next Level\",\"retry\":\"Restart Level\",\"share\":\"Share\"},\"statistics\":{\"damage\":\"Damage\",\"attack_speed\":\"Attack Speed\",\"attack_range\":\"Attack Range\",\"projectile_speed\":\"Projectile Speed\",\"aura_damage\":\"AE: Damage\",\"aura_attack_speed\":\"AE: Attack Speed\"}}";

        private string Language
        {
            get
            {
                if (!ProjectSettings.HasSetting("lang"))
                {
                    ProjectSettings.SetSetting("lang", DefaultLanguage);
                }
                
                return (string) ProjectSettings.GetSetting("lang");
            }
            set
            {
                ProjectSettings.SetSetting("lang", value);
                Notify();
            }
        }

        public LanguageManager()
        {
            _observers = new HashSet<IObserver>();
            _languageToFileName = LanguagesToFileNameDictionary();
        }

        public override void _Ready()
        {
            _persist = GetNode<Persist>("/root/Persist");
        }

        /// <summary>
        /// Get the text from language file
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public string UI(string tags)
        {
            return UI(Language, tags);
        }

        /// <summary>
        /// Get the text from language file
        /// </summary>
        /// <param name="language"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static string UI(string language, string tags)
        {
            if (tags == null) return "";
            List<string> locations = tags.Split('.').ToList();
            string tag = locations.Last();
            locations.Remove(tag);
            
            return UI(language, tag, locations.ToArray());
        }

        /// <summary>
        /// Get the text from language file using the given locations and the base tag
        /// </summary>
        /// <param name="language"></param>
        /// <param name="baseTag"></param>
        /// <param name="locations"></param>
        /// <returns></returns>
        private static string UI(string language, string baseTag, params string[] locations)
        {
            JToken token = GetJsonTokenFromFile(language);
            
            foreach (string location in locations)
            {
                token = token?[location];
            }

            string result = token?[baseTag]?.ToString();

            if (string.IsNullOrEmpty(result))
            {
                result = baseTag;
            }
            
            return result;
        }

        private static JToken GetJsonTokenFromFile(string fileName)
        {
            /*string path = ProjectSettings.GlobalizePath($"res://assets/languages/{ fileName }.json");

            string path;

            if (fileName == "it") {
                path = ProjectSettings.GlobalizePath($"res://assets/languages/it.json");
            } else {
                path = ProjectSettings.GlobalizePath($"res://assets/languages/en.json");
            }

            string jsonFileText = System.IO.File.ReadAllText(path);

            string jsonFileText = "";

            File file = new File();

            Error err = file.Open(path, File.ModeFlags.Read);

            if(err == Error.Ok)
            {
                jsonFileText = file.GetAsText();
            }
            
            file.Close();*/
            
            string jsonFileText;

            if (fileName == "it") {
                jsonFileText = oit;
            } else {
                jsonFileText = oen;
            }

            JObject json = JObject.Parse(jsonFileText);

            return json;
        }

        private static Dictionary<string, string> LanguagesToFileNameDictionary()
        {
            Dictionary<string, string> availableLanguagesAndFileName = new Dictionary<string, string>();

            availableLanguagesAndFileName.Add("English","en");

            availableLanguagesAndFileName.Add("Italiano","it");

            /*try {

                HashSet<string> langFiles = FileHelper.FilesInDirectory("res://assets/languages/");

                foreach (string file in langFiles)
                {
                    string fileName = file.Replace(".json", "");
                    JToken token = GetJsonTokenFromFile(fileName);
                
                    if (token["lang"] == null) continue;
                
                    availableLanguagesAndFileName.Add(token["lang"].ToString(), fileName);
                }
            } catch(Exception _) {

            }*/

            return availableLanguagesAndFileName;
        }
        
        public HashSet<string> GetAvailableLanguages()
        {
            return new HashSet<string>(_languageToFileName.Keys);
        }

        public void SetLanguage(string language)
        {
            if (!_languageToFileName.TryGetValue(language, out string lang))
            {
                throw new Exception("[LanguageManager] Trying to set non existing language.");
            }

            Language = lang;
            _persist.Save();
        }

        public void SetLanguageLoad(string lang)
        {
            Language = lang;
        }

        public string GetLanguage()
        {
            return Language;
        }

        public string GetLanguageText()
        {
            JToken token = GetJsonTokenFromFile(GetLanguage());

            return token["lang"].ToString();
        }

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (IObserver observer in _observers)
            {
                observer.Update();
            }
        }
    }
}