using Microsoft.Xna.Framework;
using System;

namespace ConsoleAdventure
{
    [Serializable]
    public struct Position
    {
        public int x { get; set; }
        public int y { get; set; }
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Position()
        {
            x = 0;
            y = 0;
        }

        public void SetPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Position Zero()
        {
            return new Position(0, 0);
        }

        public Vector2 ToVector2()
        {
            return new(x, y);
        }

        public Point ToPoint()
        {
            return new(x, y);
        }

        #region Operators

        public static Position operator +(Position left, Position right)
        {
            return new(left.x + right.x, left.y + right.y);
        }

        public static Position operator -(Position left, Position right)
        {
            return new(left.x - right.x, left.y - right.y);
        }

        public static Position operator *(Position left, Position right)
        {
            return new(left.x * right.x, left.y * right.y);
        }

        public static Position operator /(Position left, Position right)
        {
            return new(left.x / right.x, left.y / right.y);
        }

        public static Position operator *(Position left, int right)
        {
            return new(left.x * right, left.y * right);
        }

        public static Position operator /(Position left, int right)
        {
            return new(left.x / right, left.y / right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return left.x != right.x && left.y != right.y;
        }

        public static bool operator ==(Position left, Position right)
        {
            return left.x == right.x && left.y == right.y;
        }

        #endregion
    }
}