using ConsoleAdventure.Content.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.CaModLoaderAPI
{
    internal class EmptyMod : IMod
    {
        /// <summary>
        ///  Название мода
        /// </summary>
        public string modName { get; set; } = "undefined";

        /// <summary>
        /// Версия мода
        /// </summary>
        public string modVersion { get; set; } = "1.0.0";

        /// <summary>
        /// Описание мода
        /// </summary>
        public string modDescription { get; set; } = "No description";

        /// <summary>
        /// Автор мода
        /// </summary>
        public string modAuthor { get; set; } = "Anonymous";

        /// <summary>
        /// Иконка мода
        /// </summary>
        public CharTexture modIcon { get; set; }
    }
}
