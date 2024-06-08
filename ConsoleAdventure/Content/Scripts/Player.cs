using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework.Input;

namespace ConsoleAdventure
{
    public class Player : Transform
    {
        public int id;
        public string name = "Abdurrsss";

        //characteristics
        private int speed = 1;
        public Inventory inventory;

        public Player(int id, World world, Position position = null, int worldLayer = -1) : base(world, position)
        {
            if (worldLayer == -1) this.worldLayer = World.MobsLayerId;
            else this.worldLayer = worldLayer;
            if (position != null) this.position = position;
            else this.position = new Position(0, 0);

            this.id = id;
            this.world = world;
            renderFieldType = RenderFieldType.player;

            inventory = new Inventory(this);
            Initialize();
        }

        public void InteractWithWord()
        {
            CheckMove();
            CheckPickUpItems();
        }

        private void CheckMove()
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                Move(speed, Rotation.up);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                Move(speed, Rotation.down);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Move(speed, Rotation.left);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Move(speed, Rotation.right);
            }
        }

        private void CheckPickUpItems()
        {
            Field itemField = world.fields[World.ItemsLayerId][position.y][position.x];

            if (Keyboard.GetState().IsKeyDown(Keys.P) && itemField.content != null)
            {
                ((Loot)itemField.content).PickUpAll(inventory);
            }
        }
    }
}
