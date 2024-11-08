using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.Entities;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Content.Scripts.Player;
using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine;

public static class Spawner
{
    private static List<SpawnCondition> SpawnConditions = new List<SpawnCondition>();

    public static void Spawn(Entity entity, bool net = false)
    {
        ConsoleAdventure.world.entities.Add(entity);
        if (!net) return; //NetworkManager.Id != 0 && 
        entity.SetNetID();
        SpawnSync(entity);
    }

    public static Entity SpawnClone(Entity entity)
    {
        Entity spawnEntity = entity.Copy<Entity>();
        Spawn(spawnEntity);

        return spawnEntity;
    }

    private static void SpawnSync(Entity spawnEntity)
    {
        List<byte> data = new List<byte>();

        data.AddRange(BitConverter.GetBytes((short)spawnEntity.netID)); // 2
        data.AddRange(BitConverter.GetBytes(spawnEntity.position.x)); // 2 4
        data.AddRange(BitConverter.GetBytes(spawnEntity.position.y)); // 2 6
        data.Add(spawnEntity.w); // 1 7
        data.Add(spawnEntity.type); //1 8

        NetworkManager.SendMessage(NetworkManager.ActionID.entitySpawned, data.ToArray(), new byte[0]);
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