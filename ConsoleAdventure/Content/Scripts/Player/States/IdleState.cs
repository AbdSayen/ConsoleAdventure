using ConsoleAdventure.Content.Scripts.InputLogic;
using Microsoft.Xna.Framework.Input;

namespace ConsoleAdventure.Content.Scripts.Player.States;

public class IdleState : IPlayerState
{
    private Player player;

    public IdleState(Player player)
    {
        this.player = player;
    }

    public void HandleInput()
    {
        if (Input.IsKeyDown(InputConfig.Building) && player.Inventory.HasItem(new Log(), 1))
        {
            player.ChangeState(new BuildingState(player));
        }
        else if (Input.IsKeyDown(InputConfig.Destroying))
        {
            player.ChangeState(new DestroyingState(player));
        }
    }

    public void Enter()
    {
        
    }

    public void Exit()
    {
        
    }
}