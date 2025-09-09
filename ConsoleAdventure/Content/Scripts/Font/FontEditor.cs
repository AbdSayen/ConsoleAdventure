using CaModLoaderAPI;
using ConsoleAdventure.Content.Scripts.Font;
using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompileGlyph = ConsoleAdventure.Content.Scripts.Font.GlyphsConfigCompiler.Glyph;
using CompileError = ConsoleAdventure.Content.Scripts.Font.GlyphsConfigCompiler.Error;
using System.IO;

namespace ConsoleAdventure.Content.Scripts.FontEditor
{
    public static class FontEditor
    {
        public static void ModifyChars(ContentManager content)
        {
            ConsoleAdventure.SetFont(content.Load<SpriteFont>("Fonts/font"));

            Texture2D fontTexture = ConsoleAdventure.Font.Texture;
            Dictionary<char, SpriteFont.Glyph> glyphs = ConsoleAdventure.Font.GetGlyphs();

            Dictionary<string, (List<CompileGlyph>, List<CompileError>)> allCompileResults = new();

            List<Mod> mods = CaModLoader.GetActiveMods();

            string configsPath = "Content\\Fonts\\Configs";
            string modConfigsPath = "Content\\GlyphsConfigs";

            for (int i = -1; i < mods.Count; i++)
            {
                if (i == -1)
                {
                    string[] configs = Directory.GetFiles(configsPath, "*.txt", SearchOption.AllDirectories).ToArray();

                    if (configs.Length > 0)
                    {
                        allCompileResults.Add("TheConsoleAdventure", (new(), new()));
                    }

                    for (int j = 0; j < configs.Length; j++)
                    {
                        string path = Path.GetFullPath(configsPath + "//" + configs[j]);
                        string name = Path.GetFileName(configs[j]);
                        
                        var buildResult = GlyphsConfigCompiler.Build(File.ReadAllText(configs[j]), name, path, new Point(fontTexture.Width, fontTexture.Height), ConsoleAdventure.Font);

                        allCompileResults["TheConsoleAdventure"].Item1.AddRange(buildResult.glyphs);
                        allCompileResults["TheConsoleAdventure"].Item2.AddRange(buildResult.errors);
                    }
                }

                else
                {
                    Mod mod = mods[i];

                    if (!Directory.Exists(mod.dirName + "//" + modConfigsPath))
                        continue;

                    string[] configs = Directory.GetFiles(mod.dirName + "//" + modConfigsPath, "*.txt", SearchOption.AllDirectories).ToArray();

                    if (configs.Length > 0)
                    {
                        allCompileResults.Add(mod.dirName, (new(), new()));
                    }

                    for (int j = 0; j < configs.Length; j++)
                    {
                        string path = Path.GetFullPath(mod.dirName + "//" + modConfigsPath + "//" + configs[j]);
                        string name = Path.GetFileName(configs[j]);

                        var buildResult = GlyphsConfigCompiler.Build(File.ReadAllText(configs[j]), name, path, new Point(fontTexture.Width, fontTexture.Height), ConsoleAdventure.Font);

                        allCompileResults[mod.dirName].Item1.AddRange(buildResult.glyphs);
                        allCompileResults[mod.dirName].Item2.AddRange(buildResult.errors);
                    }
                }
            }

            for (int i = 0; i < allCompileResults.Count; i++)
            {
                var current = allCompileResults.ElementAt(i);

                List<CompileError> currentErrors = current.Value.Item2;
                List<CompileGlyph> currentGlyphs = current.Value.Item1;

                if (currentErrors.Count == 0)
                {
                    ConsoleAdventure.logger.AddMessage($"Build glyph configs of mod: \"{current.Key}\" was a success:");

                    ConsoleAdventure.logger.AddText($"{DateTime.Now}:  -'");

                    for (int j = 0; j < currentGlyphs.Count; j++)
                    {
                        ConsoleAdventure.logger.AddText(currentGlyphs[j].Char + (j < currentGlyphs.Count - 1 ? ", " : ""));

                        if (glyphs.ContainsKey(currentGlyphs[j].Char))
                        {
                            var glyph = glyphs[currentGlyphs[j].Char];
                            glyph.Character = currentGlyphs[j].Char;

                            Point sourcePos = glyph.BoundsInTexture.Location;

                            if (currentGlyphs[j].Source.Location.X > -1)
                            {
                                sourcePos.X = currentGlyphs[j].Source.Location.X;
                            }

                            if (currentGlyphs[j].Source.Location.Y > -1)
                            {
                                sourcePos.Y = currentGlyphs[j].Source.Location.Y;
                            }

                            if (currentGlyphs[j].SourceOffset.X > -1)
                            {
                                sourcePos.X += currentGlyphs[j].SourceOffset.X;
                            }

                            if (currentGlyphs[j].SourceOffset.Y > -1)
                            {
                                sourcePos.Y += currentGlyphs[j].SourceOffset.Y;
                            }

                            glyph.BoundsInTexture.Location = sourcePos;

                            Point sourceSize = glyph.BoundsInTexture.Size;

                            if (currentGlyphs[j].Source.Size.X > -1)
                            {
                                sourceSize.X = currentGlyphs[j].Source.Size.X;
                            }

                            if (currentGlyphs[j].Source.Size.Y > -1)
                            {
                                sourceSize.Y = currentGlyphs[j].Source.Size.Y;
                            }

                            glyph.BoundsInTexture.Size = sourceSize;



                            Point offsetPos = glyph.Cropping.Location;

                            if (currentGlyphs[j].Cropping.Location.X != 0)
                            {
                                offsetPos.X = currentGlyphs[j].Cropping.Location.X;
                            }

                            if (currentGlyphs[j].Cropping.Location.Y != 0)
                            {
                                offsetPos.Y = currentGlyphs[j].Cropping.Location.Y;
                            }

                            glyph.Cropping.Location = offsetPos;

                            Point offsetSize = glyph.Cropping.Size;

                            if (currentGlyphs[j].Cropping.Size.X > -1)
                            {
                                offsetSize.X = currentGlyphs[j].Cropping.Size.X;
                            }

                            if (currentGlyphs[j].Cropping.Size.Y > -1)
                            {
                                offsetSize.Y = currentGlyphs[j].Cropping.Size.Y;
                            }

                            glyph.Cropping.Size = offsetSize;

                            glyphs[currentGlyphs[j].Char] = glyph;
                        }
                    }

                    ConsoleAdventure.logger.AddText("'\n");
                }

                else
                {
                    ConsoleAdventure.logger.AddMessage($"Build glyph configs of mod: \"{current.Key}\" failed with {currentErrors.Count} error" + (currentErrors.Count > 1 ? "s:" : ":"));

                    for (int j = 0; j < currentErrors.Count; j++)
                    {
                        ConsoleAdventure.logger.AddMessage(" -" + currentErrors[j] + "\n");
                    }
                }
            }

            List<char> chars = new List<char>();
            List<Rectangle> sources = new List<Rectangle>();
            List<Rectangle> croppings = new List<Rectangle>();
            List<Vector3> kernings = new List<Vector3>();

            for (int i = 0; i < glyphs.Count; i++)
            {
                var glyph = glyphs.ElementAt(i);

                chars.Add(glyph.Value.Character);
                sources.Add(glyph.Value.BoundsInTexture);
                croppings.Add(glyph.Value.Cropping);
                kernings.Add(new(glyph.Value.LeftSideBearing, glyph.Value.Width, glyph.Value.RightSideBearing));
            }

            SpriteFont font = new SpriteFont(fontTexture, 
                                             sources, 
                                             croppings, 
                                             chars, 
                                             ConsoleAdventure.Font.LineSpacing, 
                                             ConsoleAdventure.Font.Spacing, 
                                             kernings, 
                                             ConsoleAdventure.Font.DefaultCharacter);

            ConsoleAdventure.SetFont(font);
        }

        private static void ReplaceChar(Color[] fontColors, Texture2D texture, Vector2 position)
        {
            Color[] colors = new Color[texture.Width * texture.Height];
            texture.GetData(colors);

            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    Color color = colors[x + y * texture.Width];
                }
            }
        }
    }
}
