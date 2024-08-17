using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleAdventure.Content.Scripts.Player;
using System.Threading.Tasks;

namespace ConsoleAdventure
{
    public class Recipe
    {
        public Dictionary<Item, int> Ingredients { get; private set; } = new Dictionary<Item, int>();

        public List<int> CraftStations { get; private set; } = new List<int>();

        public Stack OutItem { get; private set; }

        private static int width = 3;
        private static int height = 3;

        public Recipe(Stack stack)
        {
            OutItem = stack;
        }

        public void AddIngredient(Item item, int count = 1)
        {
            Ingredients.Add(item, count);
        }

        public void AddStation(int station)
        {
            CraftStations.Add(station);
        }

        public bool IsAvailable()
        {
            Player player = ConsoleAdventure.world.GetLocalPlayer();

            for (int i = 0; i < CraftStations.Count; i++)
            {
                bool isFound = false;

                if (CraftStations.Count == 0)
                    isFound = true;

                else
                {
                    for (int j = 0; j < width; j++)
                    {
                        for (int k = 0; k < height; k++)
                        {
                            if (ConsoleAdventure.world.GetField(player.position.x - 1 + j, player.position.y - 1 + k, WorldEngine.World.BlocksLayerId, player.w)?.content?.type == CraftStations[i])
                            {
                                isFound = true;
                            }
                        }
                    }
                }

                if(!isFound)
                {
                    return false;
                }
            }

            for(int i = 0; i < Ingredients.Count; i++)
            {
                var items = Ingredients.ElementAt(i);
                if (!player.inventory.HasItems(items.Key, items.Value))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
