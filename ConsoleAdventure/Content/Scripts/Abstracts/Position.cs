using System;

namespace ConsoleAdventure.Content.Scripts.Abstracts
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

        public int Magnitude()
        {
            return x * x + y * y;
        }

        public float MagnitudeFloat()
        {
            return x * x + y * y;
        }

        public Position Normalize()
        {
            float num = 1f / MathF.Sqrt(MagnitudeFloat());

            Console.WriteLine($"NORMILIZE: {num}");
            //x *= Convert.ToInt32(num);
            //y *= Convert.ToInt32(num);
            
            return this;
        }

        public static Position Zero()
        {
            return new Position(0, 0);
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

        #endregion
    }
}