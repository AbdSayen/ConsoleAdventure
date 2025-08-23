using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI.System.Containers
{
    public class HListContainer : UIElement
    {
        private List<UIElement> elements = new List<UIElement>();
        private int spacing;

        public HListContainer(Point screenPosition, int spacing, Point size = new(), Point margin = new(), Anchor anchor = Anchor.Center, int zOrder = 0) : base(screenPosition, size, margin, anchor, zOrder)
        {
            this.spacing = spacing;
        }

        public void AddElement(UIElement element)
        {
            element.parent = this;
            elements.Add(element);
        }

        public void RemoveElement(UIElement element)
        {
            element.parent = null;
            elements.Remove(element);
        }

        public override void Update()
        {
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].IsVisible())
                {
                    elements[i].Update();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            List<UIElement> elementsForRender = elements.FindAll((UIElement el) => { return el.IsVisible(); });
            for (int i = 0; i < elementsForRender.Count; i++)
            {
                elementsForRender[i].size.X = size.X;
                elementsForRender[i].screenPosition = screenPosition;
                elementsForRender[i].screenPosition.Y += spacing * i;
                elementsForRender[i].Draw(spriteBatch, elementsForRender[i].CalculateDrawPosition() + CalculateDrawPosition());
            }
        }
    }
}
