namespace ConsoleAdventure.Content.Scripts.Entities.StateMachine;

public interface IState
{
    public void Enter();
    public void InteractWithWorld();
    public void Exit();
}