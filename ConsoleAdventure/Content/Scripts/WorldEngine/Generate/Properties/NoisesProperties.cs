using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Content.Scripts.WorldEngine;
using ConsoleAdventure.Content.Scripts.WorldEngine.Generate;
using ConsoleAdventure.Generate.Structures;
using ConsoleAdventure.Settings;
using Microsoft.Xna.Framework;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Drawing;
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

            NoiseBuffer landspaceNoise = new NoiseBuffer(world.seed, 6, 0.5, 3.8);
            float landspaceScale = 0.00005f;
            tags["LandspaceNoise"] = landspaceNoise;
            tags["LandspaceScale"] = landspaceScale;

            tags["ClimaticNoise"] = new SineNoise(world.size / Chunk.Size, landspaceScale, 0.3f, 2f, 1f, 1f, landspaceNoise);

            tags["ForestNoise"] = new NoiseBuffer(world.seed, 6, 0.5, 3.8);
            tags["ForestScale"] = 0.0005f;


            processProgress = 0;
            processHint = Localization.GetTranslation("Progress", "DeterminationHeights");
            processProgress = 100;

            tags["PlateBoundary"] = Generator.GenRand.NextFloat(-0.2f, -0.4f);

            float seaLevel = Generator.GenRand.NextFloat(-0.05f, 0.15f);
            tags["SeaLevel"] = seaLevel;
            tags["BeachLevel"] = Generator.GenRand.NextFloat(seaLevel + 0.005f, seaLevel + 0.05f);
        }
    }
}
