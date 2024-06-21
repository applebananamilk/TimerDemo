namespace AsyncTimerDemo;

public sealed class AsyncTimer
{
    public Func<AsyncTimer, Task> Elapsed = _ => Task.CompletedTask;

    public int Period { get; set; }

    public bool RunOnStart { get; set; }

    private readonly Timer _taskTimer;
    private volatile bool _performingTasks;
    private volatile bool _isRunning;

    public AsyncTimer()
    {
        _taskTimer = new Timer(
            TimerCallBack!,
            null,
            Timeout.Infinite,
            Timeout.Infinite
        );
    }

    public void Start()
    {
        if (Period <= 0)
        {
            throw new ArgumentException("Period should be set before starting the timer!");
        }

        lock (_taskTimer)
        {
            _taskTimer.Change(RunOnStart ? 0 : Period, Timeout.Infinite);
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
                    _taskTimer.Change(Period, Timeout.Infinite);
                }

                Monitor.Pulse(_taskTimer);
            }
        }
    }
}