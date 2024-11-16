using ConsoleAdventure.Content.Scripts.Player;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.WorldEngine.Events
{
    public class Rain : GameEvent
    {
        internal List<RainDrop> rainDrops = new();
        public float rainForce = 0f; //0f-1f

        public override void Update()
        {
            Player.Player player = ConsoleAdventure.world.GetLocalPlayer();

            if ((float)ConsoleAdventure.rand.NextDouble() < rainForce)
            {
                int lifeTime = 30;
                Position position = new Position(ConsoleAdventure.rand.Next(player.position.x - 30, player.position.x + 30), ConsoleAdventure.rand.Next(player.position.y - 15, player.position.y + 15));
                Field tryField = ConsoleAdventure.world.GetField(position.x, position.y, World.FloorLayerId, player.w);
                if (tryField != null && tryField?.content == null && position.x >= 0 && position.x < ConsoleAdventure.world.size && position.y >= 0 && position.y < ConsoleAdventure.world.size)
                    rainDrops.Add(new RainDrop(position, "·", lifeTime));
            }

            for (int i = 0; i < rainDrops.Count; i++)
            {
                RainDrop drop = rainDrops[i];

                if (drop.timer <= 0)
                {
                    rainDrops.RemoveAt(i);
                    i--;
                }

                drop.timer--;
            }
        }

        int delay = 360;
        public override bool IsActive()
        {
            if (delay <= 0)
            {
                rainForce += (float)(ConsoleAdventure.rand.NextDouble() / 10) * ConsoleAdventure.rand.Next(-1, 1);

                if (rainForce >= 1)
                    rainForce = 0;

                delay = ConsoleAdventure.rand.Next(60 * 10, 60 * 60);
            }

            delay--;

            return rainForce > 0;
        }

        public override void Draw()
        {
            Player.Player player = ConsoleAdventure.world.GetLocalPlayer();

            if (player.w == 1)
            {
                for (int i = 0; i < rainDrops.Count; i++)
                {
                    Position dropPos = rainDrops[i].position;
                    if (dropPos.x > player.position.x - 30 && dropPos.x < player.position.x + 30 && dropPos.y > player.position.y - 15 && dropPos.y < player.position.y + 15)
                    {
                        StringPaint.Draw(rainDrops[i].symbol, dropPos, player.w, new(), Color.Blue);
                    }
                }
            }
        }
    }

    public class RainDrop
    {
        public Position position = new();
        public string symbol = "";
        public int timer;

        public RainDrop(Position position, string symbol, int timer)
        {
            this.position = position;
            this.symbol = symbol;
            this.timer = timer;
        }
    }
}
