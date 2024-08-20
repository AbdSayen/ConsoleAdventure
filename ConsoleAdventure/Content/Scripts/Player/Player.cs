using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ConsoleAdventure.Content.Scripts.Entities;
using ConsoleAdventure.Content.Scripts.InputLogic;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;

namespace ConsoleAdventure.Content.Scripts.Player
{
    [Serializable]
    public class Player : Entity
    {
        public readonly PlayerInfo info;
        public Inventory inventory;

        [NonSerialized]
        public Stopwatch timer = new Stopwatch();

        private int speed = 1;
        private PlayerMovement _movement;

        private bool wasCursorKeyPressedLastFrame;

        public bool isActive = false;

        private byte frames = 0;

        public int holdItemIndex = 1;
        public int holdChestItemIndex = 1;

        internal Inventory chest;
        public bool isChestOpen;

        internal Vector3 chestPosition;

        public bool isCraftOpen = false;

        public Player(short id, string pcid, Position position, int w, int worldLayer = -1) : base(position, w)
        {
            if (worldLayer == -1) this.worldLayer = World.MobsLayerId;
            else this.worldLayer = (byte)worldLayer;
            this.position = position;

            info = new PlayerInfo();
            _movement = new PlayerMovement(speed);
            inventory = new Inventory(this)
            {
                slots =
                {
                    new Stack( new IronPick(), 1),
                }
            };

            chest = new Inventory(this);

            info.Id = id;
            info.pcId = pcid;
            type = (int)RenderFieldType.player;

            AddTypeToMap<Player>(type);

            Initialize();
        }

        public void ClearChest()
        {
            chest = new Inventory(this);
        }

        public byte[] GetPlayerBytes()
        {
            List<byte> data = new List<byte>();

            byte[] inv = SerializeData.Serialize(inventory.slots);

            data.AddRange(BitConverter.GetBytes(inv.Length));
            data.AddRange(inv);

            return data.ToArray();
        }

        public void LoadPlayerFromBytes(byte[] data)
        {
            int inventoryDataLength = BitConverter.ToInt32(data, 0);
            List<byte> dat = data.ToList();
            byte[] inv = dat.GetRange(4, inventoryDataLength).ToArray();
            inventory.slots = SerializeData.Deserialize<List<Stack>>(inv);
        }

        public override string GetSymbol()
        {
            return " ^";
        }

        public override Color GetColor()
        {
            return Color.Yellow;
        }

        public override void InteractWithWorld()
        {
            if (!isActive) return;

            timer.Start();

            if (frames < 2)
                frames++;
            else
            {
                if (oldPos.x != position.x || oldPos.y != position.y)
                    NetworkManager.SendDataAsync(NetworkFuncType.setPlayerPos, position, NetworkManager.Id, 0);
                frames = 0;
            }

            if (Input.IsKeyDown(InputConfig.Run) && timer.Elapsed.TotalMilliseconds > 15 && !ConsoleAdventure.BlockHotKey)
            {
                PerformActions();
            }
            else if (timer.Elapsed.TotalMilliseconds > 50 && !ConsoleAdventure.BlockHotKey)
            {
                PerformActions();
            }

            if (!ConsoleAdventure.kstate.IsKeyDown(InputConfig.Interaction) && ConsoleAdventure.prekstate.IsKeyDown(InputConfig.Interaction))
            {
                for (int i = 0; i < World.CountOfLayers; i++)
                {
                    Transform? block = world.GetField(position.x, position.y, i, w)?.content;

                    if (block != null)
                    {
                        block.Interaction();
                    }
                }
            }

            if (!ConsoleAdventure.kstate.IsKeyDown(InputConfig.InventoryPlus) && ConsoleAdventure.prekstate.IsKeyDown(InputConfig.InventoryPlus) && holdItemIndex < inventory.slots.Count - 1 && !ConsoleAdventure.BlockHotKey)
                holdItemIndex++;
            if (!ConsoleAdventure.kstate.IsKeyDown(InputConfig.InventoryMinus) && ConsoleAdventure.prekstate.IsKeyDown(InputConfig.InventoryMinus) && holdItemIndex > 0 && !ConsoleAdventure.BlockHotKey)
                holdItemIndex--;

            if (!ConsoleAdventure.kstate.IsKeyDown(InputConfig.ChestPlus) && ConsoleAdventure.prekstate.IsKeyDown(InputConfig.ChestPlus) && holdChestItemIndex < chest.slots.Count - 1 && !ConsoleAdventure.BlockHotKey)
                holdChestItemIndex++;
            if (!ConsoleAdventure.kstate.IsKeyDown(InputConfig.ChestMinus) && ConsoleAdventure.prekstate.IsKeyDown(InputConfig.ChestMinus) && holdChestItemIndex > 0 && !ConsoleAdventure.BlockHotKey)
                holdChestItemIndex--;

            if (!ConsoleAdventure.kstate.IsKeyDown(InputConfig.RecipeOpen) && ConsoleAdventure.prekstate.IsKeyDown(InputConfig.RecipeOpen) && !ConsoleAdventure.BlockHotKey)
            {
                if (isCraftOpen)
                    isCraftOpen = false;

                else if (!isCraftOpen)
                    isCraftOpen = true;

                Display.recipesUI.type = 0;
            }

            if (!ConsoleAdventure.kstate.IsKeyDown(InputConfig.RecipeBookOpen) && ConsoleAdventure.prekstate.IsKeyDown(InputConfig.RecipeBookOpen) && !ConsoleAdventure.BlockHotKey)
            {
                if (isCraftOpen)
                {
                    isCraftOpen = false;
                    Display.recipesUI.type = 0;
                }


                else if (!isCraftOpen )
                {
                    isCraftOpen = true;
                    Display.recipesUI.type = 1;
                }
            }

            if (!ConsoleAdventure.kstate.IsKeyDown(InputConfig.DropItem) && ConsoleAdventure.prekstate.IsKeyDown(InputConfig.DropItem) && !ConsoleAdventure.BlockHotKey)
            {
                DropSlot(holdItemIndex);
                NetworkManager.SendDataAsync(NetworkFuncType.dropItem, (short)holdItemIndex, NetworkManager.Id);
            }

            if (isCraftOpen)
            {
                List<Recipe> recipes = Display.recipesUI.type == 1 ? ConsoleAdventure.recipes : ConsoleAdventure.availableRecipes;

                if (!ConsoleAdventure.kstate.IsKeyDown(InputConfig.RecipeListRight) && ConsoleAdventure.prekstate.IsKeyDown(InputConfig.RecipeListRight) && Display.recipesUI.cursorPos < recipes.Count - 1)
                {
                    Display.recipesUI.cursorPos++;
                }

                if (!ConsoleAdventure.kstate.IsKeyDown(InputConfig.RecipeListLeft) && ConsoleAdventure.prekstate.IsKeyDown(InputConfig.RecipeListLeft) && Display.recipesUI.cursorPos > 0)
                {
                    Display.recipesUI.cursorPos--;
                }

                Display.recipesUI.cursorPos = Math.Min(Display.recipesUI.cursorPos, recipes.Count - 1);

                Display.recipesUI.Update();
            }

            if (inventory.slots.Count > 0)
                holdItemIndex = Math.Min(holdItemIndex, inventory.slots.Count - 1);

            if (chest.slots?.Count > 0)
                holdChestItemIndex = Math.Min(holdChestItemIndex, chest.slots.Count - 1);

            if (!ConsoleAdventure.kstate.IsKeyDown(InputConfig.TakeInInventoryStack) && ConsoleAdventure.prekstate.IsKeyDown(InputConfig.TakeInInventoryStack) && !ConsoleAdventure.BlockHotKey && holdChestItemIndex > -1 && chest.slots.Count > 0 && inventory.slots.Count < inventory.maxCount)
            {
                inventory.PickUpItems(new() { chest.slots[holdChestItemIndex] });
                if(chest.slots[holdChestItemIndex].count <= 0)
                {
                    chest.slots.RemoveAt(holdChestItemIndex);
                }
            }

            if (!ConsoleAdventure.kstate.IsKeyDown(InputConfig.TakeInChestStack) && ConsoleAdventure.prekstate.IsKeyDown(InputConfig.TakeInChestStack) && !ConsoleAdventure.BlockHotKey && holdItemIndex > -1 && inventory.slots.Count > 0 && chest.slots.Count < chest.maxCount)
            {
                chest.PickUpItems(new() { inventory.slots[holdItemIndex] });
                if (inventory.slots[holdItemIndex].count <= 0)
                {
                    inventory.slots.RemoveAt(holdItemIndex);
                }
            }

            if(chestPosition != new Vector3(position.x, position.y, w))
            {
                ClearChest();
                chestPosition = new Vector3(-1, -1, -1);
                isChestOpen = false;
            }

            void PerformActions()
            {
                HandlePlayerInput();
                Walk();
                CheckPickUpItems();
                timer.Restart();
            }
        }

        public void DropSlot(int slot)
        {
            Stack stack = inventory.slots[slot];
            new Loot(position, w, new List<Stack>() { stack });
            inventory.RemoveAt(slot, stack.count);
        }

        public void CraftItem(int slot)
        {
            Recipe recipe = ConsoleAdventure.availableRecipes[slot];
            Stack item = recipe.OutItem.Copy();
            inventory.PickUpItems(new List<Stack>() { item });

            if (item.count > 0)
            {
                new Loot(position, w, new List<Stack>() { item });
            }

            for (int i = 0; i < recipe.Ingredients.Count; i++)
            {
                inventory.RemoveItems(recipe.Ingredients.ElementAt(i).Key, recipe.Ingredients.ElementAt(i).Value);
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

            if (Input.IsKeyDown(InputConfig.Building) && inventory.slots?.Count > 0 && holdItemIndex < inventory.slots.Count && !ConsoleAdventure.BlockHotKey)
            {
                Item item = inventory.slots[holdItemIndex].item;
                if (item.placeType > -1 )
                {
                    if (item.placeLayer == World.BlocksLayerId && CanBuildAt(targetPosition, World.BlocksLayerId))
                    {
                        Place(item);
                    }

                    if (item.placeLayer == World.FloorLayerId && CanBuildAt(targetPosition, World.FloorLayerId))
                    {
                        Place(item);
                    }

                    if (item.placeLayer == World.ItemsLayerId && CanBuildAt(targetPosition, World.ItemsLayerId))
                    {
                        Place(item);
                    }

                    if (item.placeLayer == World.MobsLayerId && CanBuildAt(targetPosition, World.MobsLayerId))
                    {
                        Place(item);
                    }
                }
            }
            else if (Input.IsKeyDown(InputConfig.Destroying) && !ConsoleAdventure.BlockHotKey)
            {
                Transform t = world.GetField(targetPosition.x, targetPosition.y, World.BlocksLayerId, w).content;
                if (t?.CanBeDestroyed() == true && CanDestroyAt(targetPosition, World.BlocksLayerId) && inventory.slots[holdItemIndex].item.pick > 0)
                {
                    t.degreeDestruction += (byte)Math.Abs(inventory.slots[holdItemIndex].item.pick / t.hardness);
                    if (t.degreeDestruction >= 100)
                    {
                        world.RemoveSubject(t, World.BlocksLayerId);
                        NetworkManager.SendDataAsync(NetworkFuncType.breakTransform, targetPosition, w, (short)0);
                    }
                }

                Transform t1 = world.GetField(targetPosition.x, targetPosition.y, World.FloorLayerId, w).content;
                if (t1?.CanBeDestroyed() == true && CanDestroyAt(targetPosition, World.FloorLayerId) && inventory.slots[holdItemIndex].item.hammer > 0)
                {
                    world.RemoveSubject(t1, World.FloorLayerId);
                    NetworkManager.SendDataAsync(NetworkFuncType.breakTransform, targetPosition, w, (short)0);
                }

                Transform t2 = world.GetField(targetPosition.x, targetPosition.y, World.ItemsLayerId, w).content;
                if (t2?.CanBeDestroyed() == true && CanDestroyAt(targetPosition, World.ItemsLayerId) && t2 is Chest && inventory.slots[holdItemIndex].item.pick > 0)
                {
                    world.RemoveSubject(t2, World.ItemsLayerId);
                    NetworkManager.SendDataAsync(NetworkFuncType.breakTransform, targetPosition, w, (short)0);
                }
            }

            void Place(Item item)
            {
                SetObject(item.placeType, targetPosition, w, items: new());
                inventory.RemoveAt(holdItemIndex, 1);
                NetworkManager.SendDataAsync(NetworkFuncType.buildTransform, targetPosition, NetworkManager.Id, BitConverter.ToInt16(new byte[] { w, (byte)holdItemIndex }));
            }
        }

        private bool CanBuildAt(Position pos, int layer)
        {
            return world.GetField(pos.x, pos.y, layer, w).content == null;
        }

        private bool CanDestroyAt(Position pos, int layer)
        {
            return world.GetField(pos.x, pos.y, layer, w).content != null;
        }

        private void Walk()
        {
            oldPos = position;
            _movement.Move(this);
        }

        private void CheckPickUpItems()
        {

            if (Input.IsKeyDown(InputConfig.PickUp) && !ConsoleAdventure.BlockHotKey)
            {
                if (TryPickUp())
                    NetworkManager.SendDataAsync(NetworkFuncType.pickUpItem, position, NetworkManager.Id);
            }
        }

        public bool TryPickUp()
        {
            Field itemField = world.GetField(position.x, position.y, World.ItemsLayerId, w);

            if (itemField.content != null)
            {
                if (itemField.content is Loot)
                {
                    ((Loot)itemField.content).PickUpAll(inventory);
                    return true;
                }
            }

            return false;
        }

        public bool TryPickUp(Position pos)
        {
            Field itemField = world.GetField(pos.x, pos.y, World.ItemsLayerId, w);

            if (itemField.content != null)
            {
                if (itemField.content is Loot)
                {
                    ((Loot)itemField.content).PickUpAll(inventory);
                    return true;
                }
            }

            return false;
        }
    }
}
