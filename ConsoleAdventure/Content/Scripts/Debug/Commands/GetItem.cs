using ConsoleAdventure.WorldEngine;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.Debug.Commands
{
    public class GetItem: Command
    {
        public GetItem() 
        {
            Name = "item";
            Description = "gives the player \nan item";
            Arguments = new List<string>()
            {
                "name",
                "count",
                "namespace"
            };
        }

        public override void Logic(string[] args, short id = -2)
        {
            string name = GetStringArg(args, "name");
            int count = GetIntArg(args, "count");
            string namespace_ = "ConsoleAdventure";
            if (args.Length >= 3)
                namespace_ = GetStringArg(args, "namespace");

            try
            {
                Type[] type = new Type[2] { Type.GetType(namespace_ + "." + name), Type.GetType(namespace_ + "." + name + "Item") };

                for (int i = 0; i < type.Length; i++)
                {
                    if (type[i] != null && type[i].IsSubclassOf(typeof(Item)))
                    {
                        if (count < 1) count = 1;

                        Item item = (Item)Activator.CreateInstance(type[i]);
                        if (id == -2) id = NetworkManager.Id;
                        if (id == -1) id = 0;
                        Player.Player pl = ConsoleAdventure.world.players[id];
                        pl.inventory.PickUpItems(new List<Stack>() { new Stack(item, count) });
                        break;
                    }
                }
            }

            catch (Exception) { }
        }
    }
}
