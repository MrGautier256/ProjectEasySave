using System;
namespace P2WorkshopP2
{
    public class Model
    {
        private string stringToConvert;

        public void SetstringToConvert(string input)
        { this.stringToConvert = input; }

        public Model()
        {
            stringToConvert = "";
        }
        public string convertToUpperCase()
        {
            return stringToConvert.ToUpper();
        }

    }
}