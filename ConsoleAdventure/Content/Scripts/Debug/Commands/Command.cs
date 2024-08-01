using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleAdventure.Content.Scripts.Debug.Commands
{
    public abstract class Command
    {
        public static List<Command> Commands { get; private set; } = new List<Command>();

        public string Name { get; protected set; }

        public List<string> Arguments { get; protected set; }

        public string Description { get; protected set; }

        protected Command() 
        { 
        
        }

        public static void InitCommands()
        {
            Type baseType = typeof(Command);
            IEnumerable<Type> list = Assembly.GetAssembly(baseType).GetTypes().Where(type => type.IsSubclassOf(baseType));
            
            foreach (Type type in list)
            {
                Commands.Add((Command)Activator.CreateInstance(type));
            }
        }

        public virtual void Logic(string[] args)
        {

        }

        public static void Find(string commandText) 
        {
            List<string> fragments = new List<string>(commandText.Split(new[] { ' ' }, StringSplitOptions.None));

            foreach (var command in Commands)
            {
                if (command.Name == fragments[0] && command.Arguments.Count == fragments.Count - 1)
                {
                    try
                    {
                        List<string> args = fragments;
                        args.RemoveAt(0);
                        command.Logic(args.ToArray());
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        protected int GetArgIndex(string name)
        {
            return Arguments.IndexOf(name);
        }

        protected int GetIntArg(string[] args, string name)
        {
            return int.Parse(args[GetArgIndex(name)]);
        }

        protected string GetStringArg(string[] args, string name)
        {
            return args[GetArgIndex(name)];
        }
    }
}
