using System;
using System.Diagnostics;
using ConsoleAdventure.Content.Scripts.Entities;
using ConsoleAdventure.Content.Scripts.InputLogic;
using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure.Content.Scripts.Player
{
    [Serializable]
    public class Player : Entity
    {
        public readonly PlayerInfo info;
        public readonly Inventory inventory;

        [NonSerialized]
        public Stopwatch timer = new Stopwatch();

        private int speed = 1;
        private PlayerMovement _movement;

        private bool wasCursorKeyPressedLastFrame;

        public Player(int id, Position position, int worldLayer = -1) : base(position)
        {
            if (worldLayer == -1) this.worldLayer = World.MobsLayerId;
            else this.worldLayer = worldLayer;
            this.position = position;

            info = new PlayerInfo();
            _movement = new PlayerMovement(speed);
            inventory = new Inventory(this);

            info.Id = id;
            this.world = world;
            type = (int)RenderFieldType.player;

            AddTypeToMap<Player>(type);

            Initialize();
        }

        public override string GetSymbol()
        {
            return " ^";
        }

        public override Color GetColor()
        {
            return Microsoft.Xna.Framework.Color.Yellow;
        }
        
        public override void InteractWithWorld()
        {
            timer.Start();

            if (Input.IsKeyDown(InputConfig.Run) && timer.Elapsed.TotalMilliseconds > 15)
            {
                PerformActions();
            }
            else if (timer.Elapsed.TotalMilliseconds > 50)
            {
                PerformActions();
            }

            void PerformActions()
            {
                HandlePlayerInput();
                Walk();
                CheckPickUpItems();
                timer.Restart();
            }
        }

        private void HandlePlayerInput()
        {
            bool isCursorKeyPressed = Input.IsKeyDown(InputConfig.Cursor);

            if (isCursorKeyPressed && !wasCursorKeyPressedLastFrame)
            {
                Cursor.Instance.Toggle();
            }

            wasCursorKeyPressedLastFrame = isCursorKeyPressed;

            if (Cursor.Instance.IsActive)
            {
                HandleCursorInput();
                Cursor.Instance.CursorMovement();
            }
        }

        private void HandleCursorInput()
        {
            Position targetPosition = new Position(position.x + Cursor.Instance.CursorPosition.x, position.y + Cursor.Instance.CursorPosition.y);

            if (targetPosition.x <= 0 || targetPosition.x >= world.size || targetPosition.y <= 0 || targetPosition.y >= world.size)
            {
                return;
            }

            if (Input.IsKeyDown(InputConfig.Building) && CanBuildAt(targetPosition))
            {
                if (inventory.HasItems(new Log(), 1))
                {
                    new Plank(targetPosition);
                    inventory.RemoveItems(new Log(), 1);
                    world.time.PassTime(120);
                }
                else
                {
                    Loger.AddLog("Not enough resources to build!");
                }
            }
            else if (Input.IsKeyDown(InputConfig.Destroying) && CanDestroyAt(targetPosition))
            {
                world.RemoveSubject(world.GetField(targetPosition.x, targetPosition.y, World.BlocksLayerId).content, World.BlocksLayerId);
                world.time.PassTime(60);
            }
        }

        private bool CanBuildAt(Position pos)
        {
            return world.GetField(pos.x, pos.y, World.BlocksLayerId).content == null;
        }

        private bool CanDestroyAt(Position pos)
        {
            return world.GetField(pos.x, pos.y, World.BlocksLayerId).content != null;
        }

        private void Walk()
        {
            _movement.Move(this);
        }

        private void CheckPickUpItems()
        {
            Field itemField = world.GetField(position.x, position.y, World.ItemsLayerId);

            if (Input.IsKeyDown(InputConfig.PickUp) && itemField.content != null)
            {
                if (itemField.content is Loot)
                    ((Loot)itemField.content).PickUpAll(inventory);
            }
        }
    }
}
