namespace SpaceBattle.Lib;
using Hwdtech;
class GameCommand : ICommand
{
    Queue<ICommand> queue = new Queue<ICommand>(40);

    object scope;
    public void Execute()
    {
        IoC.Resolve<ICommand>("Scope.Current.Set", scope).Execute();

        while(если время выполнения игры меньше выделенного кванта)
        {
            //время начала операции - -таймер точного времени через тики процессора, чтобы интервалы были не рванык
            queue.Take().Execute();
            //время завершения операции
            //увеличить время выполнения игры
        }
    }

}