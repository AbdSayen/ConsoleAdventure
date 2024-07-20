using Microsoft.Xna.Framework.Input;

namespace ConsoleAdventure.Content.Scripts.InputLogic;

public static class Input
{
    public static int GetHorizontalMovement()
    {
        if (IsKeyDown(InputConfig.Left))
        {
            return -1;
        }
        if (IsKeyDown(InputConfig.Right))
        {
            return 1;
        }

        return 0;
    }

    public static int GetVerticalMovement()
    {
        if (IsKeyDown(InputConfig.Up))
        {
            return 1;
        }
        if (IsKeyDown(InputConfig.Down))
        {
            return -1;
        }

        return 0;
    }

    public static bool IsKeyDown(Keys key)
    {
        return Keyboard.GetState().IsKeyDown(key);
    }
}