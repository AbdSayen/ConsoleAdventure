using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleAdventure.Content.Scripts.Player;
using System.Threading.Tasks;
using ConsoleAdventure.Content.Scripts.MaterialLogic;

namespace ConsoleAdventure
{
    public class Recipe
    {
        public List<Ingredient> Ingredients { get; private set; } = new();

        public List<int> CraftStations { get; private set; } = new List<int>();

        public Stack OutItem { get; private set; }

        public int InheritedMaterial { get; private set; } = -1;

        public string IDName { get; set; }

        private static int width = 3;
        private static int height = 3;

        public Recipe(Stack stack)
        {
            OutItem = stack;
        }

        public void AddIngredient(Item item, bool inheritedMaterial)
        {
            AddIngredient(item, 1, null, inheritedMaterial);
        }

        public void AddIngredient(Item item, int count, bool inheritedMaterial)
        {
            AddIngredient(item, count, null, inheritedMaterial);
        }

        public void AddIngredient(Item item, int count = 1, string materialName = null, bool inheritedMaterial = false)
        {
            Material material = null;

            Material tryMaterial = MaterialSystem.GetMaterial(materialName);
            if (tryMaterial != null && item.CanApplyMaterial(tryMaterial))
                material = tryMaterial;

            Ingredients.Add(new Ingredient(item, count, material, null));

            if (inheritedMaterial)
                InheritedMaterial = Ingredients.Count - 1;
        }

        public void AddIngredient(Item item, List<MaterialType> materialTypes, int count = 1, bool inheritedMaterial = false)
        {
            Ingredients.Add(new Ingredient(item, count, null, materialTypes));

            if (inheritedMaterial)
                InheritedMaterial = Ingredients.Count - 1;
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

            for (int i = 0; i < Ingredients.Count; i++)
            {
                Ingredient item = Ingredients[i];

                List<MaterialType> materialTypes = item.AvailableMaterialTypes;
                Material material = item.AvailableMaterial;

                if (item.AvailableMaterialTypes != null)
                {
                    if (!player.inventory.HasItems(item.Item, item.Count, materialTypes))
                    {
                        return false;
                    }
                }

                else if (item.AvailableMaterial != null)
                {
                    if (!player.inventory.HasItems(item.Item, item.Count, material.Type))
                    {
                        return false;
                    }
                }

                else
                {
                    if (!player.inventory.HasItems(item.Item, item.Count))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool IsExistItemMaterials()
        {
            Player player = ConsoleAdventure.world.GetLocalPlayer();

            for (int i = 0; i < Ingredients.Count; i++)
            {
                Ingredient item = Ingredients[i];

                if (!player.inventory.HasItems(item.Item, item.Count, item.Item.material))
                    return false;
            }

            return true;
        }

        public Recipe Copy()
        {
            Recipe copy = (Recipe)MemberwiseClone();
            copy.Ingredients = Ingredients.ToArray().ToList();
            copy.CraftStations = CraftStations.ToArray().ToList();
            copy.OutItem = OutItem.Copy();

            for (int i = 0; i < Ingredients.Count; i++)
            {
                Ingredient ingredient = Ingredients[i];
                copy.Ingredients[i] = ingredient.Copy();
            }

            return copy;
        }
    }

    public struct Ingredient
    {
        public Item Item { get; set; }

        public int Count { get; set; } = 1;

        public Material AvailableMaterial { get; set; } = null;

        public List<MaterialType> AvailableMaterialTypes { get; set; } = null;

        public Ingredient(Item item, int count, Material availableMaterial, List<MaterialType> availableMaterialTypes)
        {
            Item = item;
            Count = count;
            AvailableMaterial = availableMaterial;
            AvailableMaterialTypes = availableMaterialTypes;
        }

        public Ingredient Copy()
        {
            Ingredient copy = (Ingredient)MemberwiseClone();
            
            copy.Item = Item.Copy();
            copy.AvailableMaterial = AvailableMaterial;
            copy.AvailableMaterialTypes = AvailableMaterialTypes;

            return copy;
        }
    }
}
