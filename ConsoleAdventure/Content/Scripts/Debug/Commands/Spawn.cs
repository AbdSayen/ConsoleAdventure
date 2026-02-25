using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.Debug.Commands
{
    public class Spawn: Command
    {
        public Spawn() 
        {
            Name = "spawn";
            Description = "spawn entity";
            Arguments = new List<string>()
            {
                "name",
            };
        }

        public override void Logic(string[] args, short id = -2)
        {
            try
            {
                string name = GetStringArg(args, "name");
                string namespace_ = "ConsoleAdventure.Content.Scripts";

                try
                {
                    Type[] type = new Type[2] 
                    { 
                        CaModLoader.GetTypeFromAnyMod(namespace_ + "." + name),
                        CaModLoader.GetTypeFromAnyMod(name),
                    };

                    for (int i = 0; i < type.Length; i++)
                    {
                        if (type[i] != null && type[i].IsSubclassOf(typeof(Entity)))
                        {
                            if (id == -2) id = NetworkManager.Id;
                            if (id == -1) id = 0;
                            Player.Player pl = ConsoleAdventure.world.players[id];
                            Entity entity = (Entity)Activator.CreateInstance(type[i], new object[] { pl.position + new Position(0, 1), pl.w, null });
                            Spawner.Spawn(entity);

                            break;
                        }
                    }
                }

                catch (Exception) 
                { 
                
                }
            }
            catch 
            {
                Loger.AddLog("Неверный формат аргумента!");
            }
        }
    }
}
