using ConsoleAdventure.Content.Scripts.Abstracts;
using Microsoft.Xna.Framework.Input;

namespace ConsoleAdventure.Content.Scripts.Player;

public class Cursor
{
    public Position _cursorPosition;

    public Cursor()
    {
        _cursorPosition = new Position(0, 0);
    }
    
    public void CursorMovement()
    {
        _cursorPosition = Position.Zero();

        if (Keyboard.GetState().IsKeyDown(Keys.Up) && _cursorPosition.y > -2)
        {
            _cursorPosition.SetPosition(_cursorPosition.x, _cursorPosition.y - 1);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Down) && _cursorPosition.y < 2)
        {
            _cursorPosition.SetPosition(_cursorPosition.x, _cursorPosition.y + 1);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Left) && _cursorPosition.x > -2)
        {
            _cursorPosition.SetPosition(_cursorPosition.x - 1, _cursorPosition.y);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Right) && _cursorPosition.x < 2)
        {
            _cursorPosition.SetPosition(_cursorPosition.x + 1, _cursorPosition.y);
        }
    }
}