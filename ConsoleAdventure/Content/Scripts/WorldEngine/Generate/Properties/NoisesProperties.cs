using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Content.Scripts.WorldEngine;
using ConsoleAdventure.Content.Scripts.WorldEngine.Generate;
using ConsoleAdventure.Generate.Structures;
using ConsoleAdventure.Settings;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine.Generate
{
    public class NoisesProperties : WorldPropertiesGenerator
    {
        public override async Task UpdateWorldProperties(Tags tags, World world)
        {
            await base.UpdateWorldProperties(tags, world);
            processHint = Localization.GetTranslation("Progress", "CreateNoises");
            processProgress = 100;

            tags["LandspaceNoise"] = new NoiseBuffer(world.seed, 6, 0.5, 5);
            tags["LandspaceScale"] = 0.00005f;

            processProgress = 0;
            processHint = Localization.GetTranslation("Progress", "DeterminationHeights");
            processProgress = 100;

            tags["PlateBoundary"] = Generator.GenRand.NextFloat(-0.2f, -0.4f);
            tags["SeaLevel"] = Generator.GenRand.NextFloat(-0.05f, 0.15f);
        }
    }
}
