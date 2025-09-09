using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ConsoleAdventure.Content.Scripts.Font.GlyphsConfigCompiler;

namespace ConsoleAdventure.Content.Scripts.Font
{
    public static class GlyphsConfigCompiler
    {
        public struct Error
        {
            public string Code { get; set; } = "";

            public string Token { get; set; } = "";

            public string FileName { get; set; } = "";

            public string Path { get; set; } = "";

            public int Line { get; set; } = 0;

            public Error(string code, string token, string fileName, string path, int line)
            {
                Code = code;
                Token = token;
                FileName = fileName;
                Path = path;
                Line = line;
            }

            public override string ToString() 
            { 
                return $"{FileName} in ({Line + 1}, \"{Token}\") {Code} path: {Path}"; 
            }
        }

        public struct Glyph
        {
            public char Char { get; set; } = '\0';

            public Rectangle Source { get; set; }

            public Rectangle Cropping { get; set; }

            public Point SourceOffset { get; set; }

            public Glyph(char symbol, Rectangle source, Rectangle cropping, Point sourceOffset)
            {
                Char = symbol;
                Source = source;
                Cropping = cropping;
                SourceOffset = sourceOffset;
            }
        }

        public struct Line
        {
            public string Text { get; set; } = "";

            public int Number { get; set; } = 0;

            public Line(string text, int number)
            {
                Text = text;
                Number = number;
            }

            public override string ToString()
            {
                return $"[line {Number + 1}]: \"{Text}\"";
            }
        }

        public struct Token
        {
            public string Text { get; set; } = "";

            public int LineNumber { get; set; } = 0;

            public Token(string text, int lineNumber)
            {
                Text = text;
                LineNumber = lineNumber;
            }

            public override string ToString()
            {
                return $"[line {LineNumber + 1}]: \"{Text}\"";
            }
        }

        struct RawGlyph
        {
            public int Line { get; set; }

            public string Glyphs { get; set; } = "";

            public int SourceX { get; set; } = 0;

            public int SourceY { get; set; } = 0;

            public int SourceWidth { get; set; } = 0;

            public int SourceHeight { get; set; } = 0;

            public int OffsetX { get; set; } = 0;

            public int OffsetY { get; set; } = 0;

            public int OffsetWidth { get; set; } = 0;

            public int OffsetHeight { get; set; } = 0;

            public int OffsetSourceX { get; set; } = 0;

            public int OffsetSourceY { get; set; } = 0;

            public RawGlyph(int line)
            {
                Line = line;

                Glyphs = "";

                SourceX = -1;
                SourceY = -1;
                SourceWidth = -1;
                SourceHeight = -1;

                OffsetX = 0;
                OffsetY = 0;
                OffsetWidth = -1;
                OffsetHeight = -1;

                OffsetSourceX = 0;
                OffsetSourceY = 0;
            }
        }

        #region Errors

        private static string HeadDeclarationsError { get; } = "A new config cannot be declared inside another one.";

        private static string NoOpenError { get; } = "No open configurations.";

        private static string OtherError { get; } = "Token not available in this context or not declared.";

        private static string EmptyError { get; } = "Could not find the parameter value.";

        private static string NoTokenError { get; } = "This token does not exist.";

        private static string InvalidArgError { get; } = "Invalid argument format.";

        private static string DuplicateParameterError { get; } = "There cannot be duplicate parameters within one config.";

        private static string TwoSourceValuesError { get; } = "Can't assign values ​​to both \"-source\" and \"-offset-source\" in one config.";

        private static string NotOverrideError { get; } = "This config does not override glyph settings.";

        private static string NegativeError { get; } = "This value cannot be negative.";

        private static string SourceRectangleError { get; } = "In this config the source rectangle extends beyond the font texture boundaries ";

        private static string MultipleGlyphsPosError { get; } = "You cannot assign the same source position to multiple glyphs in this config.";

        private static string NoClosedError { get; } = "This config not closed.";

        private static string NoSymbolError { get; } = "This symbol does not exist";

        #endregion

        #region Tokens

        private static string ConfigHead { get; } = "[GlyphConfig]";

        private static string ConfigEnd { get; } = "[End]";

        private static string GlyphsParam { get; } = "glyphs";

        private static string SourceXParam { get; } = "x-source";

        private static string SourceYParam { get; } = "y-source";

        private static string SourceWidthParam { get; } = "w-size-source";

        private static string SourceHeightParam { get; } = "h-size-source";

        private static string OffsetXParam { get; } = "x-offset";

        private static string OffsetYParam { get; } = "y-offset";

        private static string OffsetWidthParam { get; } = "w-size-offset";

        private static string OffsetHeightParam { get; } = "h-size-offset";

        private static string OffsetSourceXParam { get; } = "x-offset-source";

        private static string OffsetSourceYParam { get; } = "y-offset-source";

        #endregion

        public static (List<Glyph> glyphs, List<Error> errors) Build(string configs, string fileName, string path, Point size, SpriteFont spriteFont)
        {
            if (configs == null) throw new ArgumentNullException();

            List<Glyph> glyphs = new List<Glyph>();
            List<Error> errors = new List<Error>();
            
            List<Line> lines = GetLines(configs);

            for (int i = 0; i < lines.Count; i++)
            {
                bool emptyLine = true;

                foreach (char c in lines[i].Text)
                {
                    if (c != '\r' && c != ' ' && c != '\t') { emptyLine = false; break; }
                }

                if (emptyLine)
                {
                    lines.RemoveAt(i);
                }
            }

            List<Token> tokens = GetTokens(lines);

            var value = GetRawGlyphs(tokens, fileName, path);

            errors = value.errors;

            for (int i = 0; i < value.glyphs.Count; i++)
            {
                RawGlyph raw = value.glyphs[i];

                if (raw.OffsetSourceX == 0 && 
                    raw.OffsetSourceY == 0 &&
                    raw.SourceX < 0 &&
                    raw.SourceY < 0 &&
                    raw.SourceWidth < 0 &&
                    raw.SourceHeight < 0 &&
                    raw.OffsetX == 0 &&
                    raw.OffsetY == 0 &&
                    raw.OffsetWidth < 0 &&
                    raw.OffsetHeight < 0)
                {
                    errors.Add(new Error(NotOverrideError, "[GlyphConfig]", fileName, path, raw.Line));
                }

                if ((raw.SourceX > -1 || raw.SourceY > -1) && 
                    (raw.OffsetSourceX != 0 || raw.OffsetSourceY != 0))
                {
                    errors.Add(new Error(TwoSourceValuesError, "[GlyphConfig]", fileName, path, raw.Line));
                }

                if (raw.SourceX + raw.SourceWidth > size.X && raw.SourceY + raw.SourceHeight > size.Y)
                {
                    errors.Add(new Error(SourceRectangleError + $"({size.X}, {size.Y}).", "[GlyphConfig]", fileName, path, raw.Line));
                }

                if (raw.Glyphs.Length > 1 && (raw.SourceX > -1 || raw.SourceY > -1))
                {
                    errors.Add(new Error(MultipleGlyphsPosError, "[GlyphConfig]", fileName, path, raw.Line));
                }

                if (errors.Count == 0)
                {
                    for (int j = 0; j < raw.Glyphs.Length; j++)
                    {
                        char c = raw.Glyphs[j];

                        if (!spriteFont.GetGlyphs().ContainsKey(c))
                        {
                            errors.Add(new Error(NoSymbolError + ":'" + c + "'.", "[GlyphConfig]", fileName, path, raw.Line));
                            continue;
                        }

                        Rectangle source = new Rectangle(raw.SourceX, raw.SourceY, raw.SourceWidth, raw.SourceHeight);
                        Rectangle offset = new Rectangle(raw.OffsetX, raw.OffsetY, raw.OffsetWidth, raw.OffsetHeight);

                        glyphs.Add(new Glyph(c, source, offset, new Point(raw.OffsetSourceX, raw.OffsetSourceY)));
                    }
                }
            }

            return (glyphs, errors);
        }

        private static List<Line> GetLines(string configs)
        {
            List<string> rawLines = (configs + "\r").Split("\n").ToList();

            List<Line> lines = new List<Line>();

            for (int i = 0; i < rawLines.Count; i++)
            {
                lines.Add(new Line(rawLines[i], i));
            }

            return lines;
        } 

        private static List<Token> GetTokens(List<Line> lines)
        {
            List<Token> tokens = new List<Token>();

            foreach (Line line in lines)
            {
                StringBuilder buffer = new StringBuilder();

                int commentIndex = line.Text.IndexOf("//");

                for (int i = 0; i < line.Text.Length; i++)
                {
                    bool onComment = commentIndex != -1 && i >= commentIndex;

                    if (line.Text[i] == ' ' || line.Text[i] == '\t')
                        continue;

                    if ((line.Text[i] == ':' || i >= line.Text.Length - 1 || onComment) && buffer.Length > 0)
                    {
                        tokens.Add(new Token(buffer.ToString(), line.Number));
                        buffer.Clear();

                        if (!onComment)
                            continue;
                    }

                    if (onComment)
                        break;

                    buffer.Append(line.Text[i]);
                }
            }

            return tokens;
        }

        private static (List<RawGlyph> glyphs, List<Error> errors) GetRawGlyphs(List<Token> tokens, string fileName, string path)
        {
            List<RawGlyph> glyphs = new List<RawGlyph>();
            List<Error> errors = new List<Error>();
            int rawGlyphsIndex = -1;

            bool isClosed = true;

            List<string> usedTokens = new List<string>();

            for (int i = 0; i < tokens.Count; i++)
            {
                Token token = tokens[i];

                if (token.Text == ConfigHead)
                {
                    if (isClosed)
                    {
                        isClosed = false;
                        glyphs.Add(new RawGlyph(token.LineNumber));
                        rawGlyphsIndex++;
                        usedTokens.Clear();
                    }

                    else
                        errors.Add(new Error(HeadDeclarationsError, token.Text, fileName, path, token.LineNumber));
                }

                else if (token.Text == ConfigEnd)
                {
                    if (!isClosed)
                        isClosed = true;

                    else
                        errors.Add(new Error(NoOpenError, token.Text, fileName, path, token.LineNumber));
                }

                else if (!isClosed)
                {
                    if (usedTokens.Contains(token.Text))
                    {
                        errors.Add(new Error(DuplicateParameterError, token.Text, fileName, path, token.LineNumber));
                    }

                    else
                    {
                        usedTokens.Add(token.Text);
                    }

                    string value = "";
                    int valueLine = i + 1;

                    if (i < tokens.Count - 2)
                    {
                        value = tokens[i + 1].Text;
                    }

                    if (value != "")
                    {
                        if (token.Text == GlyphsParam)
                        {

                            if (value == "--")
                            {
                                errors.Add(new Error(InvalidArgError, value, fileName, path, token.LineNumber));
                                i++;
                                continue;
                            }

                            RawGlyph raw = glyphs[rawGlyphsIndex];
                            raw.Glyphs = value;
                            glyphs[rawGlyphsIndex] = raw;
                            i++;
                        }

                        else
                        {
                            if (value == "--")
                            {
                                i++;
                                continue;
                            }


                            if (int.TryParse(value, out int result))
                            {
                                RawGlyph raw = glyphs[rawGlyphsIndex];

                                if (token.Text == SourceXParam ||
                                    token.Text == SourceYParam ||
                                    token.Text == SourceWidthParam ||
                                    token.Text == SourceHeightParam ||
                                    token.Text == OffsetWidthParam ||
                                    token.Text == OffsetHeightParam) 
                                {
                                    if (result < 0)
                                    {
                                        i++;
                                        errors.Add(new Error(NegativeError, token.Text, fileName, path, token.LineNumber));
                                        continue;
                                    } 
                                }

                                if (token.Text == SourceXParam) 
                                    raw.SourceX = result;

                                else if (token.Text == SourceYParam) 
                                    raw.SourceY = result;

                                else if (token.Text == SourceWidthParam) 
                                    raw.SourceWidth = result;

                                else if (token.Text == SourceHeightParam)
                                    raw.SourceHeight = result;
      
                                else if (token.Text == OffsetXParam)
                                    raw.OffsetX = result;

                                else if (token.Text == OffsetYParam)
                                    raw.OffsetY = result;

                                else if (token.Text == OffsetWidthParam)
                                    raw.OffsetWidth = result;

                                else if (token.Text == OffsetHeightParam)
                                    raw.OffsetHeight = result;

                                else if (token.Text == OffsetSourceXParam)
                                    raw.OffsetSourceX = result;
  
                                else if (token.Text == OffsetSourceYParam)
                                    raw.OffsetSourceY = result;

                                else
                                {
                                    errors.Add(new Error(NoTokenError, token.Text, fileName, path, token.LineNumber));
                                    continue;
                                }

                                glyphs[rawGlyphsIndex] = raw;
                                i++;
                            }

                            else
                            {
                                errors.Add(new Error(InvalidArgError, token.Text, fileName, path, token.LineNumber));
                            }
                        }
                    }

                    else
                    {
                        errors.Add(new Error(EmptyError, value, fileName, path, valueLine));
                    }
                }

                else
                {
                    errors.Add(new Error(OtherError, token.Text, fileName, path, token.LineNumber));
                }
            }

            if (!isClosed)
                errors.Add(new Error(NoClosedError, "\"\"", fileName, path, tokens[tokens.Count - 1].LineNumber));


            return (glyphs, errors);
        }
    }
}
