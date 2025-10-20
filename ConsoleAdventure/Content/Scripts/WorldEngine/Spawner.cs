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

    private static ushort spawnRate = 180;

    private static int minX = 30;
    private static int maxX = 40;
    private static int minY = 15;
    private static int maxY = 25;

    public static void Spawn(Entity entity, bool net = false)
    {
        ConsoleAdventure.world.entities.Add(entity);
        entity.PreStart();
        if (NetworkManager.isHost && ConsoleAdventure.InWorld)
        {
            NetworkManager.EntitySpawned(entity);
        }
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
            int id = candidates[curIndex].MobType;

            if (id >= 0 && id < Transform.TypeMapping.Length)
            {
                Type type = Transform.TypeMapping[id];

                if (type != null)
                {
                    Position playerPos = player.position;
                    Position position = new();
                    bool positionFoundFlag = false;

                    for (int i = 0; i < 30; i++)
                    {
                        position = new(ConsoleAdventure.rand.Next(playerPos.x - maxX, playerPos.x + maxX), ConsoleAdventure.rand.Next(playerPos.y - maxY, playerPos.y + maxY));
                        Field f1 = ConsoleAdventure.world.GetField(position.x, position.y, World.BlocksLayerId, player.w);
                        Field f2 = ConsoleAdventure.world.GetField(position.x, position.y, World.MobsLayerId, player.w);

                        if ((position.x < playerPos.x - minX || position.x > playerPos.x + minX) && (position.y < playerPos.y - minY || position.y > playerPos.y + minY) && f1 != null && f2 != null && Transform.IsObstacle[f1.content] && f2.content == null)
                        {
                            positionFoundFlag = true;
                            break;
                        }
                    }

                    if (!positionFoundFlag) return;

                    Spawn((Entity)Activator.CreateInstance(type, new object[3] { position, player.w, null }));
                }
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