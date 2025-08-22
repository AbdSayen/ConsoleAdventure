using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleAdventure.Content.Scripts.UI.System
{
    public class UIGroup : UIElement
    {
        private List<UIElement> childs = new List<UIElement>();
        List<UIElement> orderedChilds;
        public UIGroup(Point screenPosition, Point size, Point margin = new(), Anchor anchor = Anchor.Center, int zOrder = 0) : base(screenPosition, size, margin, anchor, zOrder)
        {
        }

        public void AddChild(UIElement child)
        {
            childs.Add(child);
            orderedChilds = childs.OrderBy(c => c.GetZOrder()).ToList();
        }

        public void RemoveChild(UIElement child)
        {
            childs.Remove(child);
            orderedChilds = childs.OrderBy(c => c.GetZOrder()).ToList();
        }

        public override void Update()
        {
            for (int i = 0; i < orderedChilds.Count; i++)
            {
                if (orderedChilds[i].IsVisible())
                {
                    orderedChilds[i].Update();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Point drawPosition)
        {
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
