using Newtonsoft.Json;

namespace ProjetConsole
{
    public sealed class Traduction
    {
        public Traduction()
        {
            _Language = JsonConvert.DeserializeObject<JsonTraduction>(File.ReadAllText(@"language.json"));
        }
        public JsonLangueContent Langue
        {
            get
            {
                if (_langue == langueEnum.french)
                {
                    return _Language.french;
                }
                else if (_langue == langueEnum.spanish)
                {
                    return _Language.spanish;
                }
                else
                {
                    return _Language.english;
                }
            }
        }


        private JsonTraduction _Language;
        private static Traduction? _instance;
        private static langueEnum _langue;
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
