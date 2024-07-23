using ConsoleAdventure.Content.Scripts.InputLogic;

namespace ConsoleAdventure.Content.Scripts.Player;

public class PlayerMovement
{
    public bool IsMoving { get; private set; }
    public int Speed { get; set; }
    
    private int x;
    private int y;
    
    public PlayerMovement(int speed = 1)
    {
        Speed = speed;
    }

    public void Move(Transform target)
    {
        target.Move(Speed, GetDirection());
    }
    
    public Position GetDirection()
    {
        x = 0;
        y = 0;
        
        x = Input.GetHorizontalMovement();
        y = Input.GetVerticalMovement();

        if (x != 0 || y != 0)
        {
            IsMoving = true;
        }
        
        return new Position(x ,y);
    }
}