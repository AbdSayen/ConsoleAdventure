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
        public MainMenu() : base(new(0, 0))
        {
            string[] randomItemData = ConsoleAdventure.GetRandomPrettyItem();
            string randomItem = randomItemData[0];
            ConsoleAdventure.logger.AddMessage($"Random item today is {randomItemData[1]}");

            AddElement(new UIText(TextAssets.logo, Color.White, new Point(1920 / 2, 20), anchor: Anchor.Top));
            AddElement(new UIText(new FormatString($" [color:999999=\"T\"][color:dddddd=\"h\"][color:999999=\"e\"] [color:999999=\"Co\"][color:dddddd=\"nso\"][color:999999=\"le\"] [color:999999=\"Ad\"][color:dddddd=\"ventu\"][color:999999=\"re\"] [item:{randomItem}]"), Color.White, new Point(0, 0), anchor: Anchor.TopLeft));

            UIElement textTest = AddElement(new UIText("Halo :3", Color.White, new Point(1920 / 2, 1080 / 2), anchor: Anchor.Center));
            textTest.Hide();

            VListContainer container = new VListContainer(new(1920 / 2, 220), new(300, 20), new(), Anchor.Top);

            Action<UIButton> buttonAction = (UIButton btn) => { ConsoleAdventure.logger.AddMessage($"Clicked {btn.name}"); if (btn.name == "Играть") { container.Hide(); textTest.Show(); } };

            ((UIButton)container.AddElement(new UIButton(Localization.GetTranslation("UI", "Play"), Color.White, Color.Yellow, Point.Zero, anchor: Anchor.Top))).onClick = buttonAction;
            ((UIButton)container.AddElement(new UIButton(Localization.GetTranslation("UI", "Settings"), Color.White, Color.Yellow, Point.Zero, anchor: Anchor.Top))).onClick = buttonAction;
            ((UIButton)container.AddElement(new UIButton(Localization.GetTranslation("UI", "Mods"), Color.White, Color.Yellow, Point.Zero, anchor: Anchor.Top))).onClick = buttonAction;
            ((UIButton)container.AddElement(new UIButton(Localization.GetTranslation("UI", "Exit"), Color.White, Color.Yellow, Point.Zero, anchor: Anchor.Top))).onClick = buttonAction;
            container.OnFocus();
            AddElement(container);
        }
    }
}
