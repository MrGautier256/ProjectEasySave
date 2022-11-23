using Newtonsoft.Json;
using ProjetConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesJson
{
    public class JsonTraduction
    {
        public JsonLangueContent? french;
        public JsonLangueContent? english;
    }
    public class JsonLangueContent
    {
        public string? SourcePathInvalid;
        public string? TargetPathInvalid;
        public string? EnterSourcePath;
        public string? EnterTargetPath;

    }
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
