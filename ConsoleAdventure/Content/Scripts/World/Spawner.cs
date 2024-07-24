using ConsoleAdventure.Content.Scripts.Entities;

namespace ConsoleAdventure.WorldEngine;

public static class Spawner
{
    public static Entity Spawn(Entity entity, Position position = default)
    {
        Entity spawnEntity = new Entity(World.Instance, Position.Zero());
        spawnEntity = (Entity)entity.Copy();
        spawnEntity.Color.ChooseColor(position);
        World.Instance.SetSubjectPosition(spawnEntity, entity.worldLayer, position.x, position.y);

        return spawnEntity;
    }
}