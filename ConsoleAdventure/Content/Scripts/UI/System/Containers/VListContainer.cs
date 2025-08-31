using ConsoleAdventure.Content.Scripts.InputLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI.System.Containers
{
    public class VListContainer : UIContainer
    {
        private int limit = Int32.MaxValue;
        private bool offsetMode = false;

        public VListContainer(Point screenPosition, Point size = new(), Point margin = new(), Anchor anchor = Anchor.Center, int zOrder = 0, bool offsetMode = false, int limit = Int32.MaxValue) : base(screenPosition, size, margin, anchor, zOrder)
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
            if (!Input.IsKeyDown(InputConfig.NavigationRight) && Input.IsOldKeyDown(InputConfig.NavigationRight))
            {
                if (focusableElements.Count > 0)
                {
                    focusableElements[currentlySelected].OnDefocus();
                    if (currentlySelected++ >= focusableElements.Count - 1)
                        currentlySelected = 0;
                    focusableElements[currentlySelected].OnFocus();
                }
            }

            if (!Input.IsKeyDown(InputConfig.NavigationLeft) && Input.IsOldKeyDown(InputConfig.NavigationLeft))
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
            for (int n = 0; n < elementsForRender.Count; n++)
            {
                int i = currentlySelected + n - (limit - 1);
                if (currentlySelected < limit)
                    i = n;
                if (i > elementsForRender.Count - 1) break;
                if (elementsForRender.Count <= 1 && !offsetMode) offset += size.X / 2;
                elementsForRender[i].screenPosition = drawPosition.ToPoint();
                elementsForRender[i].screenPosition.X += offset;
                elementsForRender[i].Draw(spriteBatch, elementsForRender[i].ApplyAnchor(elementsForRender[i].screenPosition.ToVector2()));
                if (!offsetMode)
                    offset += size.X / (elementsForRender.Count - 1);
                else
                    offset += size.X + elementsForRender[i].size.X;
            }
        }
    }
}