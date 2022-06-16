

using System.Collections.Concurrent;
using Moorhuhn_Console;

public class Entry
{
    private bool IsRunning { get; set; } = true;
    private ConcurrentQueue<ConsoleKeyInfo> Inputs { get; } = new();

    private List<Char> inputHistory { get; } = new();

    // private Renderer renderer { get; } = new Renderer(Console.WindowWidth, Console.WindowHeight, 2);

    public static void Main(string[] args)
    {
        new Entry().LetsGo(args);
        
    }

    public void LetsGo(string[] strings)
    {
        StartAndRunInputLoop();
        RunGameLoop();
    }

    private void StartAndRunInputLoop()
    {
        Task.Factory.StartNew(() =>
        {
            while (IsRunning)
            {
                var playerInput = Console.ReadKey();
                Inputs.Enqueue(playerInput);
            }
        });
    }

    private void RunGameLoop()
    {
        while (IsRunning)
        {
            var inputChar = Inputs.TryDequeue(out ConsoleKeyInfo keyInfo);
            if (inputChar)
            {
                inputHistory.Add(keyInfo.KeyChar);
            }
            // Console.WriteLine(String.Join("",inputHistory));


            var windowWidth = Console.WindowWidth;
            var windowHeight = Console.WindowHeight;
            try
            {
                new Renderer(windowWidth, windowHeight, 2).Draw();
            }
            catch (Exception e)
            {
                //ignore
            }
            Thread.Sleep(100);
        }
    }
}