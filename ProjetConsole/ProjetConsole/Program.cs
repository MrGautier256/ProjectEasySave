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
        /// <summary>
        /// Instanciation d'un controller et lancement de la fonction "execute"
        /// Instantiation of a controller and launch of the function 'execute"
        /// </summary>
        static void Main()
        {   
            
            IController controller = new Controller();
            controller.execute();
            Console.ReadKey();
        }
    }
}