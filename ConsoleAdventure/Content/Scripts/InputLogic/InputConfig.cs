using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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

    public static Keys CursorUp = Keys.Up;
    public static Keys CursorDown = Keys.Down;
    public static Keys CursorLeft = Keys.Left;
    public static Keys CursorRight = Keys.Right;

    #endregion

    public static Keys WorldGen = Keys.N;
    public static Keys WorldExit = Keys.Escape;
    public static Keys PickUp = Keys.L;
    public static Keys Cursor = Keys.Space;
    public static Keys Pause = Keys.P;
    public static Keys Cmd = Keys.OemTilde;
    public static Keys Interaction = Keys.E;   
    public static Keys InventoryPlus = Keys.OemPlus;
    public static Keys InventoryMinus = Keys.OemMinus;
    public static Keys ChestPlus = Keys.NumPad6;
    public static Keys ChestMinus = Keys.NumPad4;
    public static Keys RecipeOpen = Keys.R;
    public static Keys RecipeBookOpen = Keys.C;
    public static Keys RecipeListLeft = Keys.NumPad7;
    public static Keys RecipeListRight = Keys.NumPad9;
    public static Keys DropItem = Keys.Q;
    public static Keys TakeInInventory = Keys.U;
    public static Keys TakeInChest = Keys.J;
    public static Keys TakeInInventoryStack = Keys.I;
    public static Keys TakeInChestStack = Keys.O;
    public static Keys Use = Keys.B;
    public static Keys BuffsPlus = Keys.NumPad3;
    public static Keys BuffsMinus = Keys.NumPad1;
}