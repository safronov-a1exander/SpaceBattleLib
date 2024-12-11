namespace SpaceBattle.Lib;
using System.Collections.Generic;

public interface IRouter{
    public bool Route(string id, string command, Dictionary<string, object> payload);
}
