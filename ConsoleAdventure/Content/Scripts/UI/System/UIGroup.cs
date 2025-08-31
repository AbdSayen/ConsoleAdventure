using ConsoleAdventure.Content.Scripts.UI.System.Containers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ConsoleAdventure.Content.Scripts.UI.System
{
    public class UIGroup : UIContainer
    {
        private List<UIElement> childs = new List<UIElement>();
        public UIGroup(Point screenPosition, Point size = new(), Point margin = new(), Anchor anchor = Anchor.Center, int zOrder = 0) : base(screenPosition, size, margin, anchor, zOrder)
        {
        }

        public override UIElement AddElement(UIElement child)
        {
            child.parent = this;
            childs.Add(child);
            elements = new ObservableCollection<UIElement>(childs.OrderBy(c => c.GetZOrder()).ToList());
            return child;
        }

        public override UIElement RemoveElement(UIElement child)
        {
            child.parent = null;
            childs.Remove(child);
            elements = new ObservableCollection<UIElement>(childs.OrderBy(c => c.GetZOrder()).ToList());
            return child;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].IsVisible())
                {
                    elements[i].Draw(spriteBatch, elements[i].CalculateDrawPosition() + CalculateDrawPosition());
                }
            }
        }
    }
}
