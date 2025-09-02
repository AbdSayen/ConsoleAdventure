using ConsoleAdventure.Content.Scripts.InputLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI.System.Containers
{
    public class HListContainer : UIContainer
    {
        private int limit = Int32.MaxValue;
        private bool offsetMode = false;

        public HListContainer(Point screenPosition, Point size = new(), Anchor anchor = Anchor.Center, int zOrder = 0, bool offsetMode = false, int limit = Int32.MaxValue) : base(screenPosition, size, anchor, zOrder)
        {
            this.offsetMode = offsetMode;
            this.limit = limit;
        }

        public override bool IsFocusable()
        {
            return true;
        }

        public override void OnFocus()
        {
            isFocused = true;
            if (focusableElements.Count > 0)
                focusableElements[currentlySelected].OnFocus();
        }

        public override void OnDefocus()
        {
            isFocused = false;
            if (focusableElements.Count > 0)
                focusableElements[currentlySelected].OnDefocus();
        }

        public override void Update()
        {
            base.Update();

            if (!isFocused) return;
            if (Input.PostClick(InputConfig.NavigationDown))
            {
                if (focusableElements.Count > 0)
                {
                    focusableElements[currentlySelected].OnDefocus();
                    if (currentlySelected++ >= focusableElements.Count - 1)
                        currentlySelected = 0;
                    focusableElements[currentlySelected].OnFocus();
                }
            }

            if (Input.PostClick(InputConfig.NavigationUp))
            {
                if (focusableElements.Count > 0)
                {
                    focusableElements[currentlySelected].OnDefocus();
                    if (currentlySelected-- <= 0)
                        currentlySelected = focusableElements.Count - 1;
                    focusableElements[currentlySelected].OnFocus();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            List<UIElement> elementsForRender = elements.ToList().FindAll((UIElement el) => { return el.IsVisible(); });
            int offset = 0;
            for (int n = 0; n < limit; n++)
            {
                int i = currentlySelected + n - (limit - 1);
                if (currentlySelected < limit)
                    i = n;
                if (i > elementsForRender.Count - 1) break;
                if (elementsForRender.Count <= 1 && !offsetMode) offset += size.Y / 2;
                elementsForRender[i].screenPosition = drawPosition.ToPoint();
                elementsForRender[i].screenPosition.Y += offset;
                elementsForRender[i].Draw(spriteBatch, elementsForRender[i].ApplyAnchor(elementsForRender[i].screenPosition.ToVector2()));
                if (elementsForRender.Count > 1)
                {
                    if (!offsetMode)
                        offset += size.Y / (limit - 1);
                    else
                        offset += size.Y + elementsForRender[i].size.Y;
                }
            }
        }
    }
}
