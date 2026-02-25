using ConsoleAdventure.Content.Scripts.MaterialLogic;
using ConsoleAdventure.WorldEngine;
using System;

namespace ConsoleAdventure.Content.Scripts.MaterialTypes
{
    public class WoodType : MaterialType
    {
        public static Func<Transform, Item, string> GetModifySymbol(string logItem, string floorItem, string plank, string[] floor)
        {
            return (transform, item) =>
            {
                if (item != null)
                {
                    Type type = item.GetType();

                    if (type == typeof(Log))
                        return logItem;

                    else if (type == typeof(FloorItem))
                        return floorItem;
                }

                if (transform != null)
                {
                    short type = transform.type;

                    switch (type)
                    {
                        case (short)VanillaTransforms.log:
                            return plank;

                        case (short)VanillaTransforms.floor:
                            return transform.GetVariation(floor);
                    }
                }

                return null;
            };
        }
        public Func<Transform, Item, string> GetModifySymbol(string logItem, string floorItem, string plank, string floor)
        {
            return GetModifySymbol(logItem, floorItem, plank, new[] { floor });
        }

        public Func<Transform, Item, string> GetModifySymbol(string plank)
        {
            return GetModifySymbol(null, null, plank, (string[])null);
        }
    }
}
