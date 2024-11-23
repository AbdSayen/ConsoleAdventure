using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.Entities;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Content.Scripts.Player;
using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using System.Data;

namespace ConsoleAdventure.WorldEngine;

public static class Spawner
{
    private static List<SpawnCondition> SpawnConditions = new List<SpawnCondition>();

    private static ushort spawnRate = 5;

    private static int minX = 30;
    private static int maxX = 40;
    private static int minY = 15;
    private static int maxY = 25;

    public static void Spawn(Entity entity, bool net = false)
    {
        ConsoleAdventure.world.entities.Add(entity);
        if (!net) return;
        if (NetworkManager.Id != 0)
            return; //NetworkManager.Id != 0 && 
        entity.SetNetID();
        SpawnSync(entity);
    }

    public static Entity SpawnClone(Entity entity)
    {
        Entity spawnEntity = entity.Copy<Entity>();
        Spawn(spawnEntity);

        return spawnEntity;
    }

    public static void AddSpawnCondition(SpawnCondition condition)
    {
        if (!SpawnConditions.Contains(condition))
        {
            SpawnConditions.Add(condition);
        }
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
            if (SpawnConditions[i].wRange.Start.Value <= player.w && SpawnConditions[i].wRange.End.Value >= player.w)
            {
                candidates.Add(SpawnConditions[i]);
            }
        }

        int curIndex = -1;

        double totalProbability = 0;
        foreach (var candidate in candidates)
        {
            totalProbability += candidate.Probability;
        }

        double randomValue = ConsoleAdventure.rand.NextDouble() * totalProbability;

        double cumulative = 0;
        for (int i = 0; i < candidates.Count; i++)
        {
            cumulative += candidates[i].Probability;
            if (randomValue < cumulative)
            {
                curIndex = i;
                break;
            }
        }

        if (curIndex > -1)
        {
            if (Transform.TypeMapping.TryGetValue(candidates[curIndex].MobType, out Type type))
            {
                Position playerPos = player.position;
                Position position = new();
                bool positionFoundFlag = false;

                for (int i = 0; i < 30; i++)
                {
                    position = new(ConsoleAdventure.rand.Next(playerPos.x - maxX, playerPos.x + maxX), ConsoleAdventure.rand.Next(playerPos.y - maxY, playerPos.y + maxY));
                    Field f1 = ConsoleAdventure.world.GetField(position.x, position.y, World.BlocksLayerId, player.w);
                    Field f2 = ConsoleAdventure.world.GetField(position.x, position.y, World.MobsLayerId, player.w);

                    if ((position.x < playerPos.x - minX || position.x > playerPos.x + minX) && (position.y < playerPos.y - minY || position.y > playerPos.y + minY) && f1 != null && f2 != null && f1.content?.isObstacle == false && f2.content == null)
                    {
                        positionFoundFlag = true;
                        break;
                    }
                }

                if (!positionFoundFlag) return;

                Spawn((Entity)Activator.CreateInstance(type, new object[3] { position, player.w, null}));
            }
        }
    }

    static int timer;
    public static bool Update()
    {
        bool flag = false;
        if(timer % spawnRate == spawnRate - 1)
        {
            SpawnSuitableMob(ConsoleAdventure.world.GetLocalPlayer());
            flag = true;
        }

        timer++;
        return flag;
    }
}