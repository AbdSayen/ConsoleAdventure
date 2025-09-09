using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Content.Scripts.UI.System.Containers;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure.Content.Scripts.UI.System.Menus
{
    public class MainMenu : UIGroup
    {
        public MenuState State = MenuState.mainScreen;

        public void UpdateWorldList(UIContainer worldPanelsContainer)
        {
            var worlds = WorldIO.GetWorlds();

            for (int i = 0; i < worlds.names.Length; i++)
            {
                worldPanelsContainer.AddElement(new UIWorldPanel(worlds.names[i], worlds.seeds[i].ToString(), new()));
            }
        }

        public MainMenu() : base(new(0, 0))
        {
            SetName("MainMenu");

            string[] randomItemData = ConsoleAdventure.GetRandomPrettyItem();
            string randomItem = randomItemData[0];
            ConsoleAdventure.logger.AddMessage($"Random item today is {randomItemData[1]}");

            AddElement(new UIText(TextAssets.logo, Color.White, new Point(1920 / 2, 20), anchor: Anchor.Top));
            AddElement(new UIText(new FormatString($" [color:999999=\"T\"][color:dddddd=\"h\"][color:999999=\"e\"] [color:999999=\"Co\"][color:dddddd=\"nso\"][color:999999=\"le\"] [color:999999=\"Ad\"][color:dddddd=\"ventu\"][color:999999=\"re\"] [item:{randomItem}]"), Color.White, new Point(0, 0), anchor: Anchor.TopLeft));

            VListContainer mainButtonsContainer = new VListContainer(new(1920 / 2, 220), new(300, 20), Anchor.Top);
            HListContainer worldPanelsContainer = new HListContainer(new(1920 / 2, 19 * 4 + 9 * 30), new(0, 9), Anchor.Top, offsetMode: true, limit: 6);
            worldPanelsContainer.onBackButtonPressed = (UIContainer c) =>
            {
                c.Hide();
                c.OnDefocus();
                mainButtonsContainer.OnFocus();
                State = MenuState.mainScreen;
            };

            UpdateWorldList(worldPanelsContainer);

            AddElement(worldPanelsContainer.SetName("WorldPanelsContainer"));
            worldPanelsContainer.Hide();

            ((UIButton)mainButtonsContainer.AddElement(new UIButton(Localization.GetTranslation("UI", "Play"), Color.White, Color.Yellow, Point.Zero, anchor: Anchor.Top))).onClick = (UIButton btn) => {
                State = MenuState.worldMenu;
                ConsoleAdventure.display = new Display(ConsoleAdventure.world);
                worldPanelsContainer.Show();
                worldPanelsContainer.OnFocus();
                mainButtonsContainer.OnDefocus();
            };
            ((UIButton)mainButtonsContainer.AddElement(new UIButton(Localization.GetTranslation("UI", "Settings"), Color.White, Color.Yellow, Point.Zero, anchor: Anchor.Top))).onClick = (UIButton btn) => {
                State = MenuState.settings;
            };
            ((UIButton)mainButtonsContainer.AddElement(new UIButton(Localization.GetTranslation("UI", "Mods"), Color.White, Color.Yellow, Point.Zero, anchor: Anchor.Top))).onClick = (UIButton btn) => {
                State = MenuState.mods;
            };
            ((UIButton)mainButtonsContainer.AddElement(new UIButton(Localization.GetTranslation("UI", "Exit"), Color.White, Color.Yellow, Point.Zero, anchor: Anchor.Top))).onClick = (UIButton btn) => {
                ConsoleAdventure.isExit = true;
            };
            mainButtonsContainer.OnFocus();
            AddElement(mainButtonsContainer.SetName("MainButtonsContainer"));

            ConsoleAdventure.progressBar = (UIProgressBar)AddElement(new UIProgressBar(50, new(1920 / 2, 1080/2 - 120), Anchor.Top));
            ConsoleAdventure.progressBar.Hide();
        }
    }
}
