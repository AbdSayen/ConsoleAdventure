namespace ConsoleAdventure.Content.Scripts.Entities.StateMachine.States;

public class RotationState : IState
{
    int timer;
    int randomTime = 0;
    int rotation = -1;
    int rotation1 = -1;
    
    public void Enter()
    {
        randomTime = Utils.StabilizeTicks(ConsoleAdventure.rand.Next(0, 180));
        rotation = ConsoleAdventure.rand.Next(-1, 4);
        rotation1 = ConsoleAdventure.rand.Next(-3, 4);

        while (true)
        {
            if(rotation1 == rotation)
            {
                rotation1 = ConsoleAdventure.rand.Next(-3, 4);
            }
            else
            {
                break;
            }
        }
    }

    public void Exit()
    {
        
    }
}