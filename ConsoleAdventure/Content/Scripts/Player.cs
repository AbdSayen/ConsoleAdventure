using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace ConsoleAdventure
{
    [Serializable]
    public class Player : Transform
    {
        public int id;
        public string name = "William";
        public Inventory inventory;
        public Position cursorPosition = new Position();

        [NonSerialized]
        public Stopwatch timer = new Stopwatch();

        private int speed = 1;
        private bool isBuilding = false;
        private bool isDestroying = false;

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

        public void InteractWithWorld()
        {
            timer.Start();
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && timer.Elapsed.TotalMilliseconds > 15)
            {
                PerformActions();
            }
            else if (timer.Elapsed.TotalMilliseconds > 50)
            {
                PerformActions();
            }


            void PerformActions()
            {
                Move();
                Build();
                Destroy();
                CheckPickUpItems();
                timer.Restart();
            }
        }

        private void Move()
        {
            bool isMove = false;

            if (!isBuilding && !isDestroying)
            {
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
            }

            if (isMove)
            {
                world.time.PassTime(3);
            }
        }

        private void CursorMovement()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && cursorPosition.y > -2)
            {
                cursorPosition.SetPosition(cursorPosition.x, cursorPosition.y - 1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && cursorPosition.y < 2)
            {
                cursorPosition.SetPosition(cursorPosition.x, cursorPosition.y + 1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && cursorPosition.x > -2)
            {
                cursorPosition.SetPosition(cursorPosition.x - 1, cursorPosition.y);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && cursorPosition.x < 2)
            {
                cursorPosition.SetPosition(cursorPosition.x + 1, cursorPosition.y);
            }
        }

        private void Build()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.B) && inventory.HasItem(new Log(), 1))
            {
                isBuilding = true;
            }
            if (isBuilding)
            {
                CursorMovement();

                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    Position pos = new Position(position.x + cursorPosition.x, position.y + cursorPosition.y);

                    if (pos.x > 0 && pos.x < world.size && pos.y > 0 && pos.y < world.size &&
                        world.GetField(pos.x, pos.y, World.BlocksLayerId).content == null)
                    {
                        new Plank(world, pos);
                        inventory.RemoveItems(new Log(), 1);
                        cursorPosition = new Position();
                        isBuilding = false;
                        world.time.PassTime(120);
                    }
                }
            }
        }

        private void Destroy()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                isDestroying = true;
            }
            if (isDestroying)
            {
                CursorMovement();

                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    Position pos = new Position(position.x + cursorPosition.x, position.y + cursorPosition.y);

                    if (pos.x > 0 && pos.x < world.size && pos.y > 0 && pos.y < world.size &&
                        world.GetField(pos.x, pos.y, World.BlocksLayerId).content != null)
                    {
                        world.RemoveSubject(world.GetField(pos.x, pos.y, World.BlocksLayerId).content, World.BlocksLayerId);
                        cursorPosition = new Position();
                        isDestroying = false;
                        world.time.PassTime(60);
                    }
                }
            }
        }
        private void CheckPickUpItems()
        {
            Field itemField = world.GetField(position.x, position.y, World.ItemsLayerId);

            if (Keyboard.GetState().IsKeyDown(Keys.P) && itemField.content != null)
            {
                ((Loot)itemField.content).PickUpAll(inventory);
            }
        }
    }
}