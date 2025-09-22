using ConsoleAdventure.Content.Scripts.InputLogic;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Content.Scripts.UI.System.Containers;
using ConsoleAdventure.Content.Scripts.UI.System.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleAdventure.Content.Scripts.UI.System
{
    public class UIWorldPanel : UIElement
    {
        private FormatString worldName;
        private FormatString seedText;

        private string wname;

        private Color cursorColor = Color.White;

        private int cursorPos = 0;

        public UIWorldPanel(string wname, string seed, Point screenPosition, Anchor anchor = Anchor.Center, int zOrder = 0) : base(screenPosition, new(46*9, 4*19), anchor, zOrder)
        {
            worldName = new FormatString(TextAssets.Name + wname);
            seedText = new FormatString(TextAssets.Seed + seed);

            this.wname = wname;
        }

        public override void OnConfirmKeyUp()
        {

        }

        public override void OnConfirmKeyDown()
        {

        }

        public override bool IsFocusable()
        {
            return true;
        }

        public override void OnFocus()
        {
            hovered = true;
            cursorColor = Color.Yellow;
        }

        public override void OnDefocus()
        {
            hovered = false;
            cursorColor = Color.White;
        }

        public override void Update()
        {
            if (IsHovered())
            {
                if (Input.PostClick(InputConfig.NavigationRight))
                {
                    if (cursorPos++ > 1) cursorPos = 0;
                }

                if (Input.PostClick(InputConfig.NavigationLeft))
                {
                    if (cursorPos-- <= 0) cursorPos = 2;
                }

                if (Input.PostClick(InputConfig.NavigationSelect))
                {
                    if (cursorPos == 0)
                    {
                        ConsoleAdventure.progressBar.stepText = Localization.GetTranslation("Progress", "LoadFile");
                        ConsoleAdventure.progressBar.Progress = 0;

                        MainMenu menu = (MainMenu)ConsoleAdventure.mainUIgroup?.GetFirstElementWithName("MainMenu");
                        menu.State = MenuState.worldLoadingProgress;
                        UIElement worldPanels = menu?.GetFirstElementWithName("WorldPanelsContainer");
                        worldPanels?.Hide();
                        worldPanels?.OnDefocus();
                        ConsoleAdventure.progressBar.Show();


                        Display.SetBars();

                        Thread load = new Thread(new ThreadStart(LoadWorld));
                        load.Start();

                        async void LoadWorld()
                        {
                            try
                            {
                                bool inm = false; // in multiplayer
                                bool ish = false; // is host
                                if (ConsoleAdventure.kstate.IsKeyDown(Keys.M)) inm = true;
                                if (ConsoleAdventure.kstate.IsKeyDown(Keys.H)) { ish = true; inm = true; }
                                NetworkManager.isHost = ish;
                                if (!inm) NetworkManager.isHost = true;
                                await ConsoleAdventure.CreateWorld(GetName(), 0, 16, false, inm);

                                if (ConsoleAdventure.world != null)
                                {
                                    if (NetworkManager.isHost || !inm)
                                    {
                                        WorldIO.Load(wname);
                                    }
                                    else
                                    {
                                        await NetworkManager.RequestWorldData();
                                    }
                                }
                            }

                            catch (Exception ex)
                            {
                                string error = $"{Localization.GetTranslation("UI", "WorldLoadError")}\n\n{ex.GetType()}: {ex.Message}\n{ex.InnerException}\n{ex.StackTrace}\n{ex.Source}\n{ex.TargetSite}";
                                //OpenWorldError(error);
                                ConsoleAdventure.logger.AddException(error);
                            }

                            menu.GetFirstElementWithName("MainButtonsContainer")?.OnFocus();
                            ConsoleAdventure.progressBar?.Hide();
                        }
                    }
                    else if (cursorPos == 2)
                    {
                        WorldIO.Delete(wname);
                        MainMenu menu = (MainMenu)ConsoleAdventure.mainUIgroup.GetChild(0);
                        menu.UpdateWorldList(GetParent());
                        GetParent().currentlySelected = 0;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            spriteBatch.DrawString(ConsoleAdventure.Font, FrameSystem.GetFrame(new(46, 4), Frame.RoundedFrame), drawPosition - new Vector2(4, 0), cursorColor);
            spriteBatch.DrawString(ConsoleAdventure.Font, FrameSystem.GetFrame(new(7, 4), Frame.BaseFrame), drawPosition - new Vector2(4, 0), cursorColor);

            spriteBatch.DrawString(ConsoleAdventure.Font, " Λ \n╱ ╲", drawPosition + new Vector2(14, 19), Color.White);

            worldName.Draw(spriteBatch, drawPosition + new Vector2(72, 19));
            seedText.Draw(spriteBatch, drawPosition + new Vector2(72, 38));

            spriteBatch.DrawString(ConsoleAdventure.Font, "► ≡ Ս", drawPosition + new Vector2(351, 19), Color.White);

            if (IsHovered()) spriteBatch.DrawString(ConsoleAdventure.Font, "^", drawPosition + (new Vector2(9 * (39 + (cursorPos * 2)), 19 * 2)), Color.Yellow);
        }
    }
}
