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
        public static Dictionary<string, Command> Commands { get; private set; } = new Dictionary<string, Command>();

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
                Command cmd = (Command)Activator.CreateInstance(type);
                Commands.Add(cmd.Name, cmd);
            }
        }

        public virtual void Logic(string[] args)
        {

        }

        public static void Find(string commandText) 
        {
            List<string> fragments = new List<string>(commandText.Split(new[] { ' ' }, StringSplitOptions.None));

            if (Commands.Keys.Contains(fragments[0]))
            {
                Command cmd = Commands[fragments[0]];
                try
                {
                    List<string> args = fragments;
                    args.RemoveAt(0);
                    cmd.Logic(args.ToArray());
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        protected int GetArgIndex(string name)
        {
            return Arguments.IndexOf(name);
        }

        protected int GetIntArg(string[] args, string name)
        {
            int idx = GetArgIndex(name);
            if (args.Length - 1 >= idx)
                return int.Parse(args[GetArgIndex(name)]);
            else
                return 0;
        }

        protected string GetStringArg(string[] args, string name)
        {
            int idx = GetArgIndex(name);
            if (args.Length - 1 >= idx)
                return args[idx];
            else
                return "";
        }
    }
}
