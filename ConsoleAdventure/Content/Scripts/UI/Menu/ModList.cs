using ConsoleAdventure.Content.Scripts.InputLogic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI
{
    internal class ModList : ListUI
    {

        public ModList(string text, Vector2 position, List<BaseUI> elements, Color color) : base(text, position, elements, color)
        {
            height = 34 * 3;
        }

        public override async void ElementHandleInput(BaseUI element, int timer)
        {
            ModPanel modPanel = element as ModPanel;

            if (Input.OnClick(InputConfig.NavigationSelect) && timer >= 5)
            {
                if (modPanel.cursor == 0)
                {
                    if (!ConsoleAdventure.menu.onlineMods)
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
                    else
                    {
                        // Seting up the http client used to download the data
                        using (var client = new HttpClient())
                        {
                            //client.Timeout = TimeSpan.FromMinutes(5);

                            await CaModLoader.DownloadMod(modPanel.mod.dirName);
                        }
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
