using ConsoleAdventure.Content.Scripts.MaterialLogic;
using ConsoleAdventure.WorldEngine;
using System;

namespace ConsoleAdventure.Content.Scripts.MaterialTypes
{
    public class StoneType : MaterialType
    {
        public static Func<Transform, Item, string> GetModifySymbol(string stoneItem, string wallItem, string floorItem, string wall, string[] floor)
        {
            return (transform, item) =>
            {
                if (item != null)
                {
                    Type type = item.GetType();

                    if (type == typeof(StoneItem))
                        return stoneItem;

                    else if (type == typeof(WallItem))
                        return wallItem;

                    else if (type == typeof(FloorItem))
                        return floorItem;
                }

                if (transform != null)
                {
                    short type = transform.type;

                    switch (type)
                    {
                        case (short)VanillaTransforms.wall:
                            return wall;

                        case (short)VanillaTransforms.floor:
                            return transform.GetVariation(floor);
                    }
                }

                return null;
            };
        }
    }
}
