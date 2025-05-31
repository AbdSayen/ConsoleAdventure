using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.IO
{
    public class Tags
    {
        public Dictionary<string, object> Data { get; internal set; }

        public Tags() 
        { 
            Data = new(new Dictionary<string, object>());        
        }

        /// <summary>
        /// Чтение/Запись тега. При записи, в случае существования тега, он перезаписываеться.<br/><br/>
        /// Рекомендация:<br/>
        ///     Старайтесь следовать единому стилю имён тегов "<paramref name="key"/>", а именно <c>PascalCasing</c>, тоесть: все слова в названии должны начинаться с большой буквы и писаться слитно.
        ///     Например:
        ///     <code>
        ///     - ObjectPosition
        ///     - CustomWorldData
        ///     - Objects</code>
        ///     Желательно указывать в начале имя вашего мода, в таком стиле: <c>ModName/TagName</c>
        /// </summary>
        /// <param name="key">
        /// Имя тега. 
        /// </param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                return Data[key];
            }
            set
            {
                if (!Data.ContainsKey(key))
                    Data.Add(key, value);

                else Data[key] = value;
            }
        }

        /// <summary>
        /// Не совсем безопасный способ извлечения данных из <see cref="Tags"/> 
        /// </summary>
        /// <typeparam name="T">Десериализуемый тип тега</typeparam>
        /// <param name="key">Ключ тега, его имя</param>
        /// <param name="def">Значение по умолчанию, которое будет возвращено в случае отсутствия тега</param>
        /// <returns>Данные тега "<paramref name="key"/>"</returns>
        public T SafelyGet<T>(string key, T def = default)
        {
            if (Data.TryGetValue(key, out var value))
            {
                return (T)value;
            }

            return def;
        }
    }
}
