using ConsoleAdventure.Content.Scripts.UI.System.Containers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleAdventure.Content.Scripts.UI.System
{
    public enum Anchor
    {
        Left,
        Right,
        Top,
        Bottom,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Center
    }

    public class UIElement
    {
        private string name;

        internal Point screenPosition;
        internal Point size;
        internal Point margin;
        internal Anchor anchor;

        internal int zOrder;

        internal bool isVisible = true;

        private UIContainer parent;

        internal bool hovered;

        public UIElement(Point screenPosition, Point size = new(), Anchor anchor = Anchor.Center, int zOrder = 0, string name = null)
        {
            this.screenPosition = screenPosition;
            this.size = size;
            this.anchor = anchor;
            this.zOrder = zOrder;
            this.name = name;
        }

        public void Destroy()
        {
            GetParent().RemoveElement(this);
        }

        public UIElement SetName(string name)
        {
            this.name = name;
            return this;
        }

        public string GetName()
        {
            return name;
        }

        public void SetParent(UIContainer p)
        {
            parent = p;
        }

        public UIContainer GetParent()
        {
            return parent;
        }

        public Vector2 CalculateDrawPosition()
        {
            Vector2 uv = screenPosition.ToVector2() / new Vector2(1920, 1080);
            Vector2 pos = uv * new Vector2(ConsoleAdventure.Width, ConsoleAdventure.Height);
            return ApplyAnchor(pos);
        }

        public Vector2 ApplyAnchor(Vector2 pos)
        {
            Vector2 marginedSize = size.ToVector2() + margin.ToVector2() * new Vector2(2);
            switch (anchor)
            {
                case Anchor.Left:
                    pos -= new Vector2(0, marginedSize.Y / 2f);
                    break;
                case Anchor.Right:
                    pos -= new Vector2(marginedSize.X, marginedSize.Y / 2f);
                    break;
                case Anchor.Top:
                    pos -= new Vector2(marginedSize.X / 2f, 0);
                    break;
                case Anchor.Bottom:
                    pos -= new Vector2(marginedSize.X / 2f, marginedSize.Y);
                    break;
                case Anchor.TopLeft:
                    break;
                case Anchor.TopRight:
                    pos -= new Vector2(marginedSize.X, 0);
                    break;
                case Anchor.BottomLeft:
                    pos -= new Vector2(0, marginedSize.Y);
                    break;
                case Anchor.BottomRight:
                    pos -= new Vector2(marginedSize.X, marginedSize.Y);
                    break;
                case Anchor.Center:
                    pos -= new Vector2(marginedSize.X / 2f, marginedSize.Y / 2f);
                    break;
            }

            return pos;
        }

        public bool IsVisible() { return isVisible; }
        public bool IsHovered() { return hovered; }

        public void Hide()
        {
            isVisible = false;
        }

        public void Show()
        {
            isVisible = true;
        }

        public void ToggleVisibility()
        {
            isVisible = !isVisible;
        }

        public Point GetScreenPosition()
        {
            return screenPosition;
        }

        public Point GetMargin()
        {
            return margin;
        }

        public Anchor GetAnchor()
        {
            return anchor;
        }

        public int GetZOrder()
        {
            return zOrder;
        }

        public virtual void OnConfirmKeyDown()
        {

        }

        public virtual void OnConfirmKeyUp()
        {

        }

        public virtual bool IsFocusable()
        {
            return false;
        }

        public virtual void OnFocus()
        {
            hovered = true;
        }

        public virtual void OnDefocus()
        {
            hovered = false;
        }

        public virtual void Update()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {

        }
    }
}
