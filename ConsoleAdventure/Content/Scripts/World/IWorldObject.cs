using Microsoft.Xna.Framework;

namespace ConsoleAdventure.WorldEngine
{
    public interface IWorldObject
    {
        void Initialize();
        void Collapse();
        string GetSymbol();
        Color GetColor();
    }
}
