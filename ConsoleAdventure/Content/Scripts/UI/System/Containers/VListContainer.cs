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
    public class VListContainer : UIContainer
    {
        public VListContainer(Point screenPosition, Point size = new(), Point margin = new(), Anchor anchor = Anchor.Center, int zOrder = 0) : base(screenPosition, size, margin, anchor, zOrder)
        {
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
            for (int i = 0; i < elementsForRender.Count; i++)
            {
                if (elementsForRender.Count <= 1) offset += size.X / 2;
                //elementsForRender[i].size.Y = size.Y;
                elementsForRender[i].screenPosition = drawPosition.ToPoint();
                elementsForRender[i].screenPosition.X += offset;
                elementsForRender[i].Draw(spriteBatch, elementsForRender[i].ApplyAnchor(elementsForRender[i].screenPosition.ToVector2()));
                if (elementsForRender.Count > 1) offset += size.X / (elementsForRender.Count - 1);
            }
        }
    }
}