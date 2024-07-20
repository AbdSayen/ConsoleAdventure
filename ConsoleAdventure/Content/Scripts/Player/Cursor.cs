using ConsoleAdventure.Content.Scripts.Abstracts;
using ConsoleAdventure.Content.Scripts.InputLogic;
using Microsoft.Xna.Framework.Input;

namespace ConsoleAdventure.Content.Scripts.Player;

public class Cursor
{
    public Position CursorPosition;

    public Cursor()
    {
        CursorPosition = Position.Zero();
    }
    
    public void CursorMovement()
    {
        CursorPosition = Position.Zero();

        if (Input.IsKeyDown(InputConfig.Up) && CursorPosition.y > -2)
        {
            CursorPosition.SetPosition(CursorPosition.x, CursorPosition.y - 1);
        }
        if (Input.IsKeyDown(InputConfig.Down) && CursorPosition.y < 2)
        {
            CursorPosition.SetPosition(CursorPosition.x, CursorPosition.y + 1);
        }
        if (Input.IsKeyDown(InputConfig.Left) && CursorPosition.x > -2)
        {
            CursorPosition.SetPosition(CursorPosition.x - 1, CursorPosition.y);
        }
        if (Input.IsKeyDown(InputConfig.Right) && CursorPosition.x < 2)
        {
            CursorPosition.SetPosition(CursorPosition.x + 1, CursorPosition.y);
        }
    }
}