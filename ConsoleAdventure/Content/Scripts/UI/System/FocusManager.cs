using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ConsoleAdventure.Content.Scripts.UI.System
{
    public static class FocusManager
    {
        private static UIElement focusedOn = null;

        public static UIElement GetFocus()
        {
            return focusedOn;
        }

        public static async void SetFocus(UIElement newElement)
        {
            if (focusedOn != null) focusedOn.OnDefocus();

            await Task.Yield(); // Безумно важная задержка, которая чинит проблемы что один клик обрабатывается сразу двумя элементами 
            newElement.OnFocus();
            focusedOn = newElement;
        }
    }
}
