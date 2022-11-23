using System;
namespace P2WorkshopP2
{
    public interface IController
    {

    }
    public class Controller : IController
    {
        private Model model;
        private View view;
        public Controller()
        {
            model = new Model();
            view = new View();
        }

        public void begin()
        {
            view.AskInputUser();
            string text = view.getText();
            if (!lengthChecking(text))
            {
                view.updateView("Message trop long ou vide");
            }
            else
            {
                model.SetstringToConvert(text);
                view.updateView(model.convertToUpperCase());
            }
        }

        private bool lengthChecking(string text)
        {
            bool state;
            if (text.Length >= 8 || text == "")
            {
                state = false; 
            }
            else
            {
                state = true;
            }
            return state;
        }
    }
}