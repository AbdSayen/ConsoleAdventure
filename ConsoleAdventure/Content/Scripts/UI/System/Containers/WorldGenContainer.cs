using ConsoleAdventure.Content.Scripts.InputLogic;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Content.Scripts.UI.System.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI.System.Containers
{
    public class WorldGenContainer : HListContainer
    {
        public WorldGenContainer() : base(new(1920 / 2, 1080 / 2 - 200), new(9 * 30, 0), Anchor.Top, offsetMode: true)
        {

        }

        public override async void Update()
        {
            base.Update();
            if (Input.PostClick(InputConfig.NavigationSelect))
            {
                string name = ((UITextInputField)GetFirstElementWithName("WorldNameField")).fieldContent;
                string[] worlds = WorldIO.GetWorlds(false).names;

                int countIdenticalWorldName = 0;
                for (int i = 0; i < worlds.Length; i++)
                {
                    string curEndName = (countIdenticalWorldName > 0 ? countIdenticalWorldName.ToString() : "");
                    if (worlds[i] == name + curEndName)
                    {
                        countIdenticalWorldName++;
                    }
                }

                if (countIdenticalWorldName > 0)
                {
                    name += countIdenticalWorldName;
                }

                await ConsoleAdventure.CreateWorld(name, int.Parse(((UITextInputField)GetFirstElementWithName("WorldSeedField")).fieldContent), 25600);
                WorldIO.Save(ConsoleAdventure.world.name);

                WorldPanelsContainer wpc = (WorldPanelsContainer)GetParent().GetFirstElementWithName("WorldPanelsContainer");

                ((MainMenu)GetParent()).UpdateWorldList(wpc);
                wpc.OnFocus();
                wpc.Show();
                Hide();
                OnDefocus();
            }
        }
    }
}
