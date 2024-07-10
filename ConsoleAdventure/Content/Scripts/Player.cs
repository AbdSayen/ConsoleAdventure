using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework.Input;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

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

        int timer = 10;
        public void InteractWithWorld()
        {
            if (timer >= 2)
            {
                CheckMove();
                timer = 0;
            }
            
            CheckPickUpItems();
            timer++;
        }

        private void CheckMove()
        {
            bool isMove = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                Move(speed, Rotation.up);
                isMove = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                Move(speed, Rotation.down);
                isMove = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                Move(speed, Rotation.left);
                isMove = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                Move(speed, Rotation.right); 
                isMove = true;
            }

            if (isMove)
            {
                world.time.PassTime(3);
            }
        }

        private void CheckPickUpItems()
        {
            Field itemField = world.GetField(position.x, position.y, World.BlocksLayerId);

            if (Keyboard.GetState().IsKeyDown(Keys.P) && itemField.content != null)
            {
                ((Loot)itemField.content).PickUpAll(inventory);
            }
        }
    }
}
