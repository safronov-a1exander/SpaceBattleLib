using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

class RecieverAdapter : IReciver
{
    BlockingCollection<ICommand> queue;

    public RecieverAdapter(BlockingCollection<ICommand> queue) => this.queue = queue;

    public ICommand Receive()
    {
        return queue.Take();
    }

    public bool isEmpty()
    {
        return queue.Count() == 0;
    }
}
