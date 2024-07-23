using ConsoleAdventure.Content.Scripts.InputLogic;

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
        if (Input.IsKeyDown(InputConfig.BuildingUp) && CursorPosition.y > -2)
        {
            CursorPosition.SetPosition(CursorPosition.x, CursorPosition.y - 1);
        }
        if (Input.IsKeyDown(InputConfig.BuildingDown) && CursorPosition.y < 2)
        {
            CursorPosition.SetPosition(CursorPosition.x, CursorPosition.y + 1);
        }
        if (Input.IsKeyDown(InputConfig.BuildingLeft) && CursorPosition.x > -2)
        {
            CursorPosition.SetPosition(CursorPosition.x - 1, CursorPosition.y);
        }
        if (Input.IsKeyDown(InputConfig.BuildingRight) && CursorPosition.x < 2)
        {
            CursorPosition.SetPosition(CursorPosition.x + 1, CursorPosition.y);
        }
    }
}