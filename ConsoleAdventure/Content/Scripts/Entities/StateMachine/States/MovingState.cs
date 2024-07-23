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
        if(rotation > -1)
        {
            _entity.Move(1, (Rotation)(rotation * 2));
            if (rotation1 > -1)
            {
                _entity.Move(1, (Rotation)(rotation1 * 2));
            }

            //_entity.СhooseColor(colors, _entity.position, index);
        }
    }

    public void Exit()
    {
        
    }
}