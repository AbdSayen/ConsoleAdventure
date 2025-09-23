using ConsoleAdventure.Content.Scripts.InputLogic;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Content.Scripts.UI.System.Containers;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure.Content.Scripts.UI.System.Menus
{
    public class MainMenu : UIGroup
    {
        public MenuState State = MenuState.mainScreen;

        private VListContainer mainButtonsContainer;

        public void UpdateWorldList(UIContainer worldPanelsContainer)
        {
            worldPanelsContainer.ClearChilds();

            var worlds = WorldIO.GetWorlds();

            for (int i = 0; i < worlds.names.Length; i++)
            {
                worldPanelsContainer.AddElement(new UIWorldPanel(worlds.names[i], worlds.seeds[i].ToString(), new()));
            }

            if (worlds.names.Length == 0)
            {
                worldPanelsContainer.AddElement(new UIText("There is no worlds yet :(\nCreate new own world by pressing [color:cfbfff=\"N\"] key", Color.Gray, new(), align: Align.Center, anchor: Anchor.Top));
            }

            worldPanelsContainer.AddElement(new UITextInputField(Color.White, new(1920 / 2, 300), 15, "Test Field, enjoy)", UITextInputField.VerticalStickCursor, chars: new char[] { '\n', '\r' }, listType: TextInput.BlackList, anchor: Anchor.Top, isAutoSized: true));
        }

        public void BackToMainMenu(UIContainer currentMenu)
        {
            currentMenu.Hide();
            currentMenu.OnDefocus();
            mainButtonsContainer.OnFocus();
            State = MenuState.mainScreen;
        }

        public MainMenu() : base(new(0, 0))
        {
            SetName("MainMenu");

            string[] randomItemData = ConsoleAdventure.GetRandomPrettyItem();
            string randomItem = randomItemData[0];
            ConsoleAdventure.logger.AddMessage($"Random item today is {randomItemData[1]}");

            AddElement(new UIText(TextAssets.logo, Color.White, new Point(1920 / 2, 20), anchor: Anchor.Top));
            AddElement(new UIText(new FormatString($" [color:999999=\"T\"][color:dddddd=\"h\"][color:999999=\"e\"] [color:999999=\"Co\"][color:dddddd=\"nso\"][color:999999=\"le\"] [color:999999=\"Ad\"][color:dddddd=\"ventu\"][color:999999=\"re\"] [item:{randomItem}]"), Color.White, new Point(0, 0), anchor: Anchor.TopLeft));

            mainButtonsContainer = new VListContainer(new(1920 / 2, 220), new(300, 20), Anchor.Top);

            #region world container
            WorldPanelsContainer worldPanelsContainer = new WorldPanelsContainer(new(1920 / 2, 19 * 4 + 9 * 30));
            worldPanelsContainer.onBackButtonPressed += (UIContainer c) => BackToMainMenu(c);

            UpdateWorldList(worldPanelsContainer);

            worldPanelsContainer.Hide();
            AddElement(worldPanelsContainer.SetName("WorldPanelsContainer"));
            #endregion

            #region world gen
            WorldGenContainer worldGenContainer = new WorldGenContainer();
            worldGenContainer.onBackButtonPressed += (UIContainer c) =>
            {
                BackToMainMenu(c);
            };

            worldGenContainer.AddElement(new UITextInputField(Color.White, new(), 30, Localization.GetTranslation("UI", "WorldNameField"),       chars: new char[] { '\\', '|', '/', '<', '>', '*', '"', '?', '\n', '\r' }, listType: TextInput.BlackList, anchor: Anchor.TopLeft).SetName("WorldNameField"));
            worldGenContainer.AddElement(new UITextInputField(Color.White, new(), 30, Localization.GetTranslation("UI", "WorldSeedField"),       chars: new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' },    listType: TextInput.WhiteList, anchor: Anchor.TopLeft).SetName("WorldSeedField"));
            worldGenContainer.AddElement(new UITextInputField(Color.White, new(), 30, Localization.GetTranslation("UI", "WorldPlayerNameField"), chars: new char[] { '\\', '|', '/', '<', '>', '*', '"', '?', '\n', '\r' }, listType: TextInput.BlackList, anchor: Anchor.TopLeft).SetName("WorldPlayerNameField"));

            worldGenContainer.Hide();
            AddElement(worldGenContainer.SetName("WorldGenContainer"));
            #endregion

            // Main Buttons Creation
            ((UIButton)mainButtonsContainer.AddElement(new UIButton(Localization.GetTranslation("UI", "Play"), Color.White, Color.Yellow, Point.Zero, anchor: Anchor.Top))).onClick = (UIButton btn) => {
                State = MenuState.worldMenu;
                ConsoleAdventure.display = new Display(ConsoleAdventure.world);
                mainButtonsContainer.OnDefocus();

                worldPanelsContainer.Show();
                worldPanelsContainer.OnFocus();
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
