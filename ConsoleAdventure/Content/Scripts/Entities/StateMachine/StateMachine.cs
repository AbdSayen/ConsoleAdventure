using ConsoleAdventure.Content.Scripts.Entities.StateMachine.States;

namespace ConsoleAdventure.Content.Scripts.Entities.StateMachine;

public class StateMachine
{
    public IState CurrentState { get; private set; }
    
    private Entity _owner;
    
    private MovingState _movingState;
    private IdleState _idleState;
    
    public StateMachine(Entity owner)
    {
        _owner = owner;
        _movingState = new MovingState(_owner);
        _idleState = new IdleState(_owner);
    }
    
    public void ChangeState(StatesEnum state)
    {
        CurrentState?.Exit();
        
        switch (state)
        {
            case StatesEnum.Moving:
                CurrentState = _movingState;
                break;
            case StatesEnum.Idle:
                CurrentState = _idleState;
                break;
        }
        
        CurrentState?.Enter();
    }
}