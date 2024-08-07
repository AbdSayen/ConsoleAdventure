﻿using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.CompilerServices;

namespace CaModLoaderAPI
{
    public abstract class Mod // Главный класс модификации
    {
        /// <summary>
        ///  Название мода
        /// </summary>
        public string modName = "undefined";

        /// <summary>
        /// Версия мода
        /// </summary>
        public string modVersion = "1.0.0";

        /// <summary>
        /// Описание мода
        /// </summary>
        public string modDescription = "No description";

        /// <summary>
        /// Автор мода
        /// </summary>
        public string modAuthor = "Anonymous";

        public string GetModString()
        {
            return GetType().Name;
        }

        public int GetModTransform<T>()
        {
            return Main.GetModTransform<T>(GetModString());
        }

        /// <summary>
        /// Инициализация мода. Для корректной работы рекомендуется устанавливать название, версию, описание и автора мода именно в этом хуке
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

        /// <summary>
        /// Хук пост загрузки мира. Можно выполнить манипуляции над миром или какие-то изменения игрового процесса. В случае создании каких либо объектов они перекроются загруженным сохранением. Так что объекты появятся только при генерации нового мира
        /// </summary>
        /// <param name="world">Загруженный мир</param>
        public virtual void WorldLoaded(World world)
        {
            
        }
    }
}
