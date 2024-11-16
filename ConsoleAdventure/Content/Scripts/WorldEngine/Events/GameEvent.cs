using ConsoleAdventure.Content.Scripts.Debug.Commands;
using ConsoleAdventure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.WorldEngine.Events
{
    public class GameEvent
    {
        public static List<GameEvent> Events = new List<GameEvent>();

        public static void AddEvent(GameEvent evt)
        {
            if (!Events.Contains(evt))
                Events.Add(evt);
        }

        public static void InitEvents()
        {
            Events.Clear();

            Type baseType = typeof(GameEvent);
            IEnumerable<Type> list = Assembly.GetAssembly(baseType).GetTypes().Where(type => type.IsSubclassOf(baseType));

            foreach (Type type in list)
            {
                GameEvent gameEvent = (GameEvent)Activator.CreateInstance(type);
                gameEvent.Init();
                AddEvent(gameEvent);
            }

            Loger.AddLog("Events inited: " + Events.Count.ToString());
        }

        public virtual void Init()
        {

        }

        public virtual bool IsActive()
        {
            return false;
        }

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {

        }
    }
}
