using System.Collections.Generic;
using System.Linq;

using Godot;

using Newtonsoft.Json.Linq;


namespace TowerDefenseMC.Singletons
{
    public class LanguageManager : Node
    {
        public static string Language
        {
            get
            {
                if (!ProjectSettings.HasSetting("lang"))
                {
                    ProjectSettings.SetSetting("lang", "en");
                }
                
                return (string) ProjectSettings.GetSetting("lang");
            }
            set => ProjectSettings.SetSetting("lang", value);
        }

        /// <summary>
        /// Get the text from language file
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static string UI(string tags)
        {
            if (tags == null) return "";
            List<string> locations = tags.Split('.').ToList();
            string tag = locations.Last();
            locations.Remove(tag);

            return UI(tag, locations.ToArray());
        }

        /// <summary>
        /// Get the text from language file using the given locations and the base tag
        /// </summary>
        /// <param name="baseTag"></param>
        /// <param name="locations"></param>
        /// <returns></returns>
        private static string UI(string baseTag, params string[] locations)
        {
            var path = ProjectSettings.GlobalizePath($"res://assets/languages/{Language}.json");
            string jsonFileText = System.IO.File.ReadAllText(path);
            
            JObject json = JObject.Parse(jsonFileText);
            
            JToken token = json;
            
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
    }
}