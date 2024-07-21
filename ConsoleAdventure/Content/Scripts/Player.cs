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
        public Position cursorPosition = null;

        [NonSerialized]
        public Stopwatch timer = new Stopwatch();

        private int speed = 1;
        private bool isMove = false;
        private IPlayerState currentState;

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
            currentState = new IdleState(this);
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
                currentState.HandleInput();
                Walk();
                CheckPickUpItems();
                timer.Restart();
            }
        }

        public void Walk()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Move(speed, Rotation.up);
                isMove = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Move(speed, Rotation.down);
                isMove = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Move(speed, Rotation.left);
                isMove = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Move(speed, Rotation.right);
                isMove = true;
            }
        }

        internal void CursorMovement()
        {
            if (cursorPosition == null) cursorPosition = new Position(0, 0);

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
            if (Keyboard.GetState().IsKeyDown(Keys.C))
            {
                currentState = new IdleState(this);
                cursorPosition = null;
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

        public void ChangeState(IPlayerState newState)
        {
            currentState = newState;
        }
    }

    public interface IPlayerState
    {
        void HandleInput();
    }

    public class IdleState : IPlayerState
    {
        private Player player;

        public IdleState(Player player)
        {
            this.player = player;
        }

        public void HandleInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.B) && player.inventory.HasItem(new Log(), 1))
            {
                player.ChangeState(new BuildingState(player));
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.V))
            {
                player.ChangeState(new DestroyingState(player));
            }
        }
    }

    public class BuildingState : IPlayerState
    {
        private Player player;

        public BuildingState(Player player)
        {
            this.player = player;
        }

        public void HandleInput()
        {
            player.CursorMovement();

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Position pos = new Position(player.position.x + player.cursorPosition.x, player.position.y + player.cursorPosition.y);

                if (pos.x > 0 && pos.x < player.world.size && pos.y > 0 && pos.y < player.world.size &&
                    player.world.GetField(pos.x, pos.y, World.BlocksLayerId).content == null)
                {
                    new Plank(player.world, pos);
                    player.inventory.RemoveItems(new Log(), 1);
                    player.world.time.PassTime(120);
                }
            }
        }
    }

    public class DestroyingState : IPlayerState
    {
        private Player player;

        public DestroyingState(Player player)
        {
            this.player = player;
        }

        public void HandleInput()
        {
            player.CursorMovement();

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Position pos = new Position(player.position.x + player.cursorPosition.x, player.position.y + player.cursorPosition.y);

                if (pos.x > 0 && pos.x < player.world.size && pos.y > 0 && pos.y < player.world.size &&
                    player.world.GetField(pos.x, pos.y, World.BlocksLayerId).content != null)
                {
                    player.world.RemoveSubject(player.world.GetField(pos.x, pos.y, World.BlocksLayerId).content, World.BlocksLayerId);
                    player.world.time.PassTime(60);
                }
            }
        }
    }
}