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
                if (_langue == langueEnum.french)
                {
                    return language.french;
                }
                else if (_langue == langueEnum.spanish)
                {
                    return language.spanish;
                }
                else
                {
                    return language.english;
                }
            }
        }


        private JsonTraduction language;
        private static Traduction? _instance;
        private static langueEnum _langue;
        /// <summary>
        /// Attribution de la langue choisie dans la fonction askLanguage
        /// Assignment of the language chosen in the askLanguage fonction
        /// </summary>
        /// <param name="langue"></param>

        public void SetInterfaceLanguage(string langue)
        {
            setLanguage(convertLanguage(langue)); 
        }   
        public langueEnum convertLanguage(string langue)
        {
            if (langue == "francais") { return langueEnum.french; }
            else if (langue == "español") { return langueEnum.spanish; }
            else { return langueEnum.english; }

        }
        public void setLanguage(langueEnum langue)
        {
            _langue = langue;
        }
        public static Traduction Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Traduction();
                }
                return _instance;
            }
        }
    }
}
