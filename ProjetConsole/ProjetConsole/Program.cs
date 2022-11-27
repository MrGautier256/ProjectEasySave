using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ProjetConsole
{
    public class Program
    {
        static void Main()
        {
            // Instanciation d'un controller et lancement de la fonction "execute"
            IController controller = new Controller();
            controller.execute();
            Console.ReadKey();
        }
    }
}