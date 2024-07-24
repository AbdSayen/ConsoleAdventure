using Microsoft.Xna.Framework;

namespace ConsoleAdventure.Content.Scripts.Entities.StateMachine.States;

public class MovingState : IState
{
    private readonly Entity _entity;
    
    private int rotation = 0;
    private int rotation1 = 0;

    public MovingState(Entity entity)
    {
        _entity = entity;
    }
    
    public void Enter()
    {
        _entity.onMoveEnded += OnMoveEnded;
        
        if(rotation > -1)
        {
            _entity.Move(1, (Rotation)(rotation * 2));
        }
        else
        {
            _entity.Move(1, (Rotation)(rotation1 * 2));
        }

        _entity.ChooseColor();
    }

    private void OnMoveEnded()
    {
        _entity.StateMachine.ChangeState(StatesEnum.Rotation);
    }

    public void Exit()
    {
        _entity.onMoveEnded -= OnMoveEnded;
    }
}