namespace SpaceBattle.Lib.gRPC;
using SpaceBattle.Lib;
public class CreateEndpointStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        return new CreateEndpointCommand();
    }
}
