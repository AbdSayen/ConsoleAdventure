using ConsoleAdventure.Content.Scripts.InputLogic;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Content.Scripts.UI.System.Menus;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI.System.Containers
{
    public class WorldGenContainer : HListContainer
    {
        private UIText warnText;

        public WorldGenContainer() : base(new(1920 / 2, 1080 / 2 - 200), new(9 * 30, 0), Anchor.Top, offsetMode: true)
        {
            onBackButtonPressed += (UIContainer c) => { DestroyWarnTextAndResetFields(); };
        }

        private void DestroyWarnTextAndResetFields()
        {
            if (warnText != null) warnText.Destroy();
            foreach (UITextInputField field in GetChilds()) field.ClearContent();
        }

        public override async void Update()
        {
            base.Update();
            if (Input.PostClick(InputConfig.NavigationSelect))
            {
                string name = ((UITextInputField)GetFirstElementWithName("WorldNameField")).fieldContent;
                string seed = ((UITextInputField)GetFirstElementWithName("WorldSeedField")).fieldContent;

                if (name == "" || seed == "")
                {
                    warnText = (UIText)GetParent().GetFirstElementWithName("WarnText");
                    if (warnText == null)
                        warnText = (UIText)GetParent().AddElement(new UIText("", Color.Red, new(1920/2, 600), align: Align.Left).SetName("WarnText"));

                    string msg = "Name or seed doesn't be empty!";
                    string dec = new string('-', msg.Length);
                    warnText.text = new FormatString(dec + '\n' + msg + '\n' + dec);
                    return;
                }
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

                Hide();
                OnDefocus();
                ConsoleAdventure.progressBar.Show();
                await ConsoleAdventure.CreateWorld(name, int.Parse(seed), 25600);
                WorldIO.Save(ConsoleAdventure.world.name);
                ConsoleAdventure.progressBar.Hide();

                DestroyWarnTextAndResetFields();

                WorldPanelsContainer wpc = (WorldPanelsContainer)GetParent().GetFirstElementWithName("WorldPanelsContainer");
                ((MainMenu)GetParent()).UpdateWorldList(wpc);
                wpc.OnFocus();
                wpc.Show();
            }
        }
    }
}
