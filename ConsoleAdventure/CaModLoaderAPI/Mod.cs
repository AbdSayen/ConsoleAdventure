using ConsoleAdventure.CaModLoaderAPI;
using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Networks;
using ConsoleAdventure.WorldEngine;
using ConsoleAdventure.WorldEngine.Generate;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.CompilerServices;
using ConsoleAdventure.Content.Scripts.WorldEngine;
using ConsoleAdventure.Content.Scripts.IO;

namespace CaModLoaderAPI
{
    public abstract class Mod : IMod // Главный класс модификации
    {
        /// <summary>
        ///  Название папки мода
        /// </summary>
        public string dirName { get; set; }

        /// <summary>
        ///  Название мода
        /// </summary>
        public string modName { get; set; } = "undefined";

        /// <summary>
        /// Версия мода
        /// </summary>
        public string modVersion { get; set; } = "1.0.0";

        /// <summary>
        /// Описание мода
        /// </summary>
        public string modDescription { get; set; } = "No description";

        /// <summary>
        /// Автор мода
        /// </summary>
        public string modAuthor { get; set; } = "Anonymous";

        /// <summary>
        /// Иконка мода
        /// </summary>
        public CharTexture modIcon { get; set; }

        public string GetModString()
        {
            return GetType().Name;
        }

        public int GetModTransform<T>()
        {
            return Main.GetModTransform<T>();
        }

        /// <summary>
        /// Инициализация мода.
        /// </summary>
        public virtual void Init()
        {

        }

        /// <summary>
        ///  Этот хук вызывается после полной инициализации игры, можно выполнить какие-то изменения в работе приложения или присвоить значения переменным
        /// </summary>
        public virtual void Run()
        {

        }

        public virtual void PreDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        public virtual void PostDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        public virtual bool PreDrawWorld(SpriteBatch spriteBatch, GameTime gameTime, World world)
        {
            return true;
        }

        public virtual void PostDrawWorld(SpriteBatch spriteBatch, GameTime gameTime, World world)
        {

        }

        public virtual void PostDrawModPanel(SpriteBatch spriteBatch, Vector2 position)
        {

        }

        /// <summary>
        /// Хук пост загрузки мира. Можно выполнить манипуляции над миром или какие-то изменения игрового процесса. В случае создании каких либо объектов они перекроются загруженным сохранением. Так что объекты появятся только при генерации нового мира
        /// </summary>
        /// <param name="world">Загруженный мир</param>
        public virtual void WorldLoaded(World world)
        {
            
        }

        public virtual void WorldPostGenerate(World world)
        {

        }

        public virtual bool WorldGeneratorPreBuildPipeline(Generator generator)
        {
            return true;
        }

        public virtual void WorldGeneratorBuildPipeline(Generator generator)
        {

        }

        public virtual void Unload()
        {

        }

        public virtual Observer GetWorldObserver(World world)
        {
            return null;
        }

        public virtual void SaveModTags(ModTags tags)
        {

        }

        public virtual void LoadModTags(ModTags tags)
        {

        }
    }
}
