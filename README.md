## 异步Timer示例

```csharp
internal class Program
{
    static void Main(string[] args)
    {
        //var timer = new AsyncTimer();
        //timer.Period = 1000;
        //timer.Elapsed += TimerOnElapsed;

        //Console.WriteLine("Start " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        //timer.Start();

        var cronTimer = new AsyncCronTimer("0/5 * * * * ?");
        cronTimer.Elapsed += TimerOnElapsed;

        Console.WriteLine("Start " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        cronTimer.Start();

        Console.ReadLine();
    }

    static async Task TimerOnElapsed(AsyncTimer timer)
    {
        Console.WriteLine("TimerOnElapsed Begin " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        await Task.Delay(3000);
        Console.WriteLine("TimerOnElapsed End   " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        Console.WriteLine();
    }

    static async Task TimerOnElapsed(AsyncCronTimer timer)
    {
        Console.WriteLine("TimerOnElapsed Begin " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        await Task.Delay(3000);
        Console.WriteLine("TimerOnElapsed End   " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        Console.WriteLine();
    }
}
```

输出：

```txt
Start 2024-06-21 16:38:47.283
TimerOnElapsed Begin 2024-06-21 16:38:50.031
TimerOnElapsed End   2024-06-21 16:38:53.031

TimerOnElapsed Begin 2024-06-21 16:38:55.014
TimerOnElapsed End   2024-06-21 16:38:58.024

TimerOnElapsed Begin 2024-06-21 16:39:00.006
TimerOnElapsed End   2024-06-21 16:39:03.014
```

