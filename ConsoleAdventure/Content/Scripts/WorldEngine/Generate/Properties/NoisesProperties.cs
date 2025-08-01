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

            tags["ForestNoise"] = new NoiseBuffer(world.seed, 4, 0.5, 3.8);
            tags["ForestScale"] = 0.0005f;

            tags["EdgeNoise"] = new NoiseBuffer(world.seed + 1, 4, 0.5, 3.8);
            tags["EdgeScale"] = 0.0005f;

            tags["BrownIronOreNoise"] = new NoiseBuffer(world.seed, 4, 0.5, 2.7);
            tags["BrownIronOreScale"] = 0.005f;

            tags["CavernNoise"] = new NoiseBuffer(world.seed, 3, 0.45, 1.8);
            tags["CavernScale"] = 0.05f;

            tags["LavaCavernNoise"] = new NoiseBuffer(world.seed + 1, 3, 0.45, 1.8);
            tags["LavaCavernScale"] = 0.05f;

            processProgress = 0;
            processHint = Localization.GetTranslation("Progress", "DeterminationHeights");
            processProgress = 100;

            tags["PlateBoundary"] = Generator.GenRand.NextFloat(-0.3f, -0.4f);

            float seaLevel = Generator.GenRand.NextFloat(-0.25f, -0.18f);
            tags["SeaLevel"] = seaLevel;
            tags["BeachLevel"] = Generator.GenRand.NextFloat(seaLevel + 0.005f, seaLevel + 0.05f);
            tags["EdgeLevel"] = Generator.GenRand.NextFloat(0.1f, 0.3f);
            tags["BrownIronOreLevel"] = Generator.GenRand.NextFloat(0.45f, 0.55f);

            tags["CavernLevel"] = -0.1f;
            tags["LavaCavernLevel"] = -0.1f;

            tags["CavernTunnels"] = new WalkerBuffer(new Range(1, 10),  //walkers
                                                     new Range(15, 46), //steps
                                                     new Range(20, 45), //angles
                                                     new Range(2, 4),   //sizes
                                                     new Range(2, 15),  //size limits
                                                     100);              //resize chance
        }
    }
}
