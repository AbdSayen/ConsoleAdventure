using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI
{
    internal class ControlConfig : ListUI
    {
        public ControlConfig(string text, Vector2 position, List<BaseUI> elements, Color color) : base(text, position, elements, color)
        {
        }
    }
}
