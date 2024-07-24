using ConsoleAdventure.Content.Scripts.Entities.StateMachine.States;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure.Content.Scripts.Entities.StateMachine;

public class StateMachine
{
    private Entity _owner;
    
    private MovingState _movingState;
    private RotationState _rotationState;
    
    private IState _currentState;

    public StateMachine(Entity owner)
    {
        _owner = owner;
        _movingState = new MovingState(_owner);
        _rotationState = new RotationState(_owner);
    }
    
    public void ChangeState(StatesEnum state)
    {
        _currentState?.Exit();
        
        switch (state)
        {
            case StatesEnum.Moving:
                _currentState = _movingState;
                break;
            case StatesEnum.Rotation:
                _currentState = _rotationState;
                break;
        }
        
        _currentState?.Enter();
    }
}