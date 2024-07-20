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
        if (Keyboard.GetState().IsKeyDown(Keys.B) && player.inventory.HasItem(new Log(), 1))
        {
            player.ChangeState(new BuildingState(player));
        }
        else if (Keyboard.GetState().IsKeyDown(Keys.V))
        {
            player.ChangeState(new DestroyingState(player));
        }
    }
}