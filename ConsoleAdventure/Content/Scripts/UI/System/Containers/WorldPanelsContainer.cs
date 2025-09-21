using ConsoleAdventure.Content.Scripts.InputLogic;
using ConsoleAdventure.Content.Scripts.UI.System.Menus;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure.Content.Scripts.UI.System.Containers
{
    public class WorldPanelsContainer : HListContainer
    {
        public WorldPanelsContainer(Point position) : base(position, new(0, 9), anchor: Anchor.Top, offsetMode: true, limit: 6)
        {
            
        }

        public override void Update()
        {
            base.Update();

            if (Input.PostClick(InputConfig.WorldGen))
            {
                GetParent().GetFirstElementWithName("MainButtonsContainer").OnFocus();

                Hide();
                OnDefocus();

                ConsoleAdventure.menu.State = MenuState.wordGenMenu;
            }
        }
    }
}
