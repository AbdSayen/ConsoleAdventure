using System;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public enum RenderFieldType { 
        empty,
        player,
        wall,
        loot,
        tree,
        floor,
        door,
        ruine,
        water,
        log,
    }
}