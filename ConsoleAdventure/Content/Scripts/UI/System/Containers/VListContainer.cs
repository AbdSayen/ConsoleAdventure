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
    public class VListContainer : UIElement
    {
        private List<UIElement> elements = new List<UIElement>();
        private List<UIElement> focusableElements = new List<UIElement>();
        private int spacing;

        private int currentlySelected;

        public VListContainer(Point screenPosition, int spacing, Point size = new(), Point margin = new(), Anchor anchor = Anchor.Center, int zOrder = 0) : base(screenPosition, size, margin, anchor, zOrder)
        {
            this.spacing = spacing;
        }

        public void AddElement(UIElement element)
        {
            element.parent = this;
            elements.Add(element);
            focusableElements = elements.FindAll(e => e.IsFocusable() && e.IsVisible());

            List<UIElement> spaceCounting = elements.FindAll(e => e.IsVisible());
            int sizeComputing = spacing * spaceCounting.Count;
            for (int i = 0; i < spaceCounting.Count; i++)
            {
                sizeComputing += spaceCounting[i].size.X;
            }
            size.X = sizeComputing;
        }

        public void RemoveElement(UIElement element)
        {
            element.parent = null;
            elements.Remove(element);
            focusableElements = elements.FindAll(e => e.IsFocusable() && e.IsVisible());

            List<UIElement> spaceCounting = elements.FindAll(e => e.IsVisible());
            int sizeComputing = spacing * spaceCounting.Count;
            //for (int i = 0; i < spaceCounting.Count; i++)
            //{
            //    sizeComputing += spaceCounting[i].size.X;
            //}
            size.X = sizeComputing;
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

            if (!Input.IsKeyDown(InputConfig.NavigationRight) && Input.IsOldKeyDown(InputConfig.NavigationRight))
            {
                if (focusableElements.Count > 0)
                {
                    focusableElements[currentlySelected].OnDefocus();
                    if (currentlySelected++ >= focusableElements.Count - 1)
                        currentlySelected = 0;
                    //if (currentlySelected-- < 0)
                    //    currentlySelected = focusableElements.Count - 1;
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
            List<UIElement> elementsForRender = elements.FindAll((UIElement el) => { return el.IsVisible(); });
            int offset = 0;
            for (int i = 0; i < elementsForRender.Count; i++)
            {
                elementsForRender[i].size.Y = size.Y;
                elementsForRender[i].screenPosition = new();
                //elementsForRender[i].screenPosition.X += offset;
                elementsForRender[i].Draw(spriteBatch, elementsForRender[i].CalculateDrawPosition() + CalculateDrawPosition());
                offset += spacing;
            }
            spriteBatch.DrawFrame(ConsoleAdventure.Font, Utils.GetPanel(new(size.X/9, size.Y/19)), drawPosition, Color.Red);
        }
    }
}