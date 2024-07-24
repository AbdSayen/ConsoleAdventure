using System;

namespace ConsoleAdventure.Content.Scripts.Entities.StateMachine.States;

public class MovingState : IState
{
    private readonly Entity _entity;

    public MovingState(Entity entity)
    {
        _entity = entity;
    }
    
    public void Enter()
    {

    }

    public void InteractWithWorld()
    {
        _entity.Move(1, new Position(ConsoleAdventure.rand.Next(-15, 15), ConsoleAdventure.rand.Next(-15, 15)));
        _entity.Color.ChooseColor(_entity.position);
        
        if (ConsoleAdventure.rand.Next(-100, 100) == 0)
        {
            _entity.StateMachine.ChangeState(StatesEnum.Idle);
        }
    }

    public void Exit()
    {
        
    }
}