using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure.Content.Scripts.Entities;

public class EntityColor
{
    public Color[] Colors = new Color[9]
    {
        new Color(50, 50, 50),
        new Color(131, 105, 44),
        new Color(193, 138, 45),
        new Color(243, 171, 51),
        new Color(140, 147, 153),
        new Color(255, 255, 255),
        new Color(196, 207, 211),
        new Color(250, 194, 45),
        new Color(240, 210, 80),
    };
    
    public int ColorIndex { get; set; }
    
    public EntityColor()
    {
        ColorIndex = ConsoleAdventure.rand.Next(0, Colors.Length);
    }
    
    public void ChooseColor(Position position)
    {
        SetColor(Colors[ColorIndex], position);
    }
        
    public void SetColor(Color color, Position position)
    {
        World.Instance.GetField(position.x, position.y, World.MobsLayerId).color = color;
    }
}