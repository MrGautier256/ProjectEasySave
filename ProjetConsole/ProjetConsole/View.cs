using System;
using System.Reflection;

namespace P2WorkshopP2
{
    public class View
    {
        private string inputUser;
        public View()
        {
            inputUser = "";
        }
        public void AskInputUser()
        { this.inputUser = Console.ReadLine(); }

        public string getText()
        { return this.inputUser; }
        public void updateView(string upperText)
        {
            Console.WriteLine(upperText);
        }
       
    }

}