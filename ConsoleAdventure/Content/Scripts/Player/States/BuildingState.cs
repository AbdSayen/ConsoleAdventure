using ConsoleAdventure.Content.Scripts.Abstracts;
using ConsoleAdventure.Content.Scripts.InputLogic;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework.Input;

namespace ConsoleAdventure.Content.Scripts.Player.States;

public class BuildingState : IPlayerState
{
    private Player player;

    public BuildingState(Player player)
    {
        this.player = player;
    }

    public void HandleInput()
    {
        player.Cursor.CursorMovement();

        if (Input.IsKeyDown(InputConfig.Enter))
        {
            Position pos = new Position(player.position.x + player.cursorPosition.x, player.position.y + player.cursorPosition.y);

            if (pos.x > 0 && pos.x < player.world.size && pos.y > 0 && pos.y < player.world.size &&
                player.world.GetField(pos.x, pos.y, World.BlocksLayerId).content == null)
            {
                new Plank(player.world, pos);
                player.inventory.RemoveItems(new Log(), 1);
                player.world.time.PassTime(120);
            }
        }
    }
}