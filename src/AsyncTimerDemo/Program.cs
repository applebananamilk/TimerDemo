namespace AsyncTimerDemo;

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
