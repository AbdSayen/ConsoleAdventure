﻿using System;
using System.Diagnostics;
using ConsoleAdventure.Content.Scripts.Abstracts;
using ConsoleAdventure.Content.Scripts.InputLogic;
using ConsoleAdventure.Content.Scripts.Player.States;
using ConsoleAdventure.WorldEngine;

namespace ConsoleAdventure.Content.Scripts.Player
{
    [Serializable]
    public class Player : Transform
    {
        public int id;
        public string name = "William";
        public Inventory inventory;
        public Position cursorPosition;
        public readonly Cursor Cursor;

        [NonSerialized]
        public Stopwatch timer = new Stopwatch();

        private int speed = 1;
        private bool isMove = false;
        private IPlayerState currentState;
        private PlayerMovement _movement;

        public Player(int id, World world, Position position, int worldLayer = -1) : base(world, position)
        {
            if (worldLayer == -1) this.worldLayer = World.MobsLayerId;
            else this.worldLayer = worldLayer;
            this.position = position;

            this.id = id;
            this.world = world;
            renderFieldType = RenderFieldType.player;

            _movement = new PlayerMovement(this, speed);
            inventory = new Inventory(this);
            currentState = new IdleState(this);
            Cursor = new Cursor();
            
            Initialize();
        }

        public void InteractWithWorld()
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
                currentState.HandleInput();
                Walk();
                CheckPickUpItems();
                timer.Restart();
            }
        }

        public void Walk()
        {
            _movement.Move();
            
            if (Input.IsKeyDown(InputConfig.Clear))
            {
                currentState = new IdleState(this);
                cursorPosition = Position.Zero();
            }
        }

        private void CheckPickUpItems()
        {
            Field itemField = world.GetField(position.x, position.y, World.ItemsLayerId);

            if (Input.IsKeyDown(InputConfig.PickUp) && itemField.content != null)
            {
                ((Loot)itemField.content).PickUpAll(inventory);
            }
        }

        public void ChangeState(IPlayerState newState)
        {
            currentState = newState;
        }
    }
}