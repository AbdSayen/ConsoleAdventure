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
        private Point screenPosition;
        private Point size;
        private Point margin;
        private Anchor anchor;

        private int zOrder;

        private bool isVisible = true;

        public UIElement(Point screenPosition, Point size, Point margin = new(), Anchor anchor = Anchor.Center, int zOrder = 0)
        {
            this.screenPosition = screenPosition;
            this.size = size;
            this.margin = margin;
            this.anchor = anchor;
            this.zOrder = zOrder;
        }

        public Point CalculateDrawPosition()
        {
            Point pos = screenPosition;
            Point marginedSize = size + margin * new Point(2);
            switch(anchor) {
                case Anchor.Left:
                    pos -= new Point(0, marginedSize.Y / 2);
                    break;
                case Anchor.Right:
                    pos -= new Point(marginedSize.X, marginedSize.Y / 2);
                    break;
                case Anchor.Top:
                    pos -= new Point(marginedSize.X / 2, 0);
                    break;
                case Anchor.Bottom:
                    pos -= new Point(marginedSize.X / 2, marginedSize.Y);
                    break;
                case Anchor.TopLeft:
                    break;
                case Anchor.TopRight:
                    pos -= new Point(marginedSize.X, 0);
                    break;
                case Anchor.BottomLeft:
                    pos -= new Point(0, marginedSize.Y);
                    break;
                case Anchor.BottomRight:
                    pos -= new Point(marginedSize.X, marginedSize.Y);
                    break;
                case Anchor.Center:
                    pos -= new Point(marginedSize.X / 2, marginedSize.Y / 2);
                    break;
            }

            Point uv = pos / new Point(1920, 1080);
            return uv * new Point(ConsoleAdventure.screenWidth, ConsoleAdventure.screenHeight);
        }

        public bool IsVisible() { return isVisible; }

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

        public virtual void OnSomeKeyDown()
        {

        }

        public virtual void OnSomeKeyUp()
        {

        }

        public virtual void OnConfirmKeyDown()
        {

        }

        public virtual void OnConfirmKeyUp()
        {

        }

        public virtual bool OnClick()
        {
            return false;
        }

        public virtual void OnFocus()
        {

        }

        public virtual void OnDefocus()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, Point drawPosition)
        {

        }
    }
}
