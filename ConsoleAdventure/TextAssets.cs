namespace ConsoleAdventure
{
    internal class TextAssets
    {
        public static string StartLogo = "";

        public static string logo = "The  .####    .####.   ###   ##   .####.   .####.   ##        .#### \r\n    ###   \"  ###  ###  ####  ##  ###*  *  ###  ###  ##       ###   ##\r\n    ##       ##    ##  ## ## ##  *#####.  ##    ##  ##       ##  *##*\r\n    ###   .  ###  ###  ##  ####  .  .###  ###  ###  ##,,,,,  ###    .\r\n     *####    *####*   ##   ###   ####*    *####*   #######   *##### \r\n\r\n                           `-*Adventure*-´           ";

        public static string navigHelp = Localization.GetTranslation("UI", "Navigation");

        public static string navigHelpBack = Localization.GetTranslation("UI", "NavigationBack");

        public static string navigHelpWorld = Localization.GetTranslation("UI", "NavigationWorld");

        public static string navigHelpControl1 = Localization.GetTranslation("UI", "NavigationControl1");

        public static string navigHelpControl2 = Localization.GetTranslation("UI", "NavigationControl2");

        public static string modCreateHelp = Localization.GetTranslation("UI", "HelpModCreate");

        public static string Name = Localization.GetTranslation("UI", "Name");

        public static string Seed = Localization.GetTranslation("UI", "Seed");

        public static string Author = Localization.GetTranslation("UI", "Author");

        public static string Version = Localization.GetTranslation("UI", "Version");

        public static string Inventory = Localization.GetTranslation("UI", "Inventory");

        public static string Day = Localization.GetTranslation("UI", "Day");

        public static string Time = Localization.GetTranslation("UI", "Time");

        public static string Structure = Localization.GetTranslation("UI", "Structure");

        public static string Paused = Localization.GetTranslation("UI", "Paused");

        public static string About = Localization.GetTranslation("UI", "About");

        public static string AboutGame = Localization.GetTranslation("About", "Game");

        public static string Control = Localization.GetTranslation("UI", "Control");

        //public static string AboutControl = Localization.GetTranslation("About", "Control");

        public static string HelpWorldCreate = Localization.GetTranslation("UI", "HelpWorldCreate");

        public static string Mods = Localization.GetTranslation("UI", "Mods");

        public static string navigModFolderHelp = Localization.GetTranslation("UI", "NavigationModFolder");

        public static string Item = Localization.GetTranslation("Generic", "Item");

        public static string ItemGenitive = Localization.GetTranslation("Generic", "Item-genitive");

        public static string Items = Localization.GetTranslation("Generic", "Items");

        public static string ItemsGenitive = Localization.GetTranslation("Generic", "Items-genitive");

        public static string Block = Localization.GetTranslation("Generic", "Block");

        public static string BlockGenitive = Localization.GetTranslation("Generic", "Block-genitive");

        public static string Blocks = Localization.GetTranslation("Generic", "Blocks");

        public static string BlocksGenitive = Localization.GetTranslation("Generic", "Blocks-genitive");

        public static string FloorTooltip = Localization.GetTranslation("UI", "Floor");

        public static string BlockTooltip = Localization.GetTranslation("UI", "Block");

        public static string LootTooltip = Localization.GetTranslation("UI", "Loot");

        public static string EntityTooltip = Localization.GetTranslation("UI", "Entity");

        public static string ItemCanBePlaced = Localization.GetTranslation("Generic", "CanBePlaced");

        public static string ItemCanBeEaten = Localization.GetTranslation("Generic", "CanBeEaten");

        public static string ItemMaterial = Localization.GetTranslation("Generic", "Material");

        public static string ItemConsumed = Localization.GetTranslation("Generic", "Consumed");

        public static string[] NormalFrame3x2 = Utils.GetPanel(new(3, 2), 0);

        public static void UpdateLabels()
        {
            navigHelp = Localization.GetTranslation("UI", "Navigation");
            navigHelpBack = Localization.GetTranslation("UI", "NavigationBack");
            navigHelpWorld = Localization.GetTranslation("UI", "NavigationWorld");
            navigHelpControl1 = Localization.GetTranslation("UI", "NavigationControl1");
            navigHelpControl2 = Localization.GetTranslation("UI", "NavigationControl2");
            modCreateHelp = Localization.GetTranslation("UI", "HelpModCreate");
            Name = Localization.GetTranslation("UI", "Name");
            Seed = Localization.GetTranslation("UI", "Seed");
            Author = Localization.GetTranslation("UI", "Author");
            Version = Localization.GetTranslation("UI", "Version");
            Inventory = Localization.GetTranslation("UI", "Inventory");
            Day = Localization.GetTranslation("UI", "Day");
            Time = Localization.GetTranslation("UI", "Time");
            Structure = Localization.GetTranslation("UI", "Structure");
            Paused = Localization.GetTranslation("UI", "Paused");
            About = Localization.GetTranslation("UI", "About");
            AboutGame = Localization.GetTranslation("About", "Game");
            Control = Localization.GetTranslation("UI", "Control");
            //AboutControl = Localization.GetTranslation("About", "Control");
            HelpWorldCreate = Localization.GetTranslation("UI", "HelpWorldCreate");
            Mods = Localization.GetTranslation("UI", "Mods");

            Item = Localization.GetTranslation("Generic", "Item");
            ItemGenitive = Localization.GetTranslation("Generic", "Item-genitive");
            Items = Localization.GetTranslation("Generic", "Items");
            ItemsGenitive = Localization.GetTranslation("Generic", "Items-genitive");
            Block = Localization.GetTranslation("Generic", "Block");
            BlockGenitive = Localization.GetTranslation("Generic", "Block-genitive");
            Blocks = Localization.GetTranslation("Generic", "Blocks");
            BlocksGenitive = Localization.GetTranslation("Generic", "Blocks-genitive");

            FloorTooltip = Localization.GetTranslation("UI", "Floor");
            BlockTooltip = Localization.GetTranslation("UI", "Block");
            LootTooltip = Localization.GetTranslation("UI", "Loot");
            EntityTooltip = Localization.GetTranslation("UI", "Entity");

            ItemCanBePlaced = Localization.GetTranslation("Generic", "CanBePlaced");
            ItemCanBeEaten = Localization.GetTranslation("Generic", "CanBeEaten");
            ItemMaterial = Localization.GetTranslation("Generic", "Material");
            ItemConsumed = Localization.GetTranslation("Generic", "Consumed");
        }
    }
}
