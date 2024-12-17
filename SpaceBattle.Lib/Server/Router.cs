namespace SpaceBattle.Lib;
using System.Collections.Generic;

public class Router : IRouter{

    private Dictionary<string, ISender> routeDict;

    public Router(Dictionary<string, ISender> dict){
        this.routeDict = dict;
    }

    public bool Route(string id, string command, Dictionary<string, object> payload){
        try{
            routeDict[id.Split('.')[0]].Send(new DeserializeSendCommand(id, command, payload));
            return true;
        }
        catch {
            return false;
        }
    }
}
