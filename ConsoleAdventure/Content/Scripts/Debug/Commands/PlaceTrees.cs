using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.Debug.Commands
{
    public class PlaceTrees : Command
    {
        public PlaceTrees() 
        {
            Name = "placetrees";
            Description = "place all trees";
            Arguments = new List<string>()
            {
            };
        }

        public override void Logic(string[] args, short id = -2)
        {
            int chunk = 0;

            Type baseType = typeof(Transform);
            IEnumerable<Type> list = Assembly.GetAssembly(baseType).GetTypes().Where(type => type.IsSubclassOf(baseType)).ToList().Concat(CaModLoader.modTransforms);
            foreach (Type type in list)
            {
                if (type.IsAbstract)
                    continue;

                if (type.IsSubclassOf(typeof(Tree)))
                {
                    Position position = new Position(8, 8) + new Position(chunk * 16, 0);

                    ConsoleAdventure.world.chunks[chunk, 0] = new LoadedChunk();
                    Transform.Init(type, position, ConsoleAdventure.world.Surface, null, null);

                    string text = ConsoleAdventure.world.GetField(position, World.BlocksLayerId, ConsoleAdventure.world.Surface)?.content?.ModifyTooltip() ?? "~*??*~";
                    (new TextMark(position + new Position(0, 5), ConsoleAdventure.world.Surface)).LoadData(text);

                    ConsoleAdventure.world.chunks[chunk, 0].IsUpdated = true;
                    chunk++;
                }
            }
        }
    }
}
