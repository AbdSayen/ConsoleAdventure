using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.Entities;

namespace ConsoleAdventure.WorldEngine;

public static class Spawner
{
    public static Entity Spawn(Entity entity, int w, Position position = default)
    {
        Entity spawnEntity = new Entity(Position.Zero(), w);
        spawnEntity = entity.Copy<Entity>();
        spawnEntity.EntityColor.ChooseColor(position, w);
        ConsoleAdventure.world.SetSubjectPosition(spawnEntity, entity.worldLayer, position.x, position.y);

        return spawnEntity;
    }
}