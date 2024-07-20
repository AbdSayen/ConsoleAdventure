using System;
using ConsoleAdventure.Content.Scripts.Abstracts;
using ConsoleAdventure.Content.Scripts.InputLogic;

namespace ConsoleAdventure.Content.Scripts.Player;

public class PlayerMovement
{
    public bool IsMoving { get; private set; }
    
    private readonly Position _direction;
    private int x;
    private int y;
    
    public PlayerMovement()
    {
        _direction = new Position();
    }

    public Position GetDirection()
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

        //Console.WriteLine($"{_direction.x} {_direction.y} | {_direction.Magnitude()} | {_direction.Normalize().x} {_direction.Normalize().y}");
        
        return _direction;
    }
}