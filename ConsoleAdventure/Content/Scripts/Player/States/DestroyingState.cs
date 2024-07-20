using ConsoleAdventure.Content.Scripts.Abstracts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework.Input;

namespace ConsoleAdventure.Content.Scripts.Player.States;

public class DestroyingState : IPlayerState
{
    private Player player;

    public DestroyingState(Player player)
    {
        this.player = player;
    }

    public void HandleInput()
    {
        player.Cursor.CursorMovement();

        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
        {
            Position pos = new Position(player.position.x + player.cursorPosition.x, player.position.y + player.cursorPosition.y);

            if (pos.x > 0 && pos.x < player.world.size && pos.y > 0 && pos.y < player.world.size &&
                player.world.GetField(pos.x, pos.y, World.BlocksLayerId).content != null)
            {
                player.world.RemoveSubject(player.world.GetField(pos.x, pos.y, World.BlocksLayerId).content, World.BlocksLayerId);
                player.world.time.PassTime(60);
            }
        }
    }
}