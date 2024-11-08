using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.Entities;
using ConsoleAdventure.Content.Scripts.Player;
using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine;

public static class Spawner
{
    private static List<SpawnCondition> SpawnConditions = new List<SpawnCondition>();

    public static void SpawnClone(Entity entity)
    {
        Spawn(entity.Copy<Entity>());
    }

    public static void Spawn(Entity entity)
    {
        ConsoleAdventure.world.entities.Add(entity);
    }

    public static void SpawnSuitableMob(Player player)
    {
        List<SpawnCondition> candidates = new();

        for (int i = 0; i < SpawnConditions.Count; i++)
        {
            if (SpawnConditions[i].wRange.Start.Value >= player.w && SpawnConditions[i].wRange.End.Value <= player.w) 
            {
                candidates.Add(SpawnConditions[i]);
            }
        }

        int maxPosobility = -1;
        int curIndex = -1;

        for (int i = 0;i < candidates.Count;i++)
        {
            int oldPosobility = maxPosobility;
            maxPosobility = Math.Max(maxPosobility, candidates[i].Probability);

            if(oldPosobility != maxPosobility)
            {
                curIndex = i;
            }
        }

        if (curIndex > -1)
        {
            Transform.SetObject(curIndex, new(), player.w);
        }
    }
}