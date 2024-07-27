using System;
using System.IO;

namespace ConsoleAdventure
{
    internal static class ModCreator
    {
        static string path = Program.savePath + "ModSources\\";
        static string dll = "ConsoleAdventure.dll";

        static string csprojText = "<Project Sdk=\"Microsoft.NET.Sdk\">\r\n\r\n  <PropertyGroup>\r\n    <TargetFramework>net6.0-windows</TargetFramework>\r\n    <ImplicitUsings>enable</ImplicitUsings>\r\n    <Nullable>enable</Nullable>\r\n  </PropertyGroup>\r\n\r\n  <ItemGroup>\r\n    <PackageReference Include=\"MonoGame.Framework.DesktopGL\" Version=\"3.8.1.303\" />\r\n  </ItemGroup>\r\n\r\n  <ItemGroup>\r\n    <Reference Include=\"ConsoleAdventure\">\r\n      <HintPath>..\\ConsoleAdventure.dll</HintPath>\r\n    </Reference>\r\n  </ItemGroup>\r\n\r\n</Project>";

        static string GetModClass(string name)
        {
            return $"using CaModLoaderAPI;\r\nusing ConsoleAdventure.WorldEngine;\r\nusing ConsoleAdventure;\r\nusing Microsoft.Xna.Framework;\r\n\r\nnamespace {name}\r\n{{\r\n    public class {name} : Mod\r\n    {{\r\n        public override void Init()\r\n        {{\r\n            modName = \"{name}\";\r\n            modVersion = \"0.0.1\";\r\n            modAuthor = \"\";\r\n            modDescription = \"\";\r\n        }}\r\n\r\n        public override void Run()\r\n        {{\r\n\r\n        }}\r\n\r\n        public override void WorldLoaded(World world)\r\n        {{\r\n\r\n        }}\r\n    }}\r\n}}";
        }


        public static bool CreateMod(string name)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (Directory.Exists(path + name))
                return false;

            string modDirectory = path + name + "\\" + name + "\\";

            Directory.CreateDirectory(modDirectory);

            File.Copy(AppDomain.CurrentDomain.BaseDirectory + dll, path + name + "\\" + dll);

            File.WriteAllText(modDirectory + name + ".csproj", csprojText);
            File.WriteAllText(modDirectory + name + ".cs", GetModClass(name));

            return true;
        }
    }
}
