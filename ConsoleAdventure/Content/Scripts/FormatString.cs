using ConsoleAdventure.Content.Scripts.InputLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleAdventure.Content.Scripts
{
    public class FormatString
    {
        public string BaseString { get; private set; } = "";

        public string String { get; set; } = "";

        public List<IFormatMode> FormatModes { get; set; } = new List<IFormatMode>();

        public SmartColor Color { get; set; } = new SmartColor(255);

        public FormatString(string text, Color color) : this(text, new SmartColor(color))
        {

        }

        public FormatString(string text, SmartColor? color)
        {
            FormatModes = new List<IFormatMode>();

            BaseString = text;
            String = text;
            Color = color.HasValue ? color.Value : new SmartColor(255);

            Type baseInterface = typeof(IFormatMode);
            IEnumerable<Type> mods = Assembly.GetAssembly(baseInterface).GetTypes().Where(type => baseInterface.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);


            List<Range> formats = new List<Range>();

            for (int i = 0; i < BaseString.Length; i++)
            {
                if (BaseString[i] == '[')
                {
                    for (int j = i; j < BaseString.Length; j++)
                    {
                        if (BaseString[j] == ']')
                        {
                            formats.Add(new Range(i, j));
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < formats.Count; i++)
            {
                Range range = formats[i];

                foreach (Type type in mods)
                {
                    IFormatMode mode = (IFormatMode)Activator.CreateInstance(type);
                    FormatTemplate template = mode.Template;

                    int start = range.Start.Value + 1;
                    int end = range.End.Value - 1;

                    int endNameIndex = String.IndexOfInRange(":", Math.Min(start, String.Length), Math.Min(end, String.Length));
                    if (endNameIndex == -1) break;

                    int nameIndex = String.IndexOfInRange(template.name, Math.Min(start, String.Length), Math.Min(endNameIndex, String.Length));
                    if (nameIndex == -1) continue;

                    int equalsIndex = -1;

                    List<string> arguments = new List<string>();
                    StringBuilder currentArgument = new StringBuilder();

                    for (int j = endNameIndex + 1; j <= end + 1; j++)
                    {
                        char symbol = String[j];

                        if (symbol == ',' || symbol == '=' || symbol == ']')
                        {
                            if (currentArgument.Length == 0)
                                break;

                            arguments.Add(currentArgument.ToString());
                            currentArgument.Clear();

                            if (symbol == '=' || symbol == ']')
                            {
                                equalsIndex = j;
                                break;
                            }
                        }

                        else
                        {
                            currentArgument.Append(symbol);
                        }
                    }

                    if (arguments.Count != template.argumentsCount)
                        continue;

                    if ((template.isTextModifier && equalsIndex < 0) &&
                        (!template.isTextModifier && equalsIndex >= 0))
                    {
                        continue;
                    }

                    StringBuilder modifyText = new();

                    if (template.isTextModifier)
                    {
                        if (equalsIndex + 1 >= String.Length) break;
                        
                        if (equalsIndex + 1 >= end && String[equalsIndex + 1] != '"' && 
                           (String[end - 1] != '"' && equalsIndex + 1 == end - 1))
                            break;

                        for (int j = equalsIndex + 2; j < end; j++)
                        {
                            modifyText.Append(String[j]);
                        }

                        if (modifyText.Length == 0)
                            break;
                    }

                    mode.Range = range;
                    int result = mode.Define(this, arguments, modifyText.ToString());

                    if (result >= 0)
                    {
                        FormatModes.Add(mode);

                        for (int j = i + 1; j < formats.Count; j++)
                        {
                            formats[j] = new(formats[j].Start.Value - result, 
                                             formats[j].End.Value - result);
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.DrawString(ConsoleAdventure.Font, String, position, Color.Color);

            for (int i = 0; i < FormatModes.Count; i++)
            {
                FormatModes[i].Draw(spriteBatch, position, this);
            }
        }

        public int CutFill(Range range, string fill)
        {
            int start = range.Start.Value;
            int end = range.End.Value;

            StringBuilder sb = new StringBuilder(String);
            int lengthToReplace = Math.Min(end - start + 1, sb.Length);
            sb.Remove(Math.Min(start, sb.Length), lengthToReplace);


            StringBuilder filler = new StringBuilder(fill);
            for (int i = 0; i < filler.Length; i++)
            {
                if (filler[i] != '\n')
                {
                    filler[i] = ' ';
                }
            }

            sb.Insert(Math.Min(start, sb.Length), filler);
            String = sb.ToString();

            return lengthToReplace - fill.Length;
        }
    }
}

