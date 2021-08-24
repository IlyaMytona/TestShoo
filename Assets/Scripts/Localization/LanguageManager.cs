using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;


namespace Test.Localization
{
    public class LanguageManager : SingletonMonoBehaviour<LanguageManager>
    {
        private Dictionary<string, string> values;
        public event Action OnButtonSwitch;

        public string LanguageCode { get; private set; }

        public void Init(string file, string languageCode = "")
        {
            values = new Dictionary<string, string>(5);
            if (languageCode == "")
            {
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.Russian:
                        LanguageCode = "Ru";
                        break;
                    default:
                        LanguageCode = "Eng";
                        break;
                }
            }
            if (languageCode == "Ru") LanguageCode = "Ru";
            if (languageCode == "Eng") LanguageCode = "Eng";

            var config = LoadResource(file);
            if (!config) return;
            values = JsonConvert.DeserializeObject<Dictionary<string, string>>(config.text);
            OnButtonSwitch?.Invoke();
        }

        private TextAsset LoadResource(string resourceName)
        {
            return Resources.Load(LocalizeResourceName(resourceName), typeof(TextAsset)) as TextAsset;
        }

        private string LocalizeResourceName(string resourceName)
        {
            return resourceName + LanguageCode;
        }

        public string Text(string key)
        {
            if (values == null) return "[not init]";
            if (values.ContainsKey(key)) return values[key];
            else
            {
                return "[not found]";
            }
        }
    }
}
