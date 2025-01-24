using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleAdventure.Content.Scripts.Player;

public class PlayerInfo
{
    public short Id;
    public string pcId;
    public string Name = "William";

    public PlayerInfo(Dictionary<string, string> data)
    {
        data.TryGetValue("name", out Name);
        data.TryGetValue("pcId", out pcId);
        string strId = "";
        data.TryGetValue("id", out strId);
        Id = Int16.Parse(strId);
    }

    public PlayerInfo()
    {

    }
}