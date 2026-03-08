using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;

namespace ConsoleAdventure.Content.Scripts.InputLogic;

public static class Input
{
    public static int GetHorizontalMovement()
    {
        if (ConsoleAdventure.kstate.IsKeyDown(InputConfig.Left.key))
            return -1;

        if (ConsoleAdventure.kstate.IsKeyDown(InputConfig.Right.key))
            return 1;

        return 0;
    }

    public static int GetVerticalMovement()
    {
        if (ConsoleAdventure.kstate.IsKeyDown(InputConfig.Up.key))
            return 1;
        
        if (ConsoleAdventure.kstate.IsKeyDown(InputConfig.Down.key))
            return -1;

        return 0;
    }

    /// <summary>
    /// Проверяет, нажата ли кнопка
    /// </summary>
    /// <param name="key">Кнопка, которая должна быть нажата</param>
    public static bool IsKeyDown(Keys key)
    {
        return ConsoleAdventure.kstate.IsKeyDown(key);
    }

    /// <summary>
    /// Проверяет, нажата ли кнопка
    /// </summary>
    /// <param name="key">Кнопка, которая должна быть нажата</param>
    public static bool IsKeyDown(Key key)
    {
        return ConsoleAdventure.kstate.IsKeyDown(key.key);
    }

    /// <summary>
    /// Проверяет, нажата ли кнопка тик назад
    /// </summary>
    /// <param name="key">Кнопка, которая должна быть нажата</param>
    public static bool IsOldKeyDown(Keys key)
    {
        return ConsoleAdventure.prekstate.IsKeyDown(key);
    }

    /// <summary>
    /// Проверяет, нажата ли кнопка тик назад
    /// </summary>
    /// <param name="key">Кнопка, которая должна быть нажата</param>
    public static bool IsOldKeyDown(Key key)
    {
        return ConsoleAdventure.prekstate.IsKeyDown(key.key);
    }

    /// <summary>
    /// Проверяет, отжал ли пользователь кнопку
    /// </summary>
    /// <param name="key">Кнопка, которая должна быть нажата</param>
    public static bool PostClick(Keys key)
    {
        return !IsKeyDown(key) && IsOldKeyDown(key);
    }

    /// <summary>
    /// Проверяет, отжал ли пользователь кнопку
    /// </summary>
    /// <param name="key">Кнопка, которая должна быть нажата</param>
    public static bool PostClick(Key key)
    {
        return !IsKeyDown(key) && IsOldKeyDown(key);
    }

    /// <summary>
    /// Проверяет, клацнул ли пользователь на кнопку
    /// </summary>
    /// <param name="key">Кнопка, которая должна быть нажата</param>
    public static bool OnClick(Keys key)
    {
        return IsKeyDown(key) && !IsOldKeyDown(key);
    }

    /// <summary>
    /// Проверяет, клацнул ли пользователь на кнопку
    /// </summary>
    /// <param name="key">Кнопка, которая должна быть нажата</param>
    public static bool OnClick(Key key)
    {
        return IsKeyDown(key) && !IsOldKeyDown(key);
    }


    [DllImport("user32.dll")]
    private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

    private const uint KEYEVENTF_KEYUP = 0x0002;

    /// <summary>
    /// Програмно отжать кнопку
    /// </summary>
    /// <param name="key">Кнопка, которую нужно отжать</param>
    public static void ReleaseKey(Keys key)
    {
        byte vkCode = (byte)(int)key;

        keybd_event(vkCode, 0, KEYEVENTF_KEYUP, 0);
    }

    /// <summary>
    /// Програмно отжать кнопку
    /// </summary>
    /// <param name="key">Кнопка, которую нужно отжать</param>
    public static void ReleaseKey(Key key)
    {
        ReleaseKey(key.key);
    }
}