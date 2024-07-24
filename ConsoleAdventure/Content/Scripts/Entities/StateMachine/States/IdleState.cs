namespace ConsoleAdventure.Content.Scripts.Entities.StateMachine.States;

public class IdleState : IState
{
    private int _timer;
    private int _randomTime = 200;

    private Entity _entity;

    public IdleState(Entity entity)
    {
        _entity = entity;
    }
    
    public void Enter()
    {

    }

    public void InteractWithWorld()
    {
        if (_timer > _randomTime) 
        {
            _entity.StateMachine.ChangeState(StatesEnum.Moving);
        }

        _timer++;
    }

    public void Exit()
    {
        _timer = 0;
    }
}