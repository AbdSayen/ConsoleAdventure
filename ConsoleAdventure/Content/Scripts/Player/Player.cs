using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ConsoleAdventure.CaModLoaderAPI;
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
        public PlayerInfo info;
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

        public int buffCursor = 0;

        public int postKillTimer = 0;

        public Player(short id, string pcid, Position position, int w) : base(position, w)
        {
            info = new PlayerInfo();
            _movement = new PlayerMovement(speed);

            inventory = new Inventory(this)
            {
                slots =
                {
                    new Stack( new IronPick(), 1),
                    new Stack( new TorchItem(), 1),
                }
            };

            for (int i = 0; i < CaModLoader.modGlobalPlayers.Count; i++)
            {
                List<Stack> modStartInventory = CaModLoader.modGlobalPlayers[i].SetStartItems(inventory.slots);
                if (modStartInventory != null)
                    inventory.slots = modStartInventory;
            }

            chest = new Inventory(this);

            info.Id = id;
            info.pcId = pcid;

            type = (int)VanillaTransforms.player;

            SetMaxLife(20);
            Initialize();
        }

        public void LoadPlayerInfo(Dictionary<string, string> data)
        {
            info = new PlayerInfo(data);
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

        Color bgColor = new(0, 0, 0, 0);
        public override Color? GetBGColor()
        {
            if (postKillTimer > 0)
                return bgColor = Color.Lerp(bgColor, Color.Red, 0.1f);

            bgColor = new(0, 0, 0, 0);
            return null;
        }

        public override void OnTheScreen()
        {
            for (int i = 0; i < CaModLoader.modGlobalPlayers.Count; i++)
            {
                bool draw = CaModLoader.modGlobalPlayers[i].OnTheScreen(this);
                if (draw != true)
                    return;
            }

            if (inventory.slots.Count > 0 && holdItemIndex > -1 && holdItemIndex < inventory.slots.Count && inventory.slots[holdItemIndex].Item is TorchItem)
            {
                Light.Add(position, w, Color.White);
            }
        }

        public Map map = new(); ////

        public override void InteractWithWorld()
        {
            if (!isActive) 
            {
                if (postKillTimer > 0)
                {
                    postKillTimer--;
                    life = 0;

                    if (postKillTimer == 0)
                    {
                        Loger.AddLog(Localization.GetTranslation("Events", "DeadPlayer"));
                        SetPosition(new(4, 4), world.spawnW);
                        life = maxLife;
                        buffs.Clear();
                        isActive = true;
                    }
                }

                return; 
            }

            map.Update();

            UpdateBuffs();

            timer.Start();

            if (Input.IsKeyDown(InputConfig.Run) && timer.Elapsed.TotalMilliseconds > 15 && !ConsoleAdventure.BlockHotKey)
            {
                PerformActions();
            }
            else if (timer.Elapsed.TotalMilliseconds > 50 && !ConsoleAdventure.BlockHotKey)
            {
                PerformActions();
            }

            if (Input.PostClick(InputConfig.Interaction) && !ConsoleAdventure.BlockHotKey)
            {
                for (int i = 0; i < World.CountOfLayers; i++)
                {
                    Transform block = world.GetField(position.x, position.y, i, w)?.content;

                    if (block != null)
                    {
                        if (CaModLoader.PreInteractionMods(block))
                            block.Interaction();
                    }
                }
            }

            if (Input.PostClick(InputConfig.InventoryPlus) && holdItemIndex < inventory.slots.Count - 1 && !ConsoleAdventure.BlockHotKey)
                holdItemIndex++;
            if (Input.PostClick(InputConfig.InventoryMinus) && holdItemIndex > 0 && !ConsoleAdventure.BlockHotKey)
                holdItemIndex--;

            if (Input.PostClick(InputConfig.ChestPlus) && holdChestItemIndex < chest.slots.Count - 1 && !ConsoleAdventure.BlockHotKey)
                holdChestItemIndex++;
            if (Input.PostClick(InputConfig.ChestMinus) && holdChestItemIndex > 0 && !ConsoleAdventure.BlockHotKey)
                holdChestItemIndex--;

            Recipes();

            if (Input.PostClick(InputConfig.DropItem) && !ConsoleAdventure.BlockHotKey)
            {
                DropSlot(holdItemIndex);
            }

            if (inventory.slots.Count > 0)
                holdItemIndex = Math.Min(holdItemIndex, inventory.slots.Count - 1);

            if (chest.slots?.Count > 0)
                holdChestItemIndex = Math.Min(holdChestItemIndex, chest.slots.Count - 1);

            if (holdItemIndex >= 0 && holdItemIndex < inventory.slots.Count)
            {
                Item curItem = inventory.slots[holdItemIndex].Item;
                if (curItem.damageClass == 1 && curItem.damage > 0 && ConsoleAdventure.kstate.IsKeyDown(InputConfig.Use.key))
                {
                    for (int i = 0; i < world.entities.Count; i++)
                    {
                        if (world.entities[i].maxLife > 0)
                        {
                            Position pos = world.entities[i].position;
                            if (pos >= position + new Position(-1, -1) && pos <= position + new Position(1, 1) && pos != position && world.entities[i].w == w)
                            {
                                if (world.entities[i].invulnerabilityTime <= 0)
                                    world.entities[i].Hit(curItem.damage);
                            }
                        }
                    }
                }

                if (curItem.canUse && !ConsoleAdventure.kstate.IsKeyDown(InputConfig.Use.key) && ConsoleAdventure.prekstate.IsKeyDown(InputConfig.Use.key))
                {
                    curItem.UseItem();
                    if (curItem.consume)
                    {
                        inventory.RemoveAt(holdItemIndex, 1);
                    }
                }
            }

            if (Input.PostClick(InputConfig.TakeInInventoryStack) && isChestOpen && !ConsoleAdventure.BlockHotKey && holdChestItemIndex > -1 && chest.slots.Count > 0) //&& inventory.slots.Count < inventory.maxCount)
            {
                inventory.PickUpItems(new() { chest.slots[holdChestItemIndex] });
                if(chest.slots[holdChestItemIndex].count <= 0)
                {
                    chest.slots.RemoveAt(holdChestItemIndex);
                }

                UpdatedChunk(new Position((int)chestPosition.X, (int)chestPosition.Y), World.BlocksLayerId);
            }

            if (Input.PostClick(InputConfig.TakeInChestStack) && isChestOpen && !ConsoleAdventure.BlockHotKey && holdItemIndex > -1 && inventory.slots.Count > 0) //&& chest.slots.Count < chest.maxCount)
            {
                chest.PickUpItems(new() { inventory.slots[holdItemIndex] });
                if (inventory.slots[holdItemIndex].count <= 0)
                {
                    inventory.slots.RemoveAt(holdItemIndex);
                }

                UpdatedChunk(new Position((int)chestPosition.X, (int)chestPosition.Y), World.BlocksLayerId);
            }

            if (Input.PostClick(InputConfig.TakeInInventory) && isChestOpen && !ConsoleAdventure.BlockHotKey && holdChestItemIndex > -1 && chest.slots.Count > 0) //&& inventory.slots.Count < inventory.maxCount)
            {
                chest.slots[holdChestItemIndex] = inventory.AddItem(chest.slots[holdChestItemIndex], 1);             
                if (chest.slots[holdChestItemIndex].count <= 0)
                {
                    chest.slots.RemoveAt(holdChestItemIndex);
                }

                UpdatedChunk(new Position((int)chestPosition.X, (int)chestPosition.Y), World.BlocksLayerId);
            }

            if (Input.PostClick(InputConfig.TakeInChest) && isChestOpen && !ConsoleAdventure.BlockHotKey && holdItemIndex > -1 && inventory.slots.Count > 0) //&& chest.slots.Count < chest.maxCount)
            {
                inventory.slots[holdItemIndex] = chest.AddItem(inventory.slots[holdItemIndex], 1);
                if (inventory.slots[holdItemIndex].count <= 0)
                {
                    inventory.slots.RemoveAt(holdItemIndex);
                }

                UpdatedChunk(new Position((int)chestPosition.X, (int)chestPosition.Y), World.BlocksLayerId);
            }

            if (chestPosition != new Vector3(position.x, position.y, w))
            {
                ClearChest();
                chestPosition = new Vector3(-1, -1, -1);
                isChestOpen = false;
            }
            
            if (Input.PostClick(InputConfig.BuffsPlus) && buffCursor < buffs.Count - 1 && !ConsoleAdventure.BlockHotKey && chest == null)
                buffCursor++;
            if (Input.PostClick(InputConfig.BuffsMinus) && buffCursor > 0 && !ConsoleAdventure.BlockHotKey && chest == null)
                buffCursor--;

            if (life <= 0)
            {
                life = 0;
                postKillTimer = 300;
                isActive = false;
            }

            invulnerabilityTime--;

            void PerformActions()
            {
                HandlePlayerInput();
                Walk();
                CheckPickUpItems();
                timer.Restart();
            }

            for (int i = 0; i < CaModLoader.modGlobalPlayers.Count; i++)
            {
                CaModLoader.modGlobalPlayers[i].PostInteractWithWorld(this);
            }
        }

        private void Recipes()
        {
            if (Input.PostClick(InputConfig.RecipeOpen) && !ConsoleAdventure.BlockHotKey)
            {
                if (isCraftOpen)
                    isCraftOpen = false;

                else if (!isCraftOpen)
                    isCraftOpen = true;

                Display.recipesUI.type = 0;
            }

            if (Input.PostClick(InputConfig.RecipeBookOpen) && !ConsoleAdventure.BlockHotKey)
            {
                if (isCraftOpen)
                {
                    isCraftOpen = false;
                    Display.recipesUI.type = 0;
                }


                else if (!isCraftOpen)
                {
                    isCraftOpen = true;
                    Display.recipesUI.type = 1;
                }
            }

            if (isCraftOpen)
            {
                List<Recipe> recipes = Display.recipesUI.type == 1 ? ConsoleAdventure.recipes : ConsoleAdventure.availableRecipes;

                if (Input.PostClick(InputConfig.RecipeListRight) && Display.recipesUI.cursorPos < recipes.Count - 1)
                {
                    Display.recipesUI.cursorPos++;
                }

                if (Input.PostClick(InputConfig.RecipeListLeft) && Display.recipesUI.cursorPos > 0)
                {
                    Display.recipesUI.cursorPos--;
                }

                Display.recipesUI.cursorPos = Math.Min(Display.recipesUI.cursorPos, recipes.Count - 1);

                Display.recipesUI.Update();
            }
        }

        public void DropSlot(int slot)
        {
            if(inventory.slots.Count > 0 && slot > -1 && slot < inventory.slots.Count)
            {
                Stack stack = inventory.slots[slot];
                stack.Item.Droped();
                new Loot(position, w, new List<Stack>() { stack });
                inventory.RemoveAt(slot, stack.count);
            }
        }

        public void CraftItem(int slot)
        {
            Recipe recipe = ConsoleAdventure.availableRecipes[slot];

            for (int i = 0; i < CaModLoader.modGlobalPlayers.Count; i++)
            {
                bool craft = CaModLoader.modGlobalPlayers[i].CraftItem(this, recipe);
                if (craft != true)
                    return;
            }

            Stack item = recipe.OutItem.Copy();
            inventory.PickUpItems(new List<Stack>() { item });

            if (item.count > 0)
            {
                new Loot(position, w, new List<Stack>() { item });
            }

            if (!ConsoleAdventure.GodMode) 
            {
                for (int i = 0; i < recipe.Ingredients.Count; i++)
                {
                    inventory.RemoveItems(recipe.Ingredients.ElementAt(i).Key, recipe.Ingredients.ElementAt(i).Value);
                } 
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
            Position targetPosition = position + Cursor.Instance.CursorPosition;

            if (targetPosition.x <= 0 || targetPosition.x >= world.size || targetPosition.y <= 0 || targetPosition.y >= world.size)
            {
                return;
            }

            if (Input.IsKeyDown(InputConfig.Use) && inventory.slots?.Count > 0 && holdItemIndex < inventory.slots.Count && !ConsoleAdventure.BlockHotKey)
            {
                Item item0 = inventory.slots[holdItemIndex].Item;

                if (item0.GetType().BaseType == typeof(PlaceableItem))
                {
                    PlaceableItem item = (PlaceableItem)item0;

                    if (item.placeType > -1)
                    {
                        byte? placeLayer = DefaultWorldLayer[item.placeType];

                        if (placeLayer.HasValue)
                        {
                            if (CanBuildAt(targetPosition, placeLayer.Value))
                            {
                                Place(item);
                                return;
                            }
                        }

                        else
                        {
                            if (CanBuildAt(targetPosition, item.placeLayer))
                            {
                                Place(item);
                                return;
                            }
                        }

                        
                    }
                }

                Transform t = world.GetField(targetPosition.x, targetPosition.y, World.BlocksLayerId, w).content;
                if (t?.CanBeDestroyed() == true && CanDestroyAt(targetPosition, World.BlocksLayerId) && inventory.slots[holdItemIndex].Item.pick > 0 && Hardness[t.type] > 0)
                {
                    t.degreeDestruction += (byte)Math.Abs(inventory.slots[holdItemIndex].Item.pick / Hardness[t.type]);
                    if (t.degreeDestruction >= 100)
                    {
                        world.RemoveSubject(t, World.BlocksLayerId);
                    }
                }

                Transform t1 = world.GetField(targetPosition.x, targetPosition.y, World.FloorLayerId, w).content;
                if (t1?.CanBeDestroyed() == true && CanDestroyAt(targetPosition, World.FloorLayerId) && inventory.slots[holdItemIndex].Item.hammer > 0)
                {
                    world.RemoveSubject(t1, World.FloorLayerId);
                }

                Transform t2 = world.GetField(targetPosition.x, targetPosition.y, World.ItemsLayerId, w).content;
                if (t2?.CanBeDestroyed() == true && CanDestroyAt(targetPosition, World.ItemsLayerId) && t2 is Chest && inventory.slots[holdItemIndex].Item.pick > 0)
                {
                    world.RemoveSubject(t2, World.ItemsLayerId);
                }
            }

            void Place(PlaceableItem item)
            {
                SetObject(item.placeType, targetPosition, w);
                inventory.RemoveAt(holdItemIndex, 1);
            }
        }

        private bool CanBuildAt(Position pos, int layer)
        {
            for (int i = 0; i < CaModLoader.modGlobalPlayers.Count; i++)
            {
                bool? build = CaModLoader.modGlobalPlayers[i].CanBuildAt(this, pos, layer);
                if (build != null)
                    return (bool)build;
            }
            return world.GetField(pos.x, pos.y, layer, w).content == null;
        }

        private bool CanDestroyAt(Position pos, int layer)
        {
            for (int i = 0; i < CaModLoader.modGlobalPlayers.Count; i++)
            {
                bool? destroy = CaModLoader.modGlobalPlayers[i].CanDestroyAt(this, pos, layer);
                if (destroy != null)
                    return (bool)destroy;
            }
            return world.GetField(pos.x, pos.y, layer, w).content != null;
        }

        private void Walk()
        {
            _movement.Move(this);
        }

        private void CheckPickUpItems()
        {

            if (Input.IsKeyDown(InputConfig.PickUp) && !ConsoleAdventure.BlockHotKey)
            {
                if (TryPickUp()) { }
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
