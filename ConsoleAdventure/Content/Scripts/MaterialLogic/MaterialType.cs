using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.MaterialLogic
{
    public class MaterialType
    {
        /// <summary>
        /// Температура горения материала. Измеряется в кельвинах. 
        /// Если значения <c>null</c>, материал не горит.
        /// </summary>
        public float? BurningPoint { get; set; } = null;

        /// <summary>
        /// Температура плавления материала. Измеряется в кельвинах. 
        /// Если значения <c>null</c>, материал не плавится.
        /// </summary>
        public float? MeltingPoint { get; set; } = null;

        /// <summary>
        /// Температура кипения материала. Измеряется в кельвинах. 
        /// Если значения <c>null</c>, материал не кипит.
        /// </summary>
        public float? BoilingPoint { get; set; } = null;
    }
}
