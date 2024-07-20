using ConsoleAdventure.Content.Scripts.Abstracts;
using ConsoleAdventure.Content.Scripts.InputLogic;

namespace ConsoleAdventure.Content.Scripts.Player;

public class PlayerMovement
{
    public bool IsMoving { get; private set; }
    public int Speed { get; set; }
    
    private readonly Position _direction;
    private readonly Player _player;
    
    private int x;
    private int y;
    
    public PlayerMovement(Transform player, int speed = 1)
    {
        _direction = new Position();
        Speed = speed;
    }

    public void Move()
    {
        _player.Move(Speed, GetDirection());
    }
    
    private Position GetDirection()
    {
        x = Input.GetHorizontalMovement();
        y = Input.GetVerticalMovement();
        
        _direction.SetPosition(x, y);
        
        x = 0;
        y = 0;

        if (_direction.x != 0 || _direction.y != 0)
        {
            IsMoving = true;
        }
        
        return _direction;
    }
}