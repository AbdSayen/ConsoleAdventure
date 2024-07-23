using Microsoft.Xna.Framework.Input;

namespace ConsoleAdventure.Content.Scripts.InputLogic;

public static class InputConfig
{
    #region Movement

    public static Keys Up = Keys.W;
    public static Keys Down = Keys.S;
    public static Keys Left = Keys.A;
    public static Keys Right = Keys.D;
    public static Keys Run = Keys.LeftShift;

    #endregion

    #region Building

    public static Keys Building = Keys.B;
    public static Keys Destroying = Keys.V;

    #endregion
    
    public static Keys PickUp = Keys.P;
    public static Keys Clear = Keys.C;
    public static Keys Enter = Keys.Enter;

}