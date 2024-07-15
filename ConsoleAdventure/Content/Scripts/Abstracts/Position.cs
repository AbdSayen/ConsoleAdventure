using Microsoft.Xna.Framework;
using System;
using System.Drawing;

namespace ConsoleAdventure
{
    [Serializable]
    public class Position
    {
        public int x { get; private set; }
        public int y { get; private set; }
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
    }
}