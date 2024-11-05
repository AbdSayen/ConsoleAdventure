using ConsoleAdventure.Content.Scripts.InputLogic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI
{
    internal class ModList : ListUI
    {
        public ModList(string text, Vector2 position, List<BaseUI> elements, Color color) : base(text, position, elements, color)
        {
            height = 34 * 3;
        }

        public override void ElementHandleInput(BaseUI element)
        {
            ModPanel modPanel = element as ModPanel;

            if (Input.OnClick(InputConfig.ModToggle))
            {
                modPanel.enabled = !modPanel.enabled;

                if (modPanel.enabled)
                {
                    CaModLoader.EnableMod(modPanel.mod.dirName);
                }
                else
                {
                    CaModLoader.DisableMod(modPanel.mod.dirName);
                }
            }
        }
    }
}
