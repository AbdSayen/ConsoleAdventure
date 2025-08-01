using ConsoleAdventure.Content.Scripts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class Apple : FoodItem
    {
        public Apple()
        {
            satiety = 1;
            name = Localization.GetTranslation(Localization.Chapters.Items, GetType().Name);
            description = GetDescription();
            canUse = true;
            consume = true;

            AddTypeToMap<Apple>();
        }

        public override CharTexture GetTexture()
        {
            return new CharTexture().AddLayer("o", Color.Red).AddLayer("`", Color.Green);
        }

        public new string GetDescription()
        {
            return " " + Localization.GetTranslation("ItemDescription", GetType().Name) + "\n " + base.GetDescription();
        }

        public override void UseItem()
        {
            Eat();
            new Loot(ConsoleAdventure.world.GetLocalPlayer().position, ConsoleAdventure.world.GetLocalPlayer().w, new List<Stack> { new Stack(new TreeSeed(), ConsoleAdventure.rand.Next(1, 4)) });
        }
    }
}