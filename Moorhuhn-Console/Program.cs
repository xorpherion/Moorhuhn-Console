using System.Collections.Concurrent;
using System.Diagnostics;
using Moorhuhn_Console;

public class Entry
{
    private bool IsRunning { get; set; } = true;
    private ConcurrentQueue<ConsoleKeyInfo> Inputs { get; } = new();

    private List<Char> inputHistory { get; } = new();

    // private Renderer renderer { get; } = new Renderer(Console.WindowWidth, Console.WindowHeight, 2);

    private int currentColumn = 0;
    private int currentColumnX = 0;
    private int currentColumnY = 0;
    private bool nextColumn = true;

    private Stopwatch playTimer = new Stopwatch();

    private int score = 0;

    public static void Main(string[] args)
    {
        new Entry().LetsGo(args);
    }

    public void LetsGo(string[] strings)
    {
        StartAndRunInputLoop();
        RunGameLoop(TimeSpan.FromSeconds(10));
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

    private void RunGameLoop(TimeSpan MaxPlayTime)
    {
        var windowWidth = Console.WindowWidth;
        var windowHeight = Console.WindowHeight;

        while (true)
        {
            Console.Clear();
            score = 0;
            playTimer.Reset();
            playTimer.Start();
            nextColumn = true;
            IsRunning = true;
            while (IsRunning)
            {
                if (playTimer.Elapsed > MaxPlayTime)
                    IsRunning = false;

                var inputChar = Inputs.TryDequeue(out ConsoleKeyInfo keyInfo);
                if (inputChar)
                {
                    inputHistory.Add(keyInfo.KeyChar);
                    if (keyInfo.KeyChar.ToString().Equals(currentColumn.ToString()))
                    {
                        score++;
                        nextColumn = true;
                    }
                }
                // Console.WriteLine(String.Join("",inputHistory));


                HandleWindowSizeChange(ref windowHeight, ref windowWidth);

                var renderer = new Renderer(windowWidth, windowHeight);
                var height = renderer.Pixels.Length - 1;
                var width = renderer.Pixels[0].Length - 1;

                var columns = 5;
                var columnSize = width / columns;

                SetNextPointIfNeeded(columns, columnSize, height);
                try
                {
                    RenderGame(columns, height, renderer, columnSize, currentColumnX, currentColumnY);
                    renderer.Draw(score, playTimer, MaxPlayTime);
                }
                catch (Exception e)
                {
                    //ignore
                }

                Thread.Sleep(100);
            }

            Console.WriteLine($"Spiel zuende, deine Punkte sind: {score}. Drück Enter für noch eine Runde");
            Console.ReadLine();
        }
    }

    private void SetNextPointIfNeeded(int columns, int columnSize, int height)
    {
        if (nextColumn)
        {
            var random = new Random();
            currentColumn = Math.Abs(random.Next()) % columns;
            currentColumnX = currentColumn * columnSize + (Math.Abs(random.Next()) % (columnSize - 2)) + 2;
            currentColumnY = Math.Abs(random.Next()) % (height - 10);
            nextColumn = false;
        }
    }

    private static void RenderGame(int columns, int height, Renderer renderer, int columnSize, int currentColumnX,
        int currentColumnY)
    {
        for (int x = 0; x < columns + 1; x++)
        for (int y = 0; y < height - 5; y++)
            renderer.Pixels[y][x * columnSize] = '#';

        for (int x = 0; x < columns; x++)
            renderer.Pixels[height - 4][x * columnSize + columnSize / 2] = Convert.ToChar(48 + x);

        renderer.Pixels[currentColumnY][currentColumnX] = '+';
    }

    private static void HandleWindowSizeChange(ref int windowHeight, ref int windowWidth)
    {
        var newWindowWidth = Console.WindowWidth;
        var newWindowHeight = Console.WindowHeight;

        if (newWindowHeight != windowHeight || newWindowWidth != windowWidth)
            Console.Clear();

        windowWidth = newWindowWidth;
        windowHeight = newWindowHeight;
    }
}