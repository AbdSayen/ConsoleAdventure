using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure.Content.Scripts
{
    public class Entity : Transform
    {
        public int life;
        public int maxLife;
        public int damage;

        public Entity(World world, Position position = null) : base(world, position)
        {
            worldLayer = World.MobsLayerId;
            renderFieldType = RenderFieldType.entity;
            isObstacle = false;
        }

        public void InteractWithWorld()
        {
            AI();

            //if(life <= 0) Kill();
        }
        
        public void Kill()
        {
            ConsoleAdventure.world.RemoveSubject(this, worldLayer);
        }

        public virtual void AI()
        {
        }

        public void SetMaxLife(int life)
        {
            this.life = life;
            maxLife = life;
        }

        public void СhooseColor(Color[] colors, Position position, int index)
        {
            world.GetField(position.x, position.y, World.MobsLayerId).color = colors[index];
        }
    }
}
