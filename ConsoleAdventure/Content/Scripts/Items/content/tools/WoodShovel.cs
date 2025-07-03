using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Content.Scripts.Player;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class WoodShovel : Item
    {
        public WoodShovel()
        {
            name = Localization.GetTranslation("Items", "WoodShovel");
            description = GetDescription();
            maxCount = 1;
            canUse = true;
            AddTypeToMap<WoodShovel>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("/", new(94, 61, 38)).AddLayer("▪", Color.Black, new Vector2(4, -6)).AddLayer("⌂", new(94, 61, 38), new Vector2(5, -6), 0.35f);
        }

        public override Recipe AddRecipe()
        {
            Recipe recipe = new Recipe(new Stack(this, 1));
            recipe.AddIngredient(new Log(), 2);
            recipe.AddStation((int)VanillaTransforms.workbench);

            return recipe;
        }

        public override void UseItem()
        {
            if (Cursor.Instance.IsActive)
            {
                Player player = ConsoleAdventure.world.GetLocalPlayer();
                Position position = Cursor.Instance.CursorPosition + player.position;
                int w = player.w;

                if (w > 0)
                {
                    Field field0 = ConsoleAdventure.world.GetField(position, World.BlocksLayerId, w);
                    Field field1 = ConsoleAdventure.world.GetField(position, World.BlocksLayerId, w - 1);

                    Transform transform1 = field1?.content;

                    if (field0?.content == null && transform1 != null && transform1?.hardness <= 0.5f && transform1?.hardness > 0)
                    {
                        transform1.degreeDestruction += (byte)Math.Abs(10f / transform1.hardness);
                        if (transform1.degreeDestruction >= 100)
                        {
                            ConsoleAdventure.world.RemoveSubject(transform1, World.BlocksLayerId);

                            new Descent(position, w);
                            new Climb(position, w - 1);
                        }
                    }
                }
            }
        }
    }
}