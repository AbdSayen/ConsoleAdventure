using Microsoft.Xna.Framework.Input;

namespace ConsoleAdventure
{
    public class Player : Transform
    {
        public int id;
        public string name;

        //characteristics
        private int speed = 1;

        public Player(int id, WorldEngine.World world, Position position, int worldLayer) : base(world, position, worldLayer)
        {
            this.id = id;
            this.world = world;
            renderFieldType = WorldEngine.RenderFieldType.player;
            Initiate();
        }

        private void Initiate()
        {
            position.SetPosition(5, 5);
        }

        public void CheckMove()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Move(speed, Rotation.up);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Move(speed, Rotation.down);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Move(speed, Rotation.left);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Move(speed, Rotation.right);
            }
        }
    }
}
