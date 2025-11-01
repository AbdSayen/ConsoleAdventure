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
        public UIGroup(Point screenPosition, Point size = new(), Anchor anchor = Anchor.Center, int zOrder = 0) : base(screenPosition, size, anchor, zOrder)
        {
        }

        public override UIElement AddElement(UIElement child)
        {
            child.SetParent(this);
            childs.Add(child);
            elements = new ObservableCollection<UIElement>(childs.OrderBy(c => c.GetZOrder()).ToList());
            return child;
        }

        public override UIElement RemoveElement(UIElement child)
        {
            child.SetParent(null);
            childs.Remove(child);
            elements = new ObservableCollection<UIElement>(childs.OrderBy(c => c.GetZOrder()).ToList());
            return child;
        }

        public override List<UIElement> ClearChilds()
        {
            List<UIElement> list = elements.ToList();
            elements.Clear();
            childs.Clear();
            return list;
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
