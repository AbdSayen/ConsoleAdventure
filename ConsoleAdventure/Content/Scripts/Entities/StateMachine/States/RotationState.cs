namespace ConsoleAdventure.Content.Scripts.Entities.StateMachine.States;

public class RotationState : IState
{
    int timer;
    int randomTime = 0;
    int rotation = -1;
    int rotation1 = -1;

    private Entity entity;

    public RotationState(Entity entity)
    {
        this.entity = entity;
    }
    
    public void Enter()
    {
        rotation = ConsoleAdventure.rand.Next(-1, 4);
        rotation1 = ConsoleAdventure.rand.Next(-3, 4);
        
        entity.StateMachine.ChangeState(StatesEnum.Moving);
    }

    public void Exit()
    {
        
    }
}