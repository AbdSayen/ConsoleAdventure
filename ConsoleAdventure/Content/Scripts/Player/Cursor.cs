using ConsoleAdventure.Content.Scripts.InputLogic;

namespace ConsoleAdventure.Content.Scripts.Player;

public class Cursor
{
    public Position CursorPosition;
    public bool IsActive { get; set; }
    
    public static Cursor Instance => _instance;

    private static Cursor _instance;
    
    public Cursor()
    {
        CursorPosition = Position.Zero();

        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void Toggle()
    {
        if (IsActive) IsActive = false;
        else IsActive = true;
    }

    public void CursorMovement()
    {
        if (Input.IsKeyDown(InputConfig.CursorUp) && CursorPosition.y > -2)
        {
            CursorPosition.SetPosition(CursorPosition.x, CursorPosition.y - 1);
        }

        if (Input.IsKeyDown(InputConfig.CursorDown) && CursorPosition.y < 2)
        {
            CursorPosition.SetPosition(CursorPosition.x, CursorPosition.y + 1);
        }

        if (Input.IsKeyDown(InputConfig.CursorLeft) && CursorPosition.x > -2)
        {
            CursorPosition.SetPosition(CursorPosition.x - 1, CursorPosition.y);
        }

        if (Input.IsKeyDown(InputConfig.CursorRight) && CursorPosition.x < 2)
        {
            CursorPosition.SetPosition(CursorPosition.x + 1, CursorPosition.y);
        }
    }
}