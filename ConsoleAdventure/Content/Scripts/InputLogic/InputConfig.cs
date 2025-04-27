using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleAdventure.Content.Scripts.InputLogic
{

    public static class InputConfig
    {
        public static List<Key> AllKeys { get; internal set; } = new List<Key>();

        #region Movement

        public static Key Up = new(Keys.W, "Movement.Up");
        public static Key Down = new(Keys.S, "Movement.Down");
        public static Key Left = new(Keys.A, "Movement.Left");
        public static Key Right = new(Keys.D, "Movement.Right");
        public static Key Run = new(Keys.LeftShift, "Movement.Run");

        #endregion

        #region Building

        public static Key CursorUp = new(Keys.Up, "Cursor.Up");
        public static Key CursorDown = new(Keys.Down, "Cursor.Down");
        public static Key CursorLeft = new(Keys.Left, "Cursor.Left");
        public static Key CursorRight = new(Keys.Right, "Cursor.Right");
        public static Key Cursor = new(Keys.Space, "Cursor.Call");

        #endregion

        #region Inventory

        public static Key InventoryPlus = new(Keys.OemPlus, "Inventory.ListedDown");
        public static Key InventoryMinus = new(Keys.OemMinus, "Inventory.ListedUp");
        public static Key PickUp = new(Keys.L, "Inventory.PickUp");
        public static Key DropItem = new(Keys.Q, "Inventory.Drop");
        public static Key Use = new(Keys.B, "Inventory.Use");

        #endregion

        #region Chest

        public static Key ChestPlus = new(Keys.OemCloseBrackets, "Chest.ListDown");
        public static Key ChestMinus = new(Keys.OemOpenBrackets, "Chest.ListUp");
        public static Key TakeInInventory = new(Keys.U, "Chest.TakeInInventory");
        public static Key TakeInChest = new(Keys.J, "Chest.TakeInChest");
        public static Key TakeInInventoryStack = new(Keys.I, "Chest.TakeInInventoryStack");
        public static Key TakeInChestStack = new(Keys.O, "Chest.TakeInChestStack");

        #endregion

        #region Recipe

        public static Key RecipeOpen = new(Keys.R, "Recipe.Open");
        public static Key RecipeBookOpen = new(Keys.C, "Recipe.BookOpen");
        public static Key RecipeListLeft = new(Keys.OemOpenBrackets, "Recipe.ListedLeft");
        public static Key RecipeListRight = new(Keys.OemCloseBrackets, "Recipe.ListedRight");

        #endregion

        #region Menu

        public static Key NavigationUp = new(Keys.Up, "Menu.NavigationUp");
        public static Key NavigationDown = new(Keys.Down, "Menu.NavigationDown");
        public static Key NavigationLeft = new(Keys.Left, "Menu.NavigationLeft");
        public static Key NavigationRight = new(Keys.Right, "Menu.NavigationRight");
        public static Key NavigationSelect = new(Keys.Enter, "Menu.NavigationSelect");
        public static Key NavigationBeck = new(Keys.Escape, "Menu.NavigationBeck");
        public static Key OpenLogs = new(Keys.L, "Menu.OpenLogs");
        public static Key WorldGen = new(Keys.N, "Menu.WorldGen");
        public static Key ControlEdit = new(Keys.Space, "Menu.ControlKeyEdit");
        public static Key ControlReset = new(Keys.Delete, "Menu.ControlReset");
        public static Key ModCreate = new (Keys.N, "Menu.ModCreate");
        //public static Key ModToggle = new(Keys.E, "Menu.ModToggle");

        #endregion

        #region Misc

        public static Key Pause = new(Keys.P, "Misc.Pause");
        public static Key Cmd = new(Keys.OemTilde, "Misc.Cmd");
        public static Key Interaction = new(Keys.E, "Misc.Interaction");
        public static Key BuffsPlus = new(Keys.OemCloseBrackets, "Misc.BuffsListedRight");
        public static Key BuffsMinus = new(Keys.OemOpenBrackets, "Misc.BuffsListedLeft");
        public static Key WorldExit = new(Keys.Escape, "Misc.WorldExit");
        public static Key MapOpen = new (Keys.M, "Misc.MapOpen");
        public static Key MapZoom = new(Keys.N, "Misc.MapZoom");
        public static Key MapWUp = new(Keys.OemCloseBrackets, "Misc.MapWUp");
        public static Key MapWDown = new(Keys.OemOpenBrackets, "Misc.MapWDown");

        #endregion

        internal static void Init()
        {
            for (int i = 0; i < AllKeys.Count; i++)
            {
                SettingsSystem.InitSetting("Control", AllKeys[i].name, (int)AllKeys[i].key);
            }
        }

        internal static void Load()
        {
            for (int i = 0; i < AllKeys.Count; i++)
            {
                AllKeys[i].key = (Keys)SettingsSystem.GetSetting("Control", AllKeys[i].name);
            }
        }
    }

    public class Key
    {
        public Keys key;
        public string name;
        //public string mod;

        public Key(Keys key, string name, bool saveToList = true)
        {
            this.key = key;
            this.name = name;

            if (InputConfig.AllKeys == null) InputConfig.AllKeys = new List<Key>();

            if (saveToList)
            {
                InputConfig.AllKeys.Add(this);
            }
        }
    }
}