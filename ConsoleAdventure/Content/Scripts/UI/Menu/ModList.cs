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

        public override void ElementHandleInput(BaseUI element, ref int timer)
        {
            ModPanel modPanel = element as ModPanel;

            if (Input.OnClick(InputConfig.NavigationSelect) && timer >= 5)
            {
                if (modPanel.cursor == 0)
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
                    timer = 0;
                }

                else if (modPanel.cursor == 1)
                {
                    ConsoleAdventure.menu.OpenModDescription(modPanel.mod.modDescription);
                    timer = 0;
                }
            }

            if (isHover)
            {
                if (Input.OnClick(InputConfig.NavigationRight))
                {
                    for (int i = 0; i < elements.Count; i++)
                    {
                        ((ModPanel)elements[i]).cursor = 1;
                    }
                }                
                
                if (Input.OnClick(InputConfig.NavigationLeft))
                {
                    for (int i = 0; i < elements.Count; i++)
                    {
                        ((ModPanel)elements[i]).cursor = 0;
                    }
                }
            }
        }
    }
}
