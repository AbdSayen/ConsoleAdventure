using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleAdventure.WorldEngine.Levels;
using System.Threading.Tasks;
using CaModLoaderAPI;

namespace ConsoleAdventure.WorldEngine
{
    public class WorldLevelsSystem
    {
        public List<WorldLevel> Levels { get; private set; }

        public WorldLevelsSystem(List<WorldLevel> levels) 
        {
            Chunk.maxDeep = 1;

            if (levels == null)
            {
                Levels = new List<WorldLevel>
                {
                    //new LavaCavern(),
                    new Cavern(),
                    //new Sedimentary(),
                    new Surface()
                };

                foreach (Mod mod in CaModLoader.GetActiveMods())
                {
                    List<WorldLevel> newLevels = mod.ModifyWorldLevels(Levels);

                    if (newLevels != null)
                    {
                        Levels = newLevels;
                    }
                }
            }

            else
            {
                Levels = levels;
            }

            for (int i = 0; i < Levels.Count; i++)
            {
                if (Levels[i] == null) 
                {
                    Levels.RemoveAt(i);
                    i--;
                }
            }

            Chunk.maxDeep = Levels.Count;
        }

        /// <summary>
        /// Метод получение координаты по оси W, используя экземпляр класса уровня. если уровня нет, то вернётся -1
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetLevel(WorldLevel level)
        {
            Type type = level.GetType();

            for (int i = 0; i < Levels.Count; i++)
            {
                if (Levels[i].GetType() == type)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
