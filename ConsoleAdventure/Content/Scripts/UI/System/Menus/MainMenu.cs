using ConsoleAdventure.Content.Scripts.UI.System.Containers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI.System.Menus
{
    public class MainMenu : UIGroup
    {
        private MenuState State = MenuState.mainScreen;

        public MainMenu() : base(new(0, 0))
        {
            string[] randomItemData = ConsoleAdventure.GetRandomPrettyItem();
            string randomItem = randomItemData[0];
            ConsoleAdventure.logger.AddMessage($"Random item today is {randomItemData[1]}");

            AddElement(new UIText(TextAssets.logo, Color.White, new Point(1920 / 2, 20), anchor: Anchor.Top));
            AddElement(new UIText(new FormatString($" [color:999999=\"T\"][color:dddddd=\"h\"][color:999999=\"e\"] [color:999999=\"Co\"][color:dddddd=\"nso\"][color:999999=\"le\"] [color:999999=\"Ad\"][color:dddddd=\"ventu\"][color:999999=\"re\"] [item:{randomItem}]"), Color.White, new Point(0, 0), anchor: Anchor.TopLeft));

            VListContainer container = new VListContainer(new(1920 / 2, 220), new(300, 20), new(), Anchor.Top);
            HListContainer container2 = new HListContainer(new(1920 / 2, 19 * 4 + 9 * 30), new(0, 9), new(), Anchor.Top, offsetMode: true, limit: 6);
            container2.onBackButtonPressed = (UIContainer cnt) =>
            {
                cnt.Hide();
                cnt.OnDefocus();
                container.OnFocus();
                State = MenuState.mainScreen;
            };

            for (int i = 0; i < 100; i++)
            {
                container2.AddElement(new UIWorldPanel($"World #{i}", $"12{i}", new()));
            }

            AddElement(container2);
            container2.Hide();

            ((UIButton)container.AddElement(new UIButton(Localization.GetTranslation("UI", "Play"), Color.White, Color.Yellow, Point.Zero, anchor: Anchor.Top))).onClick = (UIButton btn) => {
                State = MenuState.worldMenu;
                ConsoleAdventure.display = new Display(ConsoleAdventure.world);
                container2.Show();
                container2.OnFocus();
                container.OnDefocus();
            };
            ((UIButton)container.AddElement(new UIButton(Localization.GetTranslation("UI", "Settings"), Color.White, Color.Yellow, Point.Zero, anchor: Anchor.Top))).onClick = (UIButton btn) => {
                State = MenuState.settings;
            };
            ((UIButton)container.AddElement(new UIButton(Localization.GetTranslation("UI", "Mods"), Color.White, Color.Yellow, Point.Zero, anchor: Anchor.Top))).onClick = (UIButton btn) => {
                State = MenuState.mods;
            };
            ((UIButton)container.AddElement(new UIButton(Localization.GetTranslation("UI", "Exit"), Color.White, Color.Yellow, Point.Zero, anchor: Anchor.Top))).onClick = (UIButton btn) => {
                ConsoleAdventure.isExit = true;
            };
            container.OnFocus();
            AddElement(container);
        }
    }
}
