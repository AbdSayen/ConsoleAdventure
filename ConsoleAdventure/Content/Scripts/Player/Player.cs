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

        public Player(int id, Position position, int w, int worldLayer = -1) : base(position, w)
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

            if (Input.IsKeyDown(InputConfig.Run) && timer.Elapsed.TotalMilliseconds > 15 && !ConsoleAdventure.BlockHotKey)
            {
                PerformActions();
            }
            else if (timer.Elapsed.TotalMilliseconds > 50 && !ConsoleAdventure.BlockHotKey)
            {
                PerformActions();
            }

            if(!ConsoleAdventure.kstate.IsKeyDown(InputConfig.Interaction) && ConsoleAdventure.prekstate.IsKeyDown(InputConfig.Interaction))
            {
                int? blockType = world.GetField(position.x, position.y, World.BlocksLayerId, w)?.content?.type;

                if (blockType == (int)RenderFieldType.descent)
                {
                    if (world.players[0].SetPosition(world.players[0].position, 0))
                        ConsoleAdventure.curDeep = 0;
                }

                if (blockType == (int)RenderFieldType.climb)
                {
                    if (world.players[0].SetPosition(world.players[0].position, 1))
                        ConsoleAdventure.curDeep = 1;
                }
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

            if (isCursorKeyPressed && !wasCursorKeyPressedLastFrame && !ConsoleAdventure.BlockHotKey)
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

            if (Input.IsKeyDown(InputConfig.Building) && CanBuildAt(targetPosition) && !ConsoleAdventure.BlockHotKey)
            {
                if (inventory.HasItems(new Log(), 1))
                {
                    new Plank(targetPosition, w);
                    inventory.RemoveItems(new Log(), 1);
                    world.time.PassTime(120);
                }
                else
                {
                    Loger.AddLog("Not enough resources to build!");
                }
            }
            else if (Input.IsKeyDown(InputConfig.Destroying) && CanDestroyAt(targetPosition) && !ConsoleAdventure.BlockHotKey)
            {
                Transform t = world.GetField(targetPosition.x, targetPosition.y, World.BlocksLayerId, w).content;
                if (t.CanBeDestroyed())
                {
                    world.RemoveSubject(t, World.BlocksLayerId);
                    world.time.PassTime(60);
                }
            }
        }

        private bool CanBuildAt(Position pos)
        {
            return world.GetField(pos.x, pos.y, World.BlocksLayerId, w).content == null;
        }

        private bool CanDestroyAt(Position pos)
        {
            return world.GetField(pos.x, pos.y, World.BlocksLayerId, w).content != null;
        }

        private void Walk()
        {
            _movement.Move(this);
        }

        private void CheckPickUpItems()
        {
            Field itemField = world.GetField(position.x, position.y, World.ItemsLayerId, w);

            if (Input.IsKeyDown(InputConfig.PickUp) && itemField.content != null && !ConsoleAdventure.BlockHotKey)
            {
                if (itemField.content is Loot)
                    ((Loot)itemField.content).PickUpAll(inventory);
            }
        }
    }
}
