using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.CaModLoaderAPI
{
    public class GlobalTransform
    {
        public virtual void SetStaticData() { }

        public virtual string ModifyTooltip(Transform transform) => null;

        public virtual bool PreCollapse(Transform transform) => true;

        public virtual bool PreInteraction(Transform transform) => true;
    }
}
