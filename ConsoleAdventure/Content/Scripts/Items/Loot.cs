using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class Loot : Storage
    {
        public static int blinkTimer;
        public static int blinkPer = 30;
        public static int delay = blinkPer / 2;

        public Loot(Position position, int w, List<Stack> items, int worldLayer = -1) : base(position, w, AddItems(items, position.x, position.y, World.ItemsLayerId, w))
        {
            this.worldLayer = World.ItemsLayerId;
            type = (int)RenderFieldType.loot;

            AddTypeToMap<Loot>(type);

            if (this.items != null)
                items = new();
            Initialize();
        }

        private static List<Stack> AddItems(List<Stack> items, int x, int y, int z, int w)
        {
            if(items == null)
            {
                return items;
            }

            List<Stack> result = items;
            List<Stack> stacks = new List<Stack>();

            Field field = ConsoleAdventure.world.GetField(x, y, z, w);

            if (field?.content != null)
            {
                if (field.content is Loot)
                {
                    stacks = ((Loot)field.content).items;
                }

                if (field.content is Chest)
                {
                    ((Chest)field.content).AddRange(items);
                    return null;
                }
            }

            result.AddRange(stacks);
            return result;
        }

        public void PickUpAll(Inventory inventory)
        {
            inventory.PickUpItems(items);
            bool remove = true;

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].count != 0)
                {
                    remove = false;
                }
            }

            if (remove)
            {
                world.RemoveSubject(this, World.ItemsLayerId);
            }
        }

        public override string GetSymbol()
        {
            if (blinkTimer % blinkPer > delay)
            {
                return "";
            }

            return " $";
        }

        public override Color GetColor()
        {
            return Color.Yellow;
        }


        int drawItemIndex = -1;

        public override void OnTheScreen()
        {
            if (CanDraw() && items != null)
            {
                if(blinkTimer % blinkPer == delay)
                {
                    if(drawItemIndex < items?.Count - 1 && drawItemIndex > -1) drawItemIndex++;
                    else drawItemIndex = 0;
                }

                if (blinkTimer % blinkPer > delay && drawItemIndex > -1)
                {
                    try 
                    {
                        CharTexture charTexture = items[drawItemIndex].Item.GetTexture();
                        Position tryColorPos = new Position(Math.Clamp(position.x - ConsoleAdventure.startDisplay.x, 0, 60), Math.Clamp(position.y - ConsoleAdventure.startDisplay.y, 0, 30));

                        for (int i = 0; i < charTexture.strings.Count; i++)
                        {
                            Vector2 offset = charTexture.offsets[i];
                            Color layerColor = (charTexture.colors[i].ToVector3() * Light.colors[tryColorPos.x, tryColorPos.y].ToVector3()).ToColor();

                            StringPaint.Draw(charTexture.strings[i], position, w, new((offset.X / 18) + 0.5f, offset.Y / 19), layerColor, charTexture.rotations[i]);
                        }
                    }

                    catch
                    {

                    }
                }
            }
        }
    }
}
