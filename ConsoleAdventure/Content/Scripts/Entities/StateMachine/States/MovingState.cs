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
        _entity.Move(1, new Position(ConsoleAdventure.rand.Next(-15, 15), ConsoleAdventure.rand.Next(-15, 15)));
        _entity.EntityColor.ChooseColor(_entity.position, _entity.w);
        _entity.StateMachine.ChangeState(StatesEnum.Idle);
    }

    public void InteractWithWorld()
    {

    }

    public void Exit()
    {
        
    }
}