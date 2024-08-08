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
        entity,
        cat,
        chest,
        grass,
        
        stone,                  //+
        granite,                //+
        quartz,                 //+
        feldspar,
        graniteFloor,           //+
        feldsparFloor,
        quartzite,
        redQuartzite,
        yellowQuartzite,
        quartziteFloor,
        dirt,
        dirtFloor,
        sand,
        sandFloor,
        sandstone,
        clay,
        clayFloor,
        brownIronOre,
        basalt,
        basaltFloor,
        obsidian,
        obsidianFloor,
        slate,
        onyx,
        amethyst,
        fossil,
        marble,
        marbleFloor,
        chalk,
        chalkFloor,
        moss,
        mossFloor,
        redMoss,
        redMossFloor,
        fungus,
        fungusFloor,
        descent,
        climb,
    }
}