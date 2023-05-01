namespace SpaceBattle.Lib.gRPC;
using SpaceBattle.Lib;
public class MakeEndpointStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        return new MakeEndpointCommand(args);
    }
}
