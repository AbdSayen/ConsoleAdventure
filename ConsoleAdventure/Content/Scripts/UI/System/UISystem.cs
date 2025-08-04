using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI.System
{
    public static class UISystem
    {
        private static List<UIElement> childs = new List<UIElement>();

        static UISystem()
        {

        }

        public static void AddChild(UIElement child)
        {
            childs.Add(child);
        }

        public static void RemoveChild(UIElement child)
        {
            childs.Remove(child);
        }

        public static void Update()
        {

        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            List<UIElement> orderedChilds = childs.OrderBy(c => c.GetZOrder()).ToList();
            for (int i = 0; i < orderedChilds.Count; i++)
            {
                if (orderedChilds[i].IsVisible())
                {
                    orderedChilds[i].Draw(spriteBatch, orderedChilds[i].CalculateDrawPosition());
                }
            }
        }
    }
}
