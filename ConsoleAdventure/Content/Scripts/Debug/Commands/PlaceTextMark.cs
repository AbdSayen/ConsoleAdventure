using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleAdventure.Content.Scripts.Debug.Commands
{
    public class PlaceTextMark : Command
    {
        public PlaceTextMark() 
        {
            Name = "textmark";
            Description = "place a text in world";
            Arguments = new List<string>()
            {
                "text"
            };
        }

        public override void Logic(string[] args, short id = -1)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < args.Length; i++)
                {
                    stringBuilder.Append(args[i]);

                    if (i != args.Length - 1)
                    {
                        stringBuilder.Append(" ");
                    }
                }

                Position position = ConsoleAdventure.world.GetLocalPlayer().position;
                (new TextMark(position, ConsoleAdventure.world.Surface)).LoadData(stringBuilder.ToString());
            }

            catch (Exception) { }          
        }
    }
}
