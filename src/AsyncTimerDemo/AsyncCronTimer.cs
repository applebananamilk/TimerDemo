using TimeCrontab;

namespace AsyncTimerDemo;

public sealed class AsyncCronTimer
{
    public Func<AsyncCronTimer, Task> Elapsed = _ => Task.CompletedTask;

    public bool RunOnStart { get; set; }

    private readonly Timer _taskTimer;
    private readonly Crontab _crontab;

    private volatile bool _performingTasks;
    private volatile bool _isRunning;

    public AsyncCronTimer(string cronExpression)
    {
        if (cronExpression == null)
        {
            throw new ArgumentException("cronExpression cannot be null", nameof(cronExpression));
        }

        if (!Crontab.IsValid(cronExpression, CronStringFormat.WithSeconds))
        {
            throw new ArgumentException("cronExpression is invalid", nameof(cronExpression));
        }

        _crontab = Crontab.Parse(cronExpression, CronStringFormat.WithSeconds);

        _taskTimer = new Timer(
            TimerCallBack!,
            null,
            Timeout.Infinite,
            Timeout.Infinite
        );
    }

    public void Start()
    {
        lock (_taskTimer)
        {
            _taskTimer.Change(RunOnStart ? 0 : GetPeriod(), Timeout.Infinite);
            _isRunning = true;
        }
    }

    public void Stop()
    {
        lock (_taskTimer)
        {
            _taskTimer.Change(Timeout.Infinite, Timeout.Infinite);
            while (_performingTasks)
            {
                Monitor.Wait(_taskTimer);
            }

            _isRunning = false;
        }
    }

    private void TimerCallBack(object state)
    {
        lock (_taskTimer)
        {
            if (!_isRunning || _performingTasks)
            {
                return;
            }

            _taskTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _performingTasks = true;
        }

        _ = Timer_Elapsed();
    }

    private async Task Timer_Elapsed()
    {
        try
        {
            await Elapsed(this);
        }
        catch (Exception ex)
        {
            // TODO Logger
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            lock (_taskTimer)
            {
                _performingTasks = false;
                if (_isRunning)
                {
                    _taskTimer.Change(GetPeriod(), Timeout.Infinite);
                }

                Monitor.Pulse(_taskTimer);
            }
        }
    }

    private int GetPeriod()
    {
        return (int)_crontab.GetSleepMilliseconds(DateTime.Now);
    }
}
