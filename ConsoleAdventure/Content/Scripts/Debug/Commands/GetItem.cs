using ConsoleAdventure.WorldEngine;
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
                "count"
            };
        }

        public override void Logic(string[] args)
        {
            string name = GetStringArg(args, "name");
            int count = GetIntArg(args, "count");

            try
            {
                Type type = Type.GetType(name);

                if (type != null && type.IsSubclassOf(typeof(Item))) 
                {
                    if (count < 1) count = 1;
                    
                    Item item = (Item)Activator.CreateInstance(type);
                    ConsoleAdventure.world.players[0].inventory.PickUpItems(new List<Stack>() { new Stack(item, count) });
                }
            }

            catch (Exception) { }          
        }
    }
}
