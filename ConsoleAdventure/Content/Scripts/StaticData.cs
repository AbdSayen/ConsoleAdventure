using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts
{
    public class StaticData<T>
    {
        public T DefaultData { get; private set; }

        public T[] Values { get; private set; }

        public StaticData(T defaultData)
        {
            Values = new T[0];
            DefaultData = defaultData;
        }

        private void Add(int type, T value)
        {
            if (type >= Values.Length)
            {
                T[] oldValues = Values;
                Values = new T[type + 1];

                for (int i = 0; i < Values.Length; i++)
                {
                    if(i < oldValues.Length)
                        Values[i] = oldValues[i];

                    else
                        Values[i] = DefaultData;
                }
            }

            if (type >= 0)
            {
                Values[type] = value; //в любом случае (если type >= 0) добавляем значение
            }
        }

        private T Get(int type)
        {
            if (type < 0 || type >= Values.Length)
                return DefaultData;

            return Values[type];
        }

        public T this[int type]
        {
            get
            {
                return Get(type);
            }
            set
            {
                Add(type, value);
            }
        }

        public T this[Transform transform]
        {
            get
            {
                if (transform == null)
                    return DefaultData;

                return Get(transform.type);
            }
            set
            {
                if (transform != null)
                    Add(transform.type, value);
            }
        }
    }
}
