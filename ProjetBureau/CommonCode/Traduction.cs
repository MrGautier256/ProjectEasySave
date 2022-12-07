using CommonCode;
using Newtonsoft.Json;
using System.IO;

namespace CommonCode
{
    public sealed class Traduction
    {
        /// <summary>
        /// Constructeur lisant le fichier de langue "language.json"
        /// Constructor reading the language file "language.json"
        /// </summary>

        public Traduction()
        {
            language = JsonConvert.DeserializeObject<JsonTraduction>(File.ReadAllText(@"language.json"));
        }

        /// <summary>
        /// Récupération des éléments textuels de JsonLangueContent en fonction de la langue choisie (Anglais par défaut)
        /// Recovery of textual elements from JsonLangueContent according to the selected language (English by default)
        /// </summary>

        public JsonLangueContent Langue
        {
            get
            {
                if (_langue == LangueEnum.french)
                {
                    return language.french;
                }
                else if (_langue == LangueEnum.spanish)
                {
                    return language.spanish;
                }
                else
                {
                    return language.english;
                }
            }
        }


        private readonly JsonTraduction language;
        private static Traduction? _instance;
        private static LangueEnum _langue;
        /// <summary>
        /// Attribution de la langue choisie dans la fonction askLanguage
        /// Assignment of the language chosen in the askLanguage fonction
        /// </summary>
        /// <param name="langue"></param>

        public static void SetInterfaceLanguage(string langue)
        {
            SetLanguage(ConvertLanguage(langue)); 
        }   
        public static LangueEnum ConvertLanguage(string langue)
        {
            if (langue == "francais") { return LangueEnum.french; }
            else if (langue == "español") { return LangueEnum.spanish; }
            else { return LangueEnum.english; }

        }
        public static void SetLanguage(LangueEnum langue)
        {
            _langue = langue;
        }
        public static Traduction Instance
        {
            get
            {
                _instance ??= new Traduction();
                return _instance;
            }
        }
    }
}
