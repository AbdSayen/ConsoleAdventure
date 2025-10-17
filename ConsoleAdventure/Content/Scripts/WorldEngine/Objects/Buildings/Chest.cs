using ConsoleAdventure.Content.Scripts.Player;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine
{
    public class Chest : Storage
    {
        internal bool isBrake = false;
        public Chest(Position position, int w, List<Stack> items, int worldLayer = -1) : base(position, w, items)
        {
            this.worldLayer = World.BlocksLayerId;
            type = (int)VanillaTransforms.chest;

            AddTypeToMap();

            Initialize();
        }

        public override string GetSymbol()
        {
            return "<>";
        }

        public override Color GetColor()
        {
            return new(94, 61, 38);
        }

        public override void Interaction()
        {
            Player player = world.GetLocalPlayer();
            player.ClearChest();

            if (!player.isChestOpen)
            {
                world.GetLocalPlayer().chest.slots = items;
                player.chestPosition = new Vector3(position.x, position.y, w);
                player.isChestOpen = true;
            }

            else if (player.isChestOpen)
            {
                player.isChestOpen = false;
                player.chestPosition = new Vector3(-1, -1, -1);
            }
        }

        public override void Collapse()
        {
            Player player = world.GetLocalPlayer();
            player.ClearChest();
            player.isChestOpen = false;
            player.chestPosition = new Vector3(-1, -1, -1);
            isBrake = true;
            new Loot(position, w, new List<Stack>() { new Stack(new ChestItem(), 1) });
        }

        public override bool CanBeDestroyed()
        {
            return items == null || items.Count <= 0;
        }
    }
}
