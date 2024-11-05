using ConsoleAdventure.Content.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.CaModLoaderAPI
{
    public interface IMod
    {
        /// <summary>
        ///  Название папки мода
        /// </summary>
        public string dirName { get; set; }

        /// <summary>
        ///  Название мода
        /// </summary>
        public string modName { get; set; }

        /// <summary>
        /// Версия мода
        /// </summary>
        public string modVersion { get; set; }

        /// <summary>
        /// Описание мода
        /// </summary>
        public string modDescription { get; set; }

        /// <summary>
        /// Автор мода
        /// </summary>
        public string modAuthor { get; set; }

        /// <summary>
        /// Иконка мода
        /// </summary>
        public CharTexture modIcon { get; set; }
    }
}
