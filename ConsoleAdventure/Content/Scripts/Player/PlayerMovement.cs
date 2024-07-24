using ConsoleAdventure.Content.Scripts.InputLogic;

namespace ConsoleAdventure.Content.Scripts.Player;

public class PlayerMovement
{
    public bool isMoving { get; private set; }
    public int speed { get; set; }
    
    private int x;
    private int y;

    private Position _direction;
    
    public PlayerMovement(int speed = 1)
    {
        this.speed = speed;
    }

    public void Move(Transform target)
    {
        target.Move(speed, GetDirection());
    }
    
    private Position GetDirection()
    {
        x = 0;
        y = 0;
        
        _direction = Position.Zero();
        
        _direction.x = Input.GetHorizontalMovement();
        _direction.y = Input.GetVerticalMovement();

        if (x != 0 || y != 0)
        {
            isMoving = true;
        }
        
        return _direction;
    }
}