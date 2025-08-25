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
    public class HListContainer : UIElement
    {
        private List<UIElement> elements = new List<UIElement>();
        private List<UIElement> focusableElements = new List<UIElement>();

        private int currentlySelected;

        private bool isFocused;

        public HListContainer(Point screenPosition, Point size = new(), Point margin = new(), Anchor anchor = Anchor.Center, int zOrder = 0) : base(screenPosition, size, margin, anchor, zOrder)
        {
        }

        public void AddElement(UIElement element)
        {
            element.parent = this;
            elements.Add(element);
            focusableElements = elements.FindAll(e => e.IsFocusable() && e.IsVisible());
        }

        public void RemoveElement(UIElement element)
        {
            element.parent = null;
            elements.Remove(element);
            focusableElements = elements.FindAll(e => e.IsFocusable() && e.IsVisible());
        }

        public override bool IsFocusable()
        {
            return true;
        }

        public override void OnFocus()
        {
            isFocused = true;
            if (focusableElements.Count > 0 )
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
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].IsVisible())
                {
                    elements[i].Update();
                }
            }

            if (!isFocused) return;
            if (!Input.IsKeyDown(InputConfig.NavigationDown) && Input.IsOldKeyDown(InputConfig.NavigationDown))
            {
                if (focusableElements.Count > 0)
                {
                    focusableElements[currentlySelected].OnDefocus();
                    if (currentlySelected++ >= focusableElements.Count - 1)
                        currentlySelected = 0;
                    focusableElements[currentlySelected].OnFocus();
                }
            }

            if (!Input.IsKeyDown(InputConfig.NavigationUp) && Input.IsOldKeyDown(InputConfig.NavigationUp))
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
            List<UIElement> elementsForRender = elements.FindAll((UIElement el) => { return el.IsVisible(); });
            int offset = 0;
            for (int i = 0; i < elementsForRender.Count; i++)
            {
                if (elementsForRender.Count <= 1) offset += size.Y / 2;
                //elementsForRender[i].size.X = size.X;
                elementsForRender[i].screenPosition = drawPosition.ToPoint();
                elementsForRender[i].screenPosition.Y += offset;
                elementsForRender[i].Draw(spriteBatch, elementsForRender[i].ApplyAnchor(elementsForRender[i].screenPosition.ToVector2()));
                if (elementsForRender.Count > 1) offset += size.Y / (elementsForRender.Count - 1);
            }
            spriteBatch.DrawFrame(ConsoleAdventure.Font, Utils.GetPanel(new(size.X / 9, size.Y / 19)), drawPosition, Color.Red);
        }
    }
}
